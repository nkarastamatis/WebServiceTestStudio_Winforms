using System;
using System.ComponentModel;

namespace WebServiceTestStudio.Model
{
    public class TypeDescriptorModifier
    {
        public static void modifyType(Type type)
        {
            if (type.FullName == "System.Dynamic.ExpandoObject")
            {
                TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(ExpandoObjectConverter)));
            }
            else if (type.BaseType.FullName == "System.Array")
            {
                TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(ArrayConverter)));
                Type elemType = type.GetElementType();
                TypeDescriptorModifier.modifyType(elemType);
            }
            else if (type.Namespace != "System" && type.BaseType.Name != "Enum")
            {
                TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(WsdlObjectConverter)));
            }
        }
    }
}
