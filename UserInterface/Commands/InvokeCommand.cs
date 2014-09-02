using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Directors;
using WebServiceTestStudio.UserInterface.Enums;
using System.Reflection;

namespace WebServiceTestStudio.UserInterface.Commands
{
    public class InvokeCommand : ICommand
    {
        private readonly InvokeDirector invokeDirector;
        private readonly bool canExecute;

        private ITestStudioControl activeControl;
        private string methodName;

        public InvokeCommand(InvokeDirector invokeDirector)
        {
            this.invokeDirector = invokeDirector;
            try
            {
                // Find the active document and the associated method name.
                var invokeTab = TestStudioFormBuilder.Instance.GetTab(TestStudioTab.Invoke);
                var activeContent = invokeTab.SelectedChild;
                activeControl = activeContent.Content as ITestStudioControl;

                if (activeControl is RequestControl)
                    activeControl = ((RequestControl)activeControl).requestDataPropertyGrid as ITestStudioControl;

                var strings = activeControl.Label.Split(' ');
                if (strings != null && strings.Any())
                    methodName = strings[0];
                else
                    methodName = activeControl.Label;

                canExecute = true;
            }
            catch
            {
                canExecute = false;
            }
        }

        #region ICommand Members
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {            
            return canExecute;
        }

        public void Execute(object parameter)
        {
            invokeDirector.InvokeWebMethod(activeControl, methodName);
        }

        #endregion
    }
}
