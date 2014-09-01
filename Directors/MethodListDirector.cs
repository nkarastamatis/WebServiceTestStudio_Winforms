using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;

namespace WebServiceTestStudio.Directors
{
    public class MethodListDirector
    {
        ITestStudioControl methodFilterComboBox;
        ITestStudioControl methodsListBox;
        WsdlModel wsdlModel;

        public MethodListDirector(WsdlModel wsdlModel, ITestStudioControl methodFilterComboBox, ITestStudioControl methodsListBox)
        {
            this.wsdlModel = wsdlModel;
            this.methodFilterComboBox = methodFilterComboBox;
            this.methodsListBox = methodsListBox;

            this.methodFilterComboBox.Content = wsdlModel.Classes;

            this.methodFilterComboBox.AddEventHandler("SelectionChangeCommitted",
               new EventHandler(selectionChangeCommitted_methodFilterComboBox));
        }

        private void selectionChangeCommitted_methodFilterComboBox(object sender, EventArgs e)
        {
            var iControl = sender as ITestStudioControl;

            var selection = iControl.SelectedContentItem;

            if (selection is Type)
            {
                System.ComponentModel.BindingList<System.Reflection.MethodInfo> wsMethodList = null;
                if (wsdlModel.MethodsByType.TryGetValue(selection as Type, out wsMethodList))
                {
                    methodsListBox.Content = wsMethodList;
                    return;
                }

                var methods = wsdlModel.GetMethodsByType(selection as Type);
                methodsListBox.Content = methods.ToArray();
            }
            else
            {
                methodsListBox.Content = wsdlModel.Methods;
            }
        }
    }
}
