using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;
using WebServiceTestStudio.UserInterface;
using WebServiceTestStudio.UserInterface.Enums;

namespace WebServiceTestStudio.Directors
{
    public class MethodParameterDirector
    {
        protected TestStudioFormBuilder formBuilder;
        protected Dictionary<string, Tuple<TestStudioPropertyGrid, TestStudioPropertyGrid>> visibleControlsMethodName =
            new Dictionary<string, Tuple<TestStudioPropertyGrid, TestStudioPropertyGrid>>();
        protected Dictionary<string, MethodInfo> visibleMethodsByName =
            new Dictionary<string, MethodInfo>();

        public delegate List<MethodInfo> GetMethods(Type type);
        public GetMethods GetMethodsByType;

        public MethodParameterDirector(TestStudioFormBuilder builder)
        {
            this.formBuilder = builder;
            this.formBuilder.ControlRemoved += formBuilder_ControlRemoved;
        }

        void formBuilder_ControlRemoved(object sender, EventArgs e)
        {
            var propertyGrid = sender as TestStudioPropertyGrid;
            if (propertyGrid != null)
            {
                Tuple<TestStudioPropertyGrid, TestStudioPropertyGrid> existingTuple;
                var key = propertyGrid.Text.Replace("Result ", String.Empty);
                if (visibleControlsMethodName.TryGetValue(key, out existingTuple))
                {
                    if (existingTuple.Item1.GetContainer() != null)
                        existingTuple.Item1.GetContainer().Close();
                    if (existingTuple.Item2.GetContainer() != null)
                        existingTuple.Item2.GetContainer().Close();
                    visibleControlsMethodName.Remove(propertyGrid.Text);
                    visibleMethodsByName.Remove(propertyGrid.Text);
                }
            }
        }

        public void methodsListBox_DoubleClick(object sender, EventArgs e)
        {
            var listBox = sender as ITestStudioControl;
            if (listBox != null)
            {
                var methodInfo = listBox.SelectedContentItem as MethodInfo;
                if (methodInfo == null)
                    return;

                DisplayParameters(methodInfo);
            }
        }

        protected void DisplayParameters(MethodInfo methodInfo)
        {
            if (!DisplayExistingParameters(methodInfo))
                BuildParameters(methodInfo);
        }

        protected bool DisplayExistingParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var returnValue = methodInfo.ReturnParameter;

            Tuple<TestStudioPropertyGrid, TestStudioPropertyGrid> existingTuple;
            if (visibleControlsMethodName.TryGetValue(methodInfo.Name, out existingTuple))
            {
                (existingTuple.Item1.Parent as TestStudioContent).Activate();
                if (existingTuple.Item2 != null)
                    (existingTuple.Item2.Parent as TestStudioContent).Activate();
                return true;
            }

            return false;
        }

        protected void BuildParameters(MethodInfo methodInfo)
        {
            // Input Parameters
            var parameters = methodInfo.GetParameters();
            dynamic expando = new ExpandoObject();
            var dictionary = expando as IDictionary<String, object>;
            foreach (var param in parameters)
            {
                dictionary[param.Name] = CreateObject(param.ParameterType);
            }

            formBuilder.AddControl(
                TestStudioControlType.PropertyGrid,
                methodInfo.Name,
                DockStyle.Fill,
                TestStudioTab.Invoke);

            var propertyGrid = formBuilder.GetLastControlAdded() as TestStudioPropertyGrid;
            propertyGrid.AddEventHandler("SelectedObjectsChanged", new EventHandler(propertyGrid_SelectedObjectsChanged));
            propertyGrid.Content = expando;

            // Return Parameter
            TestStudioPropertyGrid propertyGridReturn = null;
            var returnValue = methodInfo.ReturnParameter;
            if (returnValue.Name != null)
            {
                dynamic expandoReturn = new ExpandoObject();
                dictionary = expandoReturn as IDictionary<String, object>;
                dictionary["result"] = CreateObject(returnValue.ParameterType);

                formBuilder.AddControl(
                    TestStudioControlType.PropertyGrid,
                    "Result " + methodInfo.Name,
                    DockStyle.Right,
                    TestStudioTab.Invoke);

                propertyGridReturn = formBuilder.GetLastControlAdded() as TestStudioPropertyGrid;
                propertyGridReturn.AddEventHandler("SelectedObjectsChanged", new EventHandler(propertyGrid_SelectedObjectsChanged));
                propertyGridReturn.Content = expandoReturn;
            }

            // Add to Dictionary
            var tuple = new Tuple<TestStudioPropertyGrid, TestStudioPropertyGrid>(propertyGrid, propertyGridReturn);
            visibleControlsMethodName.Add(methodInfo.Name, tuple);
            visibleMethodsByName.Add(methodInfo.Name, methodInfo);
        }

        protected object CreateObject(Type type)
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

