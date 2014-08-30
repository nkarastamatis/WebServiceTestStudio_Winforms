using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Threading.Tasks;
using System.Reflection;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.UserInterface.Enums;
using System.Windows.Forms;

namespace WebServiceTestStudio.UserInterface
{
    class AdvancedParameterDisplayMethod : IParametersDisplayMethod
    {
        private static int documentCount = 0;
        private TestStudioFormBuilder formBuilder;


        public AdvancedParameterDisplayMethod()
        {
            formBuilder = TestStudioFormBuilder.Instance;
        }

        #region IParametersDisplayMethod Members

        public void DisplayParameters(MethodInfo methodInfo)
        {
            // Input Parameters
            var parameters = methodInfo.GetParameters();
            dynamic expando = new ExpandoObject();
            var dictionary = expando as IDictionary<String, object>;
            foreach (var param in parameters)
            {
                dictionary[param.Name] = param.ParameterType.CreateObject();
            }

            formBuilder.AddControl(
                TestStudioControlType.RequestControl,
                String.Format("{0} Request [{1}]",methodInfo.Name, ++documentCount),
                DockStyle.Fill,
                TestStudioTab.Invoke);

            var requestControl = formBuilder.GetLastControlAdded() as RequestControl;
            requestControl.Label = methodInfo.Name;
            var contextMenu = ParamPropGridContextMenu.Add(requestControl.requestDataPropertyGrid);
            contextMenu.SendParameter += OnSendParameter;
            requestControl.requestDataPropertyGrid.Content = expando;
        }

        public ITestStudioControl GetReturnPropertyGrid(string methodName)
        {
            var activeContent = formBuilder.GetTab(TestStudioTab.Invoke).SelectedChild;
            var requestControl = activeContent.Content as RequestControl;
            var resultControl = requestControl.resultCompositeControl;

            var lastResult = (resultControl as IEnumerable<ITestStudioControl>).LastOrDefault();
            var suffix = 1;
            if (lastResult != null)
                suffix = Convert.ToInt32(lastResult.Label.Replace("Result ", String.Empty)) + 1;

            formBuilder.AddControl(
                TestStudioControlType.PropertyGrid,
                "Result " + suffix,
                DockStyle.Fill,
                resultControl);

            var returnPropertyGrid = formBuilder.GetLastControlAdded();
            var contextMenu = ParamPropGridContextMenu.Add(returnPropertyGrid);
            contextMenu.SendParameter += OnSendParameter;

            return returnPropertyGrid;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        }

        private void OnSendParameter(object copyObject, MethodInfo sendToMethodInfo)
        {
        }

        #endregion
    }
}