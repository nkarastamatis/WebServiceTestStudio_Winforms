using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebServiceTestStudio.Core;
using System.Reflection;
using WebServiceTestStudio.UserInterface.Enums;

namespace WebServiceTestStudio.UserInterface
{
    public class ParamPropGridContextMenu
    {
        public delegate void SendParameterEventHandler(object copyObject, MethodInfo sendToMethodInfo);
        public event SendParameterEventHandler SendParameter;

        public static GetMethods GetMethodsByType;

        public static ParamPropGridContextMenu Add(ITestStudioControl control)
        {
            if (!(control is PropertyGrid))
                return null;

            var contextMenu = new ParamPropGridContextMenu();
            control.AddEventHandler("SelectedObjectsChanged", new EventHandler(contextMenu.propertyGrid_SelectedObjectsChanged));
            return contextMenu;
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
            var formBuilder = TestStudioFormBuilder.Instance;
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

                DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[0].Clone(); ;
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
                var propertyGrid = gridControl.Parent.Parent as ITestStudioControl;
                var selectedItem = propertyGrid.SelectedContentItem;

                SendParameter(selectedItem, menuItem.Tag as MethodInfo);
            }
        }
    }
}
