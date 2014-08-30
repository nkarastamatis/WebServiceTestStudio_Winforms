using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;
using WebServiceStudio;

namespace WebServiceTestStudio.Directors
{
    public class XmlTabDirector
    {
        private ITestStudioControl requestTextBox;
        private ITestStudioControl responseTextBox;

        public XmlTabDirector(ITestStudioControl requestTextBox, ITestStudioControl responseTextBox)
        {
            this.requestTextBox = requestTextBox;
            this.responseTextBox = responseTextBox;
        }

        public void OnInvokeComplete(object sender, InvokeCompleteEventArgs e)
        {
            Update(e.RequestProperties);
        }

        public void Update(RequestProperties properties)
        {
            requestTextBox.Content = properties.requestPayLoad;
            responseTextBox.Content = properties.responsePayLoad;
        }
    }
}
