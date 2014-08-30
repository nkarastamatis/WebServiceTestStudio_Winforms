using System;
using System.Linq;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;
using WebServiceStudio;

namespace WebServiceTestStudio.Directors
{
    public class LoadWsdlDirector
    {
        private WsdlModel wsdlModel;
        private ITestStudioControl wsdlPathComboBox;
        ITestStudioControl proxyPropertyGrid;
        private BindingList<string> fileHistory = new BindingList<string>();
        private string fileHistoryLocation = Path.Combine(Application.UserAppDataPath, "fileHistory.xml");

        public LoadWsdlDirector(ITestStudioControl wsdlPathComboBox, WsdlModel wsdlModel, ITestStudioControl proxyPropertyGrid = null)
        {
            this.wsdlModel = wsdlModel;
            this.wsdlPathComboBox = wsdlPathComboBox;
            this.proxyPropertyGrid = proxyPropertyGrid;

            var serializer = new XmlSerializer(typeof(BindingList<string>));
            if (File.Exists(fileHistoryLocation))
            {
                using (var stream = File.OpenRead(fileHistoryLocation))
                {
                    fileHistory = (BindingList<string>)(serializer.Deserialize(stream));
                }
            }

            wsdlPathComboBox.Content = fileHistory;
            if (fileHistory.Any())
                wsdlPathComboBox.SelectedContentItem = fileHistory[0];

            this.wsdlPathComboBox.AddEventHandler("SelectionChangeCommitted",
                new EventHandler(selectionChangeCommitted_wsdlPathComboBox));
        }

        ~LoadWsdlDirector()
        {
            var serializer = new XmlSerializer(typeof(BindingList<string>));
            using (var stream = File.OpenWrite(fileHistoryLocation))
            {
                serializer.Serialize(stream, fileHistory);
            }
        }

        private void selectionChangeCommitted_wsdlPathComboBox(object sender, EventArgs e)
        {
            var iControl = sender as ITestStudioControl;
            var selection = iControl.SelectedContentItem as string;
            fileHistory.Remove(selection);
            fileHistory.Insert(0, selection);
            wsdlPathComboBox.SelectedContentItem = selection;
        }

        public void browse_Wsdl(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();            
            fileDialog.Filter = "WSDL Files|*.wsdl";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileHistory.Insert(0, fileDialog.FileName);
                wsdlPathComboBox.SelectedContentItem = fileDialog.FileName;
            }
        }

        public void load_Wsdl(object sender, EventArgs e)
        {
            UpdateWsdl();
        }

        private void UpdateWsdl()
        {
            Cursor.Current = Cursors.WaitCursor;

            var wsdl = new Wsdl();
            var path = wsdlPathComboBox.SelectedContentItem as string;
            wsdl.Paths.Add(path);
            wsdl.Generate();
            wsdlModel.Wsdl = wsdl;

            GenerateClassesAndMethods();
            AssignProxy();

            if (proxyPropertyGrid != null)
                proxyPropertyGrid.Content = WsdlModel.Proxy;
        }

        private void GenerateClassesAndMethods()
        {
            wsdlModel.Classes.Clear();
            wsdlModel.Methods.Clear();

            wsdlModel.Classes.Add("All");

            var types = wsdlModel.Wsdl.ProxyAssembly.GetTypes().OrderBy(a => a.Name);

            if (types.Count() == 0)
            {
                MessageBox.Show("Invalid Wsdl.");
                return;
            }

            // populate the methods first
            var wsTypes = types.Where(type => typeof(HttpWebClientProtocol).IsAssignableFrom(type));
            List<MethodInfo> allMethods = new List<MethodInfo>();
            foreach (var wsType in wsTypes)
            {
                var methods = wsType.GetMethods().Where(method => method.GetCustomAttributes(typeof(SoapDocumentMethodAttribute), true).Length > 0);
                var methodsList = methods.ToList();
                methodsList.Sort((m1, m2) => m1.Name.CompareTo(m2.Name));
                allMethods.AddRange(methodsList);
            }

            foreach (var method in allMethods)
                wsdlModel.Methods.Add(method);

            // now, only add the classes that are parameters of a method
            foreach (var type in types.Where(type => !typeof(HttpWebClientProtocol).IsAssignableFrom(type) && wsdlModel.GetMethodsByType(type).Any()))
            {
                wsdlModel.Classes.Add(type);
            }
        }

        public static bool IsWebMethod(MethodInfo method)
        {
            object[] customAttributes = method.GetCustomAttributes(typeof(SoapRpcMethodAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                return true;
            }
            customAttributes = method.GetCustomAttributes(typeof(SoapDocumentMethodAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                return true;
            }
            customAttributes = method.GetCustomAttributes(typeof(HttpMethodAttribute), true);
            return ((customAttributes != null) && (customAttributes.Length > 0));
        }

        private void AssignProxy()
        {
            Assembly proxyAssembly = wsdlModel.Wsdl.ProxyAssembly;
            if (proxyAssembly != null)
            {
                foreach (Type type in proxyAssembly.GetTypes())
                {
                    if (typeof(HttpWebClientProtocol).IsAssignableFrom(type))
                    {
                        HttpWebClientProtocol proxy = (HttpWebClientProtocol)Activator.CreateInstance(type);
                        proxy.Credentials = CredentialCache.DefaultCredentials;
                        SoapHttpClientProtocol protocol2 = proxy as SoapHttpClientProtocol;
                        if (protocol2 != null)
                        {
                            protocol2.CookieContainer = new CookieContainer();
                            protocol2.AllowAutoRedirect = true;
                        }
                        WsdlModel.Proxy = proxy;
                    }
                }
            }
        }

    }
}
