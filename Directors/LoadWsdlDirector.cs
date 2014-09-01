using System;
using System.Linq;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;
using WebServiceStudio;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebServiceTestStudio.Directors
{
    public class LoadWsdlDirector
    {
        private WsdlModel wsdlModel;
        private ITestStudioControl wsdlPathComboBox;
        ITestStudioControl proxyPropertyGrid;
        ITestStudioControl proxyDataGridView;
        private BindingList<string> fileHistory = new BindingList<string>();
        public BindingList<ProxyProperties> proxyUrlHistory = new BindingList<ProxyProperties>();
        private string fileHistoryLocation = Path.Combine(Application.UserAppDataPath, "fileHistory.xml");
        private string proxyHistoryLocation = Path.Combine(Application.UserAppDataPath, "proxyHistory.xml");
        private string proxyHistoryBin = Path.Combine(Application.UserAppDataPath, "proxyHistory.bin");

        public LoadWsdlDirector(
            ITestStudioControl wsdlPathComboBox, 
            WsdlModel wsdlModel, 
            ITestStudioControl proxyPropertyGrid = null,
            ITestStudioControl proxyDataGridView = null)
        {
            this.wsdlModel = wsdlModel;
            this.wsdlPathComboBox = wsdlPathComboBox;
            this.proxyPropertyGrid = proxyPropertyGrid;
            this.proxyDataGridView = proxyDataGridView;

            // Load Wsdl path history
            var fileHistSerializer = new XmlSerializer(typeof(BindingList<string>));
            if (File.Exists(fileHistoryLocation))
            {
                using (var stream = File.OpenRead(fileHistoryLocation))
                {
                    fileHistory = (BindingList<string>)(fileHistSerializer.Deserialize(stream));
                }
            }

            wsdlPathComboBox.Content = fileHistory;
            if (fileHistory.Any())
                wsdlPathComboBox.SelectedContentItem = fileHistory[0];

            this.wsdlPathComboBox.AddEventHandler("SelectionChangeCommitted",
                new EventHandler(selectionChangeCommitted_wsdlPathComboBox));

            // Load proxy history
            //var proxyHistSerializer = new XmlSerializer(typeof(List<SoapHttpClientProtocol>));
            //if (File.Exists(proxyHistoryLocation))
            //{
            //    using (var stream = File.OpenRead(proxyHistoryLocation))
            //    {
            //        proxyHistory = (List<SoapHttpClientProtocol>)(proxyHistSerializer.Deserialize(stream));
            //    }
            //}

            //proxyDataGrid.Content = proxyHistory;
            //if (proxyHistory.Any())
            //    wsdlPathComboBox.SelectedContentItem = proxyHistory[0];

            if (File.Exists(proxyHistoryBin))
            {
                using (Stream stream = File.Open(proxyHistoryBin, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    proxyUrlHistory = (BindingList<ProxyProperties>)bin.Deserialize(stream);                    
                }                
            }

            this.proxyDataGridView.AddEventHandler("SelectionChanged",
                new EventHandler(selectionChanged_proxyDataGridView));

            this.proxyDataGridView.AddEventHandler("DefaultValuesNeeded",
                new DataGridViewRowEventHandler(defaultValuesNeeded_proxyDataGridView));

            this.proxyPropertyGrid.AddEventHandler("PropertyValueChanged",
                new PropertyValueChangedEventHandler(propertyValueChanged_proxyPropertyGrid));
        }

        private void propertyValueChanged_proxyPropertyGrid(object s, PropertyValueChangedEventArgs e)
        {
            var properties = proxyPropertyGrid.Content as ProxyProperties;
            if (properties != null)
                properties.UpdateProxy(WsdlModel.Proxy);
        }


        private void selectionChanged_proxyDataGridView(object sender, EventArgs e)
        {
            var properties = proxyDataGridView.SelectedContentItem as ProxyProperties;
            if (properties != null)
            {
                var currentProperties = proxyPropertyGrid.Content as ProxyProperties;
                currentProperties.lastSelected = false;

                properties.lastSelected = true;
                proxyPropertyGrid.Content = properties;
                properties.UpdateProxy(WsdlModel.Proxy);
            }
        }

        ~LoadWsdlDirector()
        {
            var serializer = new XmlSerializer(typeof(BindingList<string>));
            using (var stream = File.OpenWrite(fileHistoryLocation))
            {
                serializer.Serialize(stream, fileHistory);
            }

            // Write proxy history
            //XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            //var proxyHistSerializer = new XmlSerializer(proxyUrlHistory.GetType(), overrides);
            //using (var stream = File.OpenWrite(proxyHistoryLocation))
            //{
            //    serializer.Serialize(stream, proxyUrlHistory);
            //}

            using (Stream stream = File.Open(proxyHistoryBin, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, proxyUrlHistory);
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
            WsdlModel.ProxyAssembly = wsdlModel.Wsdl.ProxyAssembly;

            GenerateClassesAndMethods();
            AssignProxy();

            

            //proxyHistory.Add(WsdlModel.Proxy as SoapHttpClientProtocol);
        }

        private void GenerateClassesAndMethods()
        {
            wsdlModel.Classes.Clear();
            wsdlModel.Methods.Clear();
            wsdlModel.MethodsByType.Clear();

            // Use next line if you want to be able to display methods from
            // all WS at once. 
            //wsdlModel.Classes.Add("All");

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

                wsdlModel.MethodsByType.Add(wsType, new BindingList<MethodInfo>(methodsList));
                wsdlModel.Classes.Add(wsType);

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

                        // DataGridView
                        var proxyProperties = new ProxyProperties(proxy);
                        proxyProperties.NickName = "Default";
                        if (!proxyUrlHistory.Contains(proxyProperties))
                            proxyUrlHistory.Add(proxyProperties);
                        proxyDataGridView.Content = proxyUrlHistory;// proxyUrlHistory;

                        // Proxy Property Grid
                        if (proxyPropertyGrid != null)
                            proxyPropertyGrid.Content = proxyProperties;
                        foreach (var history in proxyUrlHistory)
                        {
                            if (history.lastSelected)
                            {
                                history.lastSelected = false;
                                proxyPropertyGrid.Content = history;
                                history.UpdateProxy(WsdlModel.Proxy);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void defaultValuesNeeded_proxyDataGridView(object sender, DataGridViewRowEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView.Columns.Contains("CookieContainer"))
                e.Row.Cells["CookieContainer"].Value = new CookieContainer();
            if (dataGridView.Columns.Contains("Server"))
                e.Row.Cells["Server"].Value = new ProxyProperties.ServerProperties();
            if (dataGridView.Columns.Contains("Timeout"))
                e.Row.Cells["Timeout"].Value = 100000;
        }
    }
}
