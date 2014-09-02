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
    public class NewWebServiceAddedEventArgs
    {
        public NewWebServiceAddedEventArgs(Type wsType)
        {
            Type = wsType;
        }

        public Type Type { get; private set; }
    }
}
