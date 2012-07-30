using System;
using System.Reflection;

namespace Delta.Design
{
    /// <summary>
    /// Stock class from Microsoft.Xna.Framework with public methods. 
    /// </summary>
    public class FieldPropertyDescriptor : MemberPropertyDescriptor
    {
        FieldInfo _field = null;

        public override Type PropertyType { get { return _field.FieldType; } }

        public FieldPropertyDescriptor(FieldInfo field)
            : base(field)
        {
            _field = field;
        }

        public override object GetValue(object component)
        {
            return _field.GetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            _field.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

}
