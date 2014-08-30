using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Design;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using WebServiceTestStudio.UserInterface;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public partial class MainForm : Form 
    {
        public StringCollection Classes { get; set; }
        public List<MethodInfo> Methods { get; set; }
        public ClassListDockContent ClassList { get; set; }
        public PropertiesDockContent PropertyGrid { get; set; }
        public OutputDockContent OutputView { get; set; }
        public ObjectsDockContent ObjectsView { get; set; }
        public MethodListDockContent MethodList { get; set; }
        public List<PropertiesDockContent> Parameters {get;set;}

        public MainForm()
        {
            InitializeComponent();


            //PropertyGrid = new PropertiesDockContent();
            //PropertyGrid.Show(dockPanel1, DockState.DockRight);

            OutputView = new OutputDockContent();
            OutputView.Show(mainDockPanel, DockState.DockBottom);

            ObjectsView = new ObjectsDockContent();
            ObjectsView.Show(mainDockPanel, DockState.Document);

            Classes = new StringCollection();
            Methods = new List<MethodInfo>();

        }
        
        public void DisplayParameters(object info)
        {
            MethodInfo methodInfo = (MethodInfo)info;
            var parameters = methodInfo.GetParameters();
            var returnValue = methodInfo.ReturnParameter;

            if (Parameters == null)
                Parameters = new List<PropertiesDockContent>();
            else
            {
                foreach (var existingParam in Parameters)
                    existingParam.Dispose();

                Parameters.Clear();
            }

            dynamic expando = new System.Dynamic.ExpandoObject();
            foreach (var param in parameters)
            {
                var p = expando as IDictionary<String, object>;
                p[param.Name] = CreateObject(param.ParameterType);
            }
            var paramDockContent = new PropertiesDockContent();
            paramDockContent.Text = methodInfo.Name;
            paramDockContent.SetItem(expando as System.Dynamic.ExpandoObject);
            paramDockContent.Show(mainDockPanel, DockState.DockRight);

        }

        private object CreateObject(Type type)
        {
            object obj = null;
            if (type == typeof(String))
                obj = Activator.CreateInstance(type, '\0', 0);
            else if (type.BaseType == typeof(Array))
                obj = Activator.CreateInstance(type, 1);
            else
                obj = Activator.CreateInstance(type);

            return obj;
        }

        public MenuItem[] GetMenuItems(Type type)
        {
            var items = new List<MenuItem>();

            var methods =
                   Methods.Where(
                        method => method.ReturnType.FullName == type.FullName || method.ReturnType.FullName == type.FullName + "[]" ||
                                    method.GetParameters().Where(p => p.ParameterType == type).Count() > 0);

            foreach (var method in methods)
                items.Add(new MenuItem(method.Name));

            return items.ToArray();
        }
    }
}
