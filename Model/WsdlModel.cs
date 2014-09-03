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
        public List<Type> Classes;
        public List<MethodInfo> Methods;
        public Dictionary<Type, List<MethodInfo>> MethodsByType;

        public Wsdl Wsdl;
        public static HttpWebClientProtocol Proxy;
        public static Assembly ProxyAssembly;

        public event EventHandler WsdlModelInitialized;
        
        public WsdlModel()
        {
            Classes = new List<Type>();
            Methods = new List<MethodInfo>();
            MethodsByType = new Dictionary<Type, List<MethodInfo>>();
            WsdlPath = "";
        }

        public void Initialize(string path)
        {
            var wsdl = new Wsdl();
            wsdl.Paths.Add(path);
            wsdl.Generate();
            Wsdl = wsdl;
            WsdlModel.ProxyAssembly = Wsdl.ProxyAssembly;

            GenerateClassesAndMethods();

            if (WsdlModelInitialized != null)
                WsdlModelInitialized(this, EventArgs.Empty);
        }

        public string WsdlPath
        {
            get { return wsdlPath; }
            set { wsdlPath = value;}
        }

        private void GenerateClassesAndMethods()
        {
            Classes.Clear();
            Methods.Clear();
            MethodsByType.Clear();

            var types = Wsdl.ProxyAssembly.GetTypes().OrderBy(a => a.Name);

            if (types.Count() == 0)
                throw new Exception("Invalid Wsdl.");

            // populate the methods first
            var wsTypes = types.Where(type => typeof(HttpWebClientProtocol).IsAssignableFrom(type));
            List<MethodInfo> allMethods = new List<MethodInfo>();
            foreach (var wsType in wsTypes)
            {
                var methods = wsType.GetMethods().Where(method => method.GetCustomAttributes(typeof(SoapDocumentMethodAttribute), true).Length > 0);
                var methodsList = methods.ToList();
                methodsList.Sort((m1, m2) => m1.Name.CompareTo(m2.Name));

                MethodsByType.Add(wsType, methodsList);

                Methods.AddRange(methodsList);
            }

            // now, only add the classes that are parameters of a method
            Classes.AddRange(
                types.Where(type => !typeof(HttpWebClientProtocol).IsAssignableFrom(type) && GetMethodsByType(type).Any()));
        }

        public List<MethodInfo> GetMethodsByType(Type type)
        {
            var methods =
                   Methods.Where(
                        method => method.ReturnType.FullName == type.FullName || method.ReturnType.FullName == type.FullName + "[]" ||
                                    method.GetParameters().Where(p => p.ParameterType == type).Count() > 0);

            return methods.ToList();
        }

        public static bool IsWebMethod(MethodInfo method)
        {
            object[] customAttributes = method.GetCustomAttributes(typeof(SoapRpcMethodAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                return true;
            }
            customAttributes = method.GetCustomAttributes(typeof(SoapDocumentMethodAttribute), true);
            if ((customAttributes != null) && (customAttributes.Length > 0))
            {
                return true;
            }
            customAttributes = method.GetCustomAttributes(typeof(HttpMethodAttribute), true);
            return ((customAttributes != null) && (customAttributes.Length > 0));
        }
    }
}
