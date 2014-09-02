using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.UserInterface;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Directors;
using WebServiceTestStudio.Model;
using WebServiceTestStudio.UserInterface.Enums;
using WebServiceStudio;
using System.Windows.Forms;
using WebServiceTestStudio.UserInterface.Interactors;
using WebServiceTestStudio.UserInterface.Commands;

namespace WebServiceTestStudio.UserInterface
{
    public class TestStudioFormDesigner
    {
        private TestStudioFormBuilder builder;
        private bool advancedMode;
        private WsdlModel wsdlModel = new WsdlModel();

        public TestStudioFormDesigner(TestStudioFormBuilder builder, bool advancedMode)
        {
            this.builder = builder;
            this.advancedMode = advancedMode;

            // Create the Form in the constructor so there can only be one form.
            builder.CreateForm();
            BuildForm();
            builder.GetForm().KeyUp += form_KeyUp;
        }

        private void SwitchMode()
        {
            var form = builder.GetForm();
            form.SuspendLayout();
            form.Reset();
            builder.Reset();

            advancedMode = !advancedMode;
            BuildForm();
            form.ResumeLayout(false);
        }

        private void BuildForm()
        {
            if (advancedMode)
                BuildAdvancedForm();
            else
                BuildSimpleForm();
        }

        private void BuildAdvancedForm()
        {
            
            builder.CreateTab(TestStudioTab.Invoke);

            builder.AddControl(TestStudioControlType.WsdlControl, TestStudioControlKey.WsdlControl, DockStyle.Left, TestStudioTab.Invoke);
            builder.AddControl(TestStudioControlType.RequestControl, "Request", DockStyle.Fill, TestStudioTab.Invoke);

            builder.CreateTab(TestStudioTab.XML);
            builder.AddControl(TestStudioControlType.TextBox, TestStudioControlKey.RequestTextBox, DockStyle.Fill, TestStudioTab.XML);
            builder.AddControl(TestStudioControlType.TextBox, TestStudioControlKey.ResponseTextBox, DockStyle.Bottom, TestStudioTab.XML);

            builder.CreateTab(TestStudioTab.Proxy);
            builder.AddControl(TestStudioControlType.PropertyGrid, TestStudioControlKey.ProxyPropertyGrid, DockStyle.Left, TestStudioTab.Proxy);
            builder.AddControl(TestStudioControlType.DataGrid, TestStudioControlKey.ProxyDataGrid, DockStyle.Fill, TestStudioTab.Proxy);
            
            var requestControl = builder.GetControl("Request") as RequestControl;
            WsdlControl wsdlControl = builder.GetControl(TestStudioControlKey.WsdlControl) as WsdlControl;
            var wsdlPathComboBox = wsdlControl.WsdlPathComboBox;
            var browseButton = wsdlControl.BrowseButton;
            var loadButton = wsdlControl.LoadButton;
            var methodsListBox = wsdlControl.MethodsListBox;
            var requestTextBox = builder.GetControl(TestStudioControlKey.RequestTextBox);
            var responseTextBox = builder.GetControl(TestStudioControlKey.ResponseTextBox);
            var proxyPropertyGrid = builder.GetControl(TestStudioControlKey.ProxyPropertyGrid);
            var proxyDataGrid = builder.GetControl(TestStudioControlKey.ProxyDataGrid);

            // Initialize Bindings and Directors           
            methodsListBox.Content = wsdlModel.Methods;

            var methodListDirector = new MethodListDirector(wsdlModel, wsdlControl.MethodFilterComboBox, wsdlControl.MethodsListBox);

            var methodParameterDirector = new MethodParameterDirector(new AdvancedParameterDisplayMethod()); 
            methodParameterDirector.GetMethodsByType = wsdlModel.GetMethodsByType;
            methodsListBox.AddEventHandler("DoubleClick", new EventHandler(methodParameterDirector.methodsListBox_DoubleClick));
            ParamPropGridContextMenu.GetMethodsByType = wsdlModel.GetMethodsByType;

            var loadWsdlDirector = new LoadWsdlDirector(wsdlPathComboBox, wsdlModel, ApplicationConstants.FileHistoryLocation);
            var buttonInteractor = new ButtonInteractor();
            buttonInteractor.Add(browseButton as Button, new BrowseWsdlCommand(loadWsdlDirector));
            buttonInteractor.Add(loadButton as Button, new LoadWsdlCommand(loadWsdlDirector));

            var proxyInfoDirector = new ProxyInfoDirector(proxyPropertyGrid, proxyDataGrid);
            loadWsdlDirector.NewWebServiceAdded += proxyInfoDirector.OnNewWebServiceAdded;

            var xmlTabDirector = new XmlTabDirector(requestTextBox, responseTextBox);

            var invokeDirector = new InvokeDirector(wsdlModel.Methods);
            invokeDirector.InvokeComplete += methodParameterDirector.OnInvokeComplete;
            invokeDirector.InvokeComplete += xmlTabDirector.OnInvokeComplete;

            var form = builder.GetForm();
            var formInteractor = new FormInteractor(form, invokeDirector);            
        }

