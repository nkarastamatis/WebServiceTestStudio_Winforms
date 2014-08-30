using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Services.Protocols;
using WebServiceTestStudio.Core;
using System.ComponentModel;
using WebServiceStudio;


namespace WebServiceTestStudio.Model
{
    public class WsdlModel
    {
        private string wsdlPath;
        public BindingList<object> Classes;
        public BindingList<MethodInfo> Methods;

        public Wsdl Wsdl;
        public static HttpWebClientProtocol Proxy;
        
        public WsdlModel()
        {
            Classes = new BindingList<object>();
            Methods = new BindingList<MethodInfo>();
            WsdlPath = "";
        }

        public string WsdlPath
        {
            get { return wsdlPath; }
            set
            {
                wsdlPath = value;
            }
        }

        public List<MethodInfo> GetMethodsByType(Type type)
        {
            var methods =
                   Methods.Where(
                        method => method.ReturnType.FullName == type.FullName || method.ReturnType.FullName == type.FullName + "[]" ||
                                    method.GetParameters().Where(p => p.ParameterType == type).Count() > 0);

            return methods.ToList();
        }
    }
}
