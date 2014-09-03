using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using WebServiceStudio;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;

namespace WebServiceTestStudio.Directors
{
    public class LoadWsdlDirector
    {
        private WsdlModel wsdlModel;
        private ITestStudioControl wsdlPathComboBox;
        
        private BindingList<string> fileHistory = new BindingList<string>();
        private string fileHistoryLocation;

        public event NewWebServiceAddedEventHandler NewWebServiceAdded;

        public LoadWsdlDirector(
            ITestStudioControl wsdlPathComboBox, 
            WsdlModel wsdlModel,
            string fileHistoryLocation = null)
        {
            this.wsdlModel = wsdlModel;
            this.wsdlPathComboBox = wsdlPathComboBox;
            this.fileHistoryLocation = fileHistoryLocation;

            // Load Wsdl path history
            var fileHistSerializer = new XmlSerializer(typeof(BindingList<string>));
            if (File.Exists(fileHistoryLocation))
            {
                try
                {
                    using (var stream = File.OpenRead(fileHistoryLocation))
                    {
                        fileHistory = (BindingList<string>)(fileHistSerializer.Deserialize(stream));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not load file history {0}", ex.Message);
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

        public void FileSelected(string fileName)
        {
            fileHistory.Insert(0, fileName);
            wsdlPathComboBox.SelectedContentItem = fileName;
        }

        public void UpdateWsdl()
        {
            var path = wsdlPathComboBox.SelectedContentItem as string;
            wsdlModel.Initialize(path);

            // Notify other directors (i.e. WS Proxy editor)
            //if (NewWebServiceAdded != null)
              //  NewWebServiceAdded(this, new NewWebServiceAddedEventArgs(null));
        }
    }
}