        private void form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                SwitchMode();
            }
        }

        private void BuildSimpleForm()
        {

            builder.BeginCompositeControl("Wsdl Path");
            builder.AddControl(TestStudioControlType.ComboBox, TestStudioControlKey.WsdlPathComboBox, DockStyle.Top);
            builder.AddControl(TestStudioControlType.Button, TestStudioControlKey.BrowseButton, DockStyle.Right);
            builder.AddControl(TestStudioControlType.Button, TestStudioControlKey.LoadButton, DockStyle.Right);
            builder.AddCompositeControlToForm(DockStyle.Top);

            builder.CreateTab(TestStudioTab.Invoke);

            builder.BeginCompositeControl("Classes", true);
            builder.AddControl(TestStudioControlType.ListBox, TestStudioControlKey.ClassesListBox, DockStyle.Fill);
            builder.AddControl(TestStudioControlType.ListBox, TestStudioControlKey.MethodsByClassListBox, DockStyle.Bottom);
            builder.AddCompositeControlToForm(DockStyle.Left, TestStudioTab.Invoke);

            builder.AddControl(TestStudioControlType.ListBox, TestStudioControlKey.MethodsListBox, DockStyle.Left, TestStudioTab.Invoke);

            builder.CreateTab(TestStudioTab.XML);
            builder.AddControl(TestStudioControlType.TextBox, TestStudioControlKey.RequestTextBox, DockStyle.Fill, TestStudioTab.XML);
            builder.AddControl(TestStudioControlType.TextBox, TestStudioControlKey.ResponseTextBox, DockStyle.Bottom, TestStudioTab.XML);

            var wsdlPathComboBox = builder.GetControl(TestStudioControlKey.WsdlPathComboBox);
            var browseButton = builder.GetControl(TestStudioControlKey.BrowseButton);
            var loadButton = builder.GetControl(TestStudioControlKey.LoadButton);
            var classesListBox = builder.GetControl(TestStudioControlKey.ClassesListBox);
            var methodsByClassListBox = builder.GetControl(TestStudioControlKey.MethodsByClassListBox);
            var methodsListBox = builder.GetControl(TestStudioControlKey.MethodsListBox);
            var requestTextBox = builder.GetControl(TestStudioControlKey.RequestTextBox);
            var responseTextBox = builder.GetControl(TestStudioControlKey.ResponseTextBox);

            // Initialize Bindings and Directors
            var classListDirector = new ClassListDirector(classesListBox, methodsByClassListBox);
            classesListBox.Content = wsdlModel.Classes;
            methodsListBox.Content = wsdlModel.Methods;
            classListDirector.GetMethodsByType = wsdlModel.GetMethodsByType;

            var methodParameterDirector = new MethodParameterDirector(new BasicParameterDisplayMethod());
            methodParameterDirector.GetMethodsByType = wsdlModel.GetMethodsByType;
            ParamPropGridContextMenu.GetMethodsByType = wsdlModel.GetMethodsByType;
            methodsListBox.AddEventHandler("DoubleClick", new EventHandler(methodParameterDirector.methodsListBox_DoubleClick));
            methodsByClassListBox.AddEventHandler("DoubleClick", new EventHandler(methodParameterDirector.methodsListBox_DoubleClick));

            var loadWsdlDirector = new LoadWsdlDirector(wsdlPathComboBox, wsdlModel, ApplicationConstants.FileHistoryLocation);
            var buttonInteractor = new ButtonInteractor();
            buttonInteractor.Add(browseButton as Button, new BrowseWsdlCommand(loadWsdlDirector));
            buttonInteractor.Add(loadButton as Button, new LoadWsdlCommand(loadWsdlDirector));

            var xmlTabDirector = new XmlTabDirector(requestTextBox, responseTextBox);

            var invokeDirector = new InvokeDirector(wsdlModel.Methods);
            invokeDirector.InvokeComplete += methodParameterDirector.OnInvokeComplete;
            invokeDirector.InvokeComplete += xmlTabDirector.OnInvokeComplete;

            var form = builder.GetForm();
            var formInteractor = new FormInteractor(form, invokeDirector);  
        }
    }
}
