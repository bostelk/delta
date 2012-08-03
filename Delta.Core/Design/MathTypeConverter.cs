using System;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.Design.Serialization;

namespace Delta.Design
{
    /// <summary>
    /// Stock class from Microsoft.Xna.Framework with public methods. 
    /// </summary>
    public class MathTypeConverter : ExpandableObjectConverter
    {
        [SuppressMessage("Microsoft.Design", "CA1051")]
        protected PropertyDescriptorCollection PropertyDescriptions { get; set; }
        protected bool SupportStringConvert { get; set; }

        public MathTypeConverter()
            : base()
        {
            SupportStringConvert = true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return ((SupportStringConvert && (sourceType == typeof(string))) || base.CanConvertFrom(context, sourceType));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public static string ConvertFromValues<T>(ITypeDescriptorContext context, CultureInfo culture, T[] values)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;
            string separator = culture.TextInfo.ListSeparator + " ";
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            string[] stringValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                stringValues[i] = converter.ConvertToString(context, culture, values[i]);
            return string.Join(separator, stringValues);
        }

        internal static T[] ConvertToValues<T>(ITypeDescriptorContext context, CultureInfo culture, object value, int arrayCount, params string[] expectedParams)
        {
            string stringValue = value as string;
            if (stringValue == null)
                return null;
            stringValue = stringValue.Trim();
            if (culture == null)
                culture = CultureInfo.CurrentCulture;
            string[] stringValues = stringValue.Split(new string[] { culture.TextInfo.ListSeparator }, StringSplitOptions.None);
            T[] values = new T[stringValues.Length];
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            for (int i = 0; i < values.Length; i++)
            {
                try
                {
                    values[i] = (T)converter.ConvertFromString(context, culture, stringValues[i]);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid string format. Expected a string in the format '{0}'."
, new object[] { string.Join(culture.TextInfo.ListSeparator, expectedParams) }), exception);
                }
            }
            if (values.Length != arrayCount)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid string format. Expected a string in the format '{0}'.", new object[] { string.Join(culture.TextInfo.ListSeparator, expectedParams) }));
            }
            return values;
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return PropertyDescriptions;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }


}
