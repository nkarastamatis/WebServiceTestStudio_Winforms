using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;
using WebServiceStudio;
using WebServiceTestStudio.Core;
using WebServiceTestStudio.Model;

namespace WebServiceTestStudio.Directors
{
    public class InvokeDirector
    {
        private IList<MethodInfo> methods;

        public InvokeCompleteEventHandler InvokeComplete;        

        public InvokeDirector(IList<MethodInfo> methods)
        {
            this.methods = methods;
        }

        public void InvokeWebMethod(ITestStudioControl activeControl, string methodName)
        {           
            if (!methods.Any())
                throw new Exception("Not methods are loaded");

            MethodInfo methodInfo = methods.Single(i => i.Name == methodName);

            var dictionary = activeControl.Content as IDictionary<String, object>;

            InvokeWebMethod(methodInfo, dictionary.Values.ToArray());           
        }

        private object InvokeWebMethod(MethodInfo method, object[] parameters)
        {
            HttpWebClientProtocol proxy = WsdlModel.Proxy;
            object result = null;
            RequestProperties properties = new RequestProperties(proxy);
            try
            {                
                Type declaringType = method.DeclaringType;
                WSSWebRequest.RequestTrace = properties;
                result = method.Invoke(proxy, BindingFlags.Public, null, parameters, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                WSSWebRequest.RequestTrace = null;
                InvokeComplete(this, new InvokeCompleteEventArgs(method, properties, result));
            }

            return result;
        }
    }
}
