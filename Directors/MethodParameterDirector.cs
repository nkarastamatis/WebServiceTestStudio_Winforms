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
        protected Dictionary<string, Tuple<ITestStudioControl, ITestStudioControl>> visibleControlsMethodName =
            new Dictionary<string, Tuple<ITestStudioControl, ITestStudioControl>>();

        public GetMethods GetMethodsByType;

        private IParametersDisplayMethod displayMethod;

        public MethodParameterDirector(IParametersDisplayMethod displayMethod)
        {
            this.formBuilder = TestStudioFormBuilder.Instance;
            this.displayMethod = displayMethod;
        }

        public void methodsListBox_DoubleClick(object sender, EventArgs e)
        {
            var listBox = sender as ITestStudioControl;
            if (listBox != null)
            {
                var methodInfo = listBox.SelectedContentItem as MethodInfo;
                if (methodInfo == null)
                    return;

                displayMethod.DisplayParameters(methodInfo);
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

        public void OnInvokeComplete(object sender, InvokeCompleteEventArgs e)
        {
            
            var result = e.Result;
            var methodName = e.Method.Name;
            if (result != null)
            {
                dynamic expandoReturn = new ExpandoObject();
                var dictionary = expandoReturn as IDictionary<String, object>;
                dictionary["result"] = result;

                var returnPropertyGrid = displayMethod.GetReturnPropertyGrid(methodName);
                returnPropertyGrid.Content = expandoReturn;
                returnPropertyGrid.GetContainer().Focus();
            }
        }
    }
}
