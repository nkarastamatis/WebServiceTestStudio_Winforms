using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace WebServiceTestStudio.Model
{
    public class ElementDescriptor : PropertyDescriptor, ICustomTypeDescriptor
    {
        private Type _elementType;
        private Array _collection;
        private int _index = -1;

        public ElementDescriptor(Type elementType, Array collection, int index)
            : base(elementType.Name, null)
        {
            _elementType = elementType;
            _collection = collection;
            _index = index;
        }

        public override bool IsReadOnly
        {

            get { return false; }
        }

        public override void ResetValue(object component) { }

        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        { return true; }
        public override Type ComponentType
        {
            get
            {
                return typeof(Array);
            }
        }
        public override Type PropertyType
        {
            get
            {
                return _elementType;
            }
        }

        public override string DisplayName
        {
            get
            {
                return base.DisplayName + " #" + _index;
            }
        }

        public override object GetValue(object component)
        {
            var value = _collection.GetValue(_index);
            if (value == null)
            {
                value = Activator.CreateInstance(PropertyType);
                _collection.SetValue(value, _index);
            }

            return value;
        }
        public override void SetValue(object component, object value)
        {
            _collection.SetValue(value, _index);
            OnValueChanged(component, EventArgs.Empty);
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
