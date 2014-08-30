using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WebServiceTestStudio.Core;

namespace WebServiceTestStudio.UserInterface
{
    public interface IParametersDisplayMethod
    {
        void DisplayParameters(MethodInfo methodInfo);
        ITestStudioControl GetReturnPropertyGrid(string methodName);
    }
}