        private void propertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        {
            var propertyGrid = sender as TestStudioPropertyGrid;
            foreach (System.Windows.Forms.Control control in propertyGrid.Controls[2].Controls)
            {
                control.MouseDown += new System.Windows.Forms.MouseEventHandler(propertyGrid_MouseClick);
            }
        }

        private void propertyGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {                
                var control = sender as Control;
                var propertyGrid = control.Parent.Parent as ITestStudioControl;
                var selectedItem = propertyGrid.SelectedContentItem;

                if (selectedItem != null)
                {
                    MenuItem[] methodMenuItems = GetMethodMenuItems(selectedItem.GetType());

                    List<MenuItem> menuItems = new List<MenuItem>();                   

                    var sendToObjetMenuItem = new MenuItem("Send to...", methodMenuItems);
                    menuItems.Add(sendToObjetMenuItem);

                    if (selectedItem is Array)
                    {
                        var displayInDataGrid = new MenuItem("Data Grid");
                        displayInDataGrid.Click += new EventHandler(displayInDataGridMenuItem_MouseClick);
                        displayInDataGrid.Tag = selectedItem;
                        menuItems.Add(displayInDataGrid);
                    }

                    var contextMenu = new ContextMenu(menuItems.ToArray());
                    contextMenu.Show(sender as Control, e.Location);
                }
            }
        }

        private void displayInDataGridMenuItem_MouseClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            Type elementType = menuItem.Tag.GetType().GetElementType();
            var suffix = 1;
            while (formBuilder.GetControl(elementType.Name + (suffix > 1 ? String.Format("[{0}]", suffix) : String.Empty)) != null) { suffix++; };

            formBuilder.AddControl(
                TestStudioControlType.DataGrid, 
                elementType.Name + (suffix > 1 ? String.Format("[{0}]", suffix) : String.Empty), 
                DockStyle.Bottom);
            var control = formBuilder.GetLastControlAdded();
            var dataGrid = (TestStudioDataGrid)control;
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);

            foreach (var field in elementType.GetFields())
            {
                dataGrid.Columns.Add(field.Name, field.Name);
            }

            foreach (var element in (menuItem.Tag as Array))
            {
                    
                DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[0].Clone();;
                if (element == null)
                {
                    dataGrid.Rows.Add(row);
                    continue;
                }

                int ix = 0;
                foreach (var field in elementType.GetFields())
                    row.Cells[ix++].Value = field.GetValue(element);

                dataGrid.Rows.Add(row);
            }

            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
        }

        private MenuItem[] GetMethodMenuItems(Type type)
        {
            var items = new List<MenuItem>();

            var methods = GetMethodsByType(type);

            foreach (var method in methods)
            {
                var menuItem = new MenuItem(method.Name);
                menuItem.Click += new EventHandler(methodMenuItem_MouseClick);
                menuItem.Tag = method;
                items.Add(menuItem);
            }

            return items.ToArray();
        }

        private void methodMenuItem_MouseClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem.Tag != null)
            {
                var subMenu = menuItem.Parent as MenuItem;
                var contextMenu = subMenu.Parent as ContextMenu;
                var gridControl = contextMenu.SourceControl as Control;
                var propertyGrid = gridControl.Parent.Parent as TestStudioPropertyGrid;
                var selectedItem = propertyGrid.SelectedContentItem;

                Tuple<TestStudioPropertyGrid, TestStudioPropertyGrid> existingMethod;
                if (!visibleControlsMethodName.TryGetValue(menuItem.Text, out existingMethod))
                {
                    BuildParameters(menuItem.Tag as MethodInfo);
                    existingMethod = visibleControlsMethodName[menuItem.Text];
                }


                var existingObj = existingMethod.Item1.Content as IDictionary<String, object>;
                foreach (var obj in existingObj)
                {
                    if (obj.Value.GetType() == selectedItem.GetType())
                    {
                        existingObj[obj.Key] = selectedItem;
                        break;
                    }
                }

                if (existingMethod.Item2 != null)
                {
                    existingObj = existingMethod.Item2.Content as IDictionary<String, object>;
                    foreach (var obj in existingObj)
                    {
                        if (obj.Value.GetType() == selectedItem.GetType())
                        {
                            existingObj[obj.Key] = selectedItem;
                            break;
                        }
                    }
                }
                
            }
        }

        public void OnInvokeComplete(object sender, InvokeCompleteEventArgs e)
        {
            var result = e.Result;
            var methodName = e.Method.Name;
            if (result != null)
            {
                var resultPropertyGrid = visibleControlsMethodName[methodName].Item2;
                if (resultPropertyGrid != null)
                {
                    var resultDictionary = resultPropertyGrid.Content as IDictionary<String, object>;
                    resultDictionary["result"] = result;
                    resultPropertyGrid.Content = resultDictionary;
                    resultPropertyGrid.GetContainer().Focus();
                }
            }
        }
    }
}
