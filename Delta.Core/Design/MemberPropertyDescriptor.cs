using System;
using System.Reflection;
using System.ComponentModel;

namespace Delta.Design
{
    /// <summary>
    /// Stock class from Microsoft.Xna.Framework with public methods. 
    /// </summary>
    public abstract class MemberPropertyDescriptor : PropertyDescriptor
    {
        MemberInfo _member = null;

        public override Type ComponentType { get { return this._member.DeclaringType; } }
        public override bool IsReadOnly { get { return false; } }

        public MemberPropertyDescriptor(MemberInfo member)
            : base(member.Name, (Attribute[])member.GetCustomAttributes(typeof(Attribute), true))
        {
            _member = member;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            MemberPropertyDescriptor descriptor = obj as MemberPropertyDescriptor;
            return ((descriptor != null) && descriptor._member.Equals(_member));
        }

        public override int GetHashCode()
        {
            return _member.GetHashCode();
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }

}
