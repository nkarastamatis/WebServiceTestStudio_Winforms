using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceTestStudio.UserInterface;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Directors;
using WebServiceTestStudio.Model;
using WebServiceTestStudio.UserInterface;
using WebServiceTestStudio.UserInterface.Enums;
using WebServiceStudio;
using System.Windows.Forms;

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
            WsdlControl wsdlControl = builder.GetControl(TestStudioControlKey.WsdlControl) as WsdlControl;
            builder.AddControl(TestStudioControlType.RequestControl, "Request", DockStyle.Fill, TestStudioTab.Invoke);
            var requestControl = builder.GetLastControlAdded() as RequestControl;

            builder.CreateTab(TestStudioTab.XML);
            builder.AddControl(TestStudioControlType.TextBox, TestStudioControlKey.RequestTextBox, DockStyle.Fill, TestStudioTab.XML);
            builder.AddControl(TestStudioControlType.TextBox, TestStudioControlKey.ResponseTextBox, DockStyle.Bottom, TestStudioTab.XML);

            builder.CreateTab(TestStudioTab.Proxy);
            builder.AddControl(TestStudioControlType.PropertyGrid, TestStudioControlKey.ProxyPropertyGrid, DockStyle.Left, TestStudioTab.Proxy);
            var proxyPropertyGrid = builder.GetLastControlAdded();
            builder.AddControl(TestStudioControlType.DataGrid, TestStudioControlKey.ProxyDataGrid, DockStyle.Fill, TestStudioTab.Proxy);
            var proxyDataGrid = builder.GetLastControlAdded();

            
            var wsdlPathComboBox = wsdlControl.WsdlPathComboBox;
            var browseButton = wsdlControl.BrowseButton;
            var loadButton = wsdlControl.LoadButton;
            var methodsListBox = wsdlControl.MethodsListBox;
            var requestTextBox = builder.GetControl(TestStudioControlKey.RequestTextBox) as TestStudioTextBox;
            var responseTextBox = builder.GetControl(TestStudioControlKey.ResponseTextBox) as TestStudioTextBox;

            // Initialize Bindings and Directors           
            methodsListBox.Content = wsdlModel.Methods;

            var methodListDirector = new MethodListDirector(wsdlModel, wsdlControl.MethodFilterComboBox, wsdlControl.MethodsListBox);

            var methodParameterDirector = new MethodParameterDirector(new AdvancedParameterDisplayMethod()); 
            methodParameterDirector.GetMethodsByType = wsdlModel.GetMethodsByType;
            methodsListBox.DoubleClick += new EventHandler(methodParameterDirector.methodsListBox_DoubleClick);
            ParamPropGridContextMenu.GetMethodsByType = wsdlModel.GetMethodsByType;
            //controls.MethodsByClassListBox.DoubleClick += new EventHandler(methodParameterDirector.methodsListBox_DoubleClick);

            var loadWsdlDirector = new LoadWsdlDirector(wsdlPathComboBox, wsdlModel);
            browseButton.Click += new EventHandler(loadWsdlDirector.browse_Wsdl);
            loadButton.Click += new EventHandler(loadWsdlDirector.load_Wsdl);

            var proxyInfoDirector = new ProxyInfoDirector(proxyPropertyGrid, proxyDataGrid);
            loadWsdlDirector.NewWebServiceAdded += proxyInfoDirector.OnNewWebServiceAdded;

            var xmlTabDirector = new XmlTabDirector(requestTextBox, responseTextBox);

            var invokeDirector = new InvokeDirector(builder.GetTab(TestStudioTab.Invoke), wsdlModel.Methods);
            invokeDirector.InvokeComplete += methodParameterDirector.OnInvokeComplete;
            invokeDirector.InvokeComplete += xmlTabDirector.OnInvokeComplete;

            var form = builder.GetForm();
            form.KeyPreview = true;
            form.KeyUp += invokeDirector.form_KeyUp;
            
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

            TestStudioControls controls = new TestStudioControls();
            controls.WsdlPathComboBox = builder.GetControl(TestStudioControlKey.WsdlPathComboBox) as TestStudioComboBox;
            controls.BrowseButton = builder.GetControl(TestStudioControlKey.BrowseButton) as TestStudioButton;
            controls.LoadButton = builder.GetControl(TestStudioControlKey.LoadButton) as TestStudioButton;
            controls.ClassesListBox = builder.GetControl(TestStudioControlKey.ClassesListBox) as TestStudioListBox;
            controls.MethodsByClassListBox = builder.GetControl(TestStudioControlKey.MethodsByClassListBox) as TestStudioListBox;
            controls.MethodsListBox = builder.GetControl(TestStudioControlKey.MethodsListBox) as TestStudioListBox;
            controls.RequestTextBox = builder.GetControl(TestStudioControlKey.RequestTextBox) as TestStudioTextBox;
            controls.ResponseTextBox = builder.GetControl(TestStudioControlKey.ResponseTextBox) as TestStudioTextBox;

            // Initialize Bindings and Directors
            var classListDirector = new ClassListDirector(controls.ClassesListBox, controls.MethodsByClassListBox);
            controls.ClassesListBox.Content = wsdlModel.Classes;
            controls.MethodsListBox.Content = wsdlModel.Methods;
            classListDirector.GetMethodsByType = wsdlModel.GetMethodsByType;

            var methodParameterDirector = new MethodParameterDirector(new BasicParameterDisplayMethod());
            methodParameterDirector.GetMethodsByType = wsdlModel.GetMethodsByType;
            ParamPropGridContextMenu.GetMethodsByType = wsdlModel.GetMethodsByType;
            controls.MethodsListBox.DoubleClick += new EventHandler(methodParameterDirector.methodsListBox_DoubleClick);
            controls.MethodsByClassListBox.DoubleClick += new EventHandler(methodParameterDirector.methodsListBox_DoubleClick);

            var loadWsdlDirector = new LoadWsdlDirector(controls.WsdlPathComboBox, wsdlModel);
            controls.BrowseButton.Click += new EventHandler(loadWsdlDirector.browse_Wsdl);
            controls.LoadButton.Click += new EventHandler(loadWsdlDirector.load_Wsdl);

            var xmlTabDirector = new XmlTabDirector(controls.RequestTextBox, controls.ResponseTextBox);

            var invokeDirector = new InvokeDirector(builder.GetTab(TestStudioTab.Invoke), wsdlModel.Methods);
            invokeDirector.InvokeComplete += methodParameterDirector.OnInvokeComplete;
            invokeDirector.InvokeComplete += xmlTabDirector.OnInvokeComplete;

            var form = builder.GetForm();
            form.KeyPreview = true;
            form.KeyUp += invokeDirector.form_KeyUp;
        }
    }
}
