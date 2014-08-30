using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace WebServiceTestStudio.Model
{
    class WsdlObjectConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value != null && value.GetType().Namespace != "System")
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //var cols = base.GetProperties();
            var props = new PropertyDescriptorCollection(null);


            foreach (FieldInfo fi in value.GetType().GetFields())
            {
                var prop = new WsdlFieldDescriptor(fi, attributes);
                props.Add(prop);
                if (fi.FieldType.BaseType.FullName == "System.Array")
                {
                    TypeDescriptor.AddAttributes(fi.FieldType, new TypeConverterAttribute(typeof(ArrayConverter)));
                    Type elemType = fi.FieldType.GetElementType();
                    TypeDescriptorModifier.modifyType(elemType);
                }
                else if (fi.FieldType.BaseType.FullName == "System.Enum")
                {
                }
                else
                {
                    TypeDescriptorModifier.modifyType(fi.FieldType);
                }
            }

            if (props.Count > 0)
                return props;

            return base.GetProperties(context, value, attributes);
        }
    }
}
