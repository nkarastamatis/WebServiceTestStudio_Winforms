using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using WebServiceStudio;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;


namespace WebServiceTestStudio.Directors
{
    public class ProxyInfoDirector
    {
        ITestStudioControl proxyPropertyGrid;
        ITestStudioControl proxyDataGridView;

        BindingList<ProxyProperties> proxyUrlHistory = new BindingList<ProxyProperties>();


        private string proxyHistoryLocation = Path.Combine(Application.UserAppDataPath, "proxyHistory.xml");
        private string proxyHistoryBin = Path.Combine(Application.UserAppDataPath, "proxyHistory.bin");


        public ProxyInfoDirector(
            ITestStudioControl proxyPropertyGrid,
            ITestStudioControl proxyDataGridView)
        {
            this.proxyPropertyGrid = proxyPropertyGrid;
            this.proxyDataGridView = proxyDataGridView;

            // Load proxy history
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

        ~ProxyInfoDirector()
        {
            using (Stream stream = File.Open(proxyHistoryBin, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, proxyUrlHistory);
            }
        }

        public void OnNewWebServiceAdded(object sender, NewWebServiceAddedEventArgs e)
        {
            var type = e.Type;

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
            proxyDataGridView.Content = proxyUrlHistory;

            // Proxy Property Grid
            if (proxyPropertyGrid != null)
                proxyPropertyGrid.Content = proxyProperties;

            foreach (var history in proxyUrlHistory)
            {
                if (history.lastSelected)
                {
                    proxyPropertyGrid.Content = history;
                    history.UpdateProxy(WsdlModel.Proxy);
                    break;
                }
            }
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
