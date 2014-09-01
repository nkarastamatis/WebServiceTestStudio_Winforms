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
    class BasicParameterDisplayMethod : IParametersDisplayMethod
    {
        Dictionary<string, Tuple<ITestStudioControl, ITestStudioControl>> visibleControlsMethodName = 
            new Dictionary<string, Tuple<ITestStudioControl, ITestStudioControl>>();

        TestStudioFormBuilder formBuilder;

        public BasicParameterDisplayMethod()
        {
            this.formBuilder = TestStudioFormBuilder.Instance;
            this.formBuilder.ControlRemoved += formBuilder_ControlRemoved;
        }

        void formBuilder_ControlRemoved(object sender, EventArgs e)
        {
            var propertyGrid = sender as ITestStudioControl;
            if (propertyGrid != null)
            {
                Tuple<ITestStudioControl, ITestStudioControl> existingTuple;
                var key = propertyGrid.Label.Replace("Result ", String.Empty);
                if (visibleControlsMethodName.TryGetValue(key, out existingTuple))
                {
                    if (existingTuple.Item1.GetContainer() != null)
                        existingTuple.Item1.GetContainer().Close();
                    if (existingTuple.Item2 != null && 
                        existingTuple.Item2.GetContainer() != null)
                        existingTuple.Item2.GetContainer().Close();

                    visibleControlsMethodName.Remove(propertyGrid.Label);
                }
            }
        }

        #region IParametersDisplayMethod
        public void DisplayParameters(MethodInfo methodInfo)
        {
            if (!DisplayExistingParameters(methodInfo))
                BuildParameters(methodInfo);
        }

        public ITestStudioControl GetReturnPropertyGrid(string methodName)
        {
            var resultControl = visibleControlsMethodName[methodName].Item2;
            if (resultControl.GetContainer() != null)
                resultControl.GetContainer().Focus();

            var lastResult = (resultControl as IEnumerable<ITestStudioControl>).LastOrDefault();
            var suffix = 1;
            if (lastResult != null)
                suffix = Convert.ToInt32(lastResult.Label.Replace("Result ", String.Empty)) + 1;

            formBuilder.AddControl(
                TestStudioControlType.PropertyGrid,
                "Result " + suffix,
                DockStyle.Fill,
                resultControl.Label);

            var returnPropertyGrid = formBuilder.GetLastControlAdded();
            var contextMenu = ParamPropGridContextMenu.Add(returnPropertyGrid);
            contextMenu.SendParameter += OnSendParameter;

            return returnPropertyGrid;
        }
        #endregion


        private bool DisplayExistingParameters(MethodInfo methodInfo)
        {
            Tuple<ITestStudioControl, ITestStudioControl> existingTuple;
            if (visibleControlsMethodName.TryGetValue(methodInfo.Name, out existingTuple))
            {
                existingTuple.Item1.GetContainer().Focus();
                if (existingTuple.Item2 != null)
                    existingTuple.Item2.GetContainer().Focus();
                return true;
            }

            return false;
        }

        private void BuildParameters(MethodInfo methodInfo)
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
                TestStudioControlType.PropertyGrid,
                methodInfo.Name,
                DockStyle.Fill,
                TestStudioTab.Invoke);

            var propertyGrid = formBuilder.GetLastControlAdded();
            var contextMenu = ParamPropGridContextMenu.Add(propertyGrid);
            contextMenu.SendParameter += OnSendParameter;
            propertyGrid.Content = expando;

            // Return Parameter
            ITestStudioControl returnControl = null;
            var returnValue = methodInfo.ReturnParameter;
            if (returnValue.Name != null)
            {
                formBuilder.BeginCompositeControl("Result " + methodInfo.Name, true);
                formBuilder.AddCompositeControlToForm(DockStyle.Right, TestStudioTab.Invoke);
                returnControl = formBuilder.GetControl("Result " + methodInfo.Name);
            }

            // Add to Dictionary
            var tuple = new Tuple<ITestStudioControl, ITestStudioControl>(propertyGrid, returnControl);
            visibleControlsMethodName.Add(methodInfo.Name, tuple);
        }

        private void OnSendParameter(object copyObject, MethodInfo sendToMethodInfo)
        {
          
            Tuple<ITestStudioControl, ITestStudioControl> existingMethod;
            if (!visibleControlsMethodName.TryGetValue(sendToMethodInfo.Name, out existingMethod))
            {
                BuildParameters(sendToMethodInfo);
                existingMethod = visibleControlsMethodName[sendToMethodInfo.Name];
            }


            var existingObj = existingMethod.Item1.Content as IDictionary<String, object>;
            foreach (var obj in existingObj)
            {
                if (obj.Value.GetType() == copyObject.GetType())
                {
                    //existingObj[obj.Key] = copyObject.Clone();
                    var value = existingObj[obj.Key];
                    copyObject.Copy(ref value);
                    break;
                }
            }                       
        }



    }
}
