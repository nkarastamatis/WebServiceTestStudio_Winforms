using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.UserInterface;
using System.Reflection;
using WebServiceTestStudio.Model;
using WebServiceStudio;

namespace WebServiceTestStudio.Directors
{
    public class InvokeDirector
    {
        private TestStudioCompositeControl invokeTab;
        private IList<MethodInfo> methods;

        public InvokeCompleteEventHandler InvokeComplete;

        public InvokeDirector(TestStudioCompositeControl invokeTab, IList<MethodInfo> methods)
        {
            this.invokeTab = invokeTab;
            this.methods = methods;
        }

        public void form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                if (!methods.Any())
                    return;
                var activeContent = invokeTab.SelectedChild;
                var activeControl = activeContent.Content as ITestStudioControl;                

                string searchString;
                var strings = activeControl.Label.Split(' ');
                if (strings != null && strings.Any())
                    searchString = strings[0];
                else
                    searchString = activeControl.Label;

                if (!(activeControl is TestStudioPropertyGrid))
                    activeControl = activeControl.Content as ITestStudioControl;

                try
                {
                    MethodInfo methodInfo = methods.Single(i => i.Name == searchString);

                    var dictionary = activeControl.Content as IDictionary<String, object>;

                    InvokeWebMethod(methodInfo, dictionary.Values.ToArray());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private object InvokeWebMethod(MethodInfo method, object[] parameters)
        {
            HttpWebClientProtocol proxy = WsdlModel.Proxy;
            object result = null;
            RequestProperties properties = new RequestProperties(proxy);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Type declaringType = method.DeclaringType;
                WSSWebRequest.RequestTrace = properties;
                result = method.Invoke(proxy, BindingFlags.Public, null, parameters, null);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
            finally
            {
                WSSWebRequest.RequestTrace = null;
                InvokeComplete(this, new InvokeCompleteEventArgs(method, properties, result));
            }

            return result;
        }
    
    
    }
}
