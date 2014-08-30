using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.UserInterface.Enums;

namespace WebServiceTestStudio.UserInterface
{
    class TestStudioControlFactory
    {
        public ITestStudioControl GetControl(TestStudioControlType type)
        {
            Type controlType = null;
            switch(type)
            {
                case TestStudioControlType.Button:
                    controlType = typeof(TestStudioButton);
                    break;
                case TestStudioControlType.ComboBox:
                    controlType = typeof(TestStudioComboBox);
                    break;
                case TestStudioControlType.DataGrid:
                    controlType = typeof(TestStudioDataGrid);
                    break;
                case TestStudioControlType.DockPanel:
                    controlType = typeof(TestStudioDockPanel);
                    break;
                case TestStudioControlType.EntryField:
                    controlType = typeof(TestStudioTextBox);
                    break;
                case TestStudioControlType.ListBox:
                    controlType = typeof(TestStudioListBox);
                    break;
                case TestStudioControlType.PropertyGrid:
                    controlType = typeof(TestStudioPropertyGrid);
                    break;
                case TestStudioControlType.TextBox:
                    controlType = typeof(TestStudioTextBox);
                    break;
                case TestStudioControlType.WsdlControl:
                    controlType = typeof(WsdlControl);
                    break;
                case TestStudioControlType.RequestControl:
                    controlType = typeof(RequestControl);
                    break;
                default:
                    throw new NotImplementedException();
            }

            var control = CreateControl(controlType);
            if (type == TestStudioControlType.TextBox)
            {
                ((TestStudioTextBox)control).Multiline = true;
                ((TestStudioTextBox)control).ScrollBars = System.Windows.Forms.ScrollBars.Both;
            }
            if (type == TestStudioControlType.WsdlControl)
                ((WsdlControl)control).Dock = System.Windows.Forms.DockStyle.Fill;
            if (type == TestStudioControlType.RequestControl)
                ((RequestControl)control).Dock = System.Windows.Forms.DockStyle.Fill;

            return control;
        }

        public TestStudioCompositeControl GetCompositeControl(bool useDockPanel)
        {
            ITestStudioContainer container = null;
            if (useDockPanel)
            {
                container = new TestStudioDockPanel();
                ((TestStudioDockPanel)container).DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingSdi;
            }

            var compositeControl = new TestStudioCompositeControl(container);
            if (useDockPanel)
                compositeControl.Dock = System.Windows.Forms.DockStyle.Fill;

            return compositeControl;
        }

        private ITestStudioControl CreateControl(Type type)
        {
            bool valid = false;
            foreach (var i in type.GetInterfaces())
            {
                if (i == typeof(ITestStudioControl))
                    valid = true;
            }
            if (!valid)
                throw new Exception("Control must implement ITestStudioControl");

            return Activator.CreateInstance(type) as ITestStudioControl;
        }
        
    }
}
