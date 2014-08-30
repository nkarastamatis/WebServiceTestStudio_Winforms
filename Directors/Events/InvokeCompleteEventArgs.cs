using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WebServiceTestStudio.Core;
using WebServiceStudio;

namespace WebServiceTestStudio.Directors
{
    public class InvokeCompleteEventArgs
    {
        public InvokeCompleteEventArgs(MethodInfo method, RequestProperties properties, object result)
        {
            Method = method;
            RequestProperties = properties;
            Result = result;
        }

        public MethodInfo Method { get; private set; }
        public RequestProperties RequestProperties { get; private set; }
        public object Result { get; private set; }
    }
}
