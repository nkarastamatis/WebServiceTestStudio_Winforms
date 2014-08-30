using System;
using System.ComponentModel;
using System.Globalization;

namespace WebServiceTestStudio.Model
{
    class ArrayConverter : ExpandableObjectConverter, ICustomTypeDescriptor
    {

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value != null && value.GetType().Namespace != typeof(System.String).Namespace)
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //var cols = base.GetProperties();
            var props = new PropertyDescriptorCollection(null);

            if (value is Array)
            {
                var collection = (Array)value;
                Type arrayType = collection.GetType().GetElementType();
                for (int i = 0; i < collection.Length; i++)
                {
                    var prop = new ElementDescriptor(arrayType, collection, i);
                    props.Add(prop);
                }
            }


            if (props.Count > 0)
                return props;

            return base.GetProperties(context, value, attributes);
        }

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string GetClassName()
        {
            throw new NotImplementedException();
        }

        public string GetComponentName()
        {
            throw new NotImplementedException();
        }

        public TypeConverter GetConverter()
        {
            throw new NotImplementedException();
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            throw new NotImplementedException();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
