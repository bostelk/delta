using System;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using Microsoft.Xna.Framework.Design;
using System.Diagnostics.CodeAnalysis;

namespace Delta.Design
{
    public class TimedRangeConverter : MathTypeConverter
    {
        public TimedRangeConverter()
        {
            Type type = typeof(TimedRange);
            PropertyDescriptions = new PropertyDescriptorCollection(new PropertyDescriptor[] { new FieldPropertyDescriptor(type.GetField("Lower")), new FieldPropertyDescriptor(type.GetField("Upper")), new FieldPropertyDescriptor(type.GetField("Duration")) }).Sort(new string[] { "Lower", "Upper", "Duration" });
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            float[] values = MathTypeConverter.ConvertToValues<float>(context, culture, value, 3, new string[] { "Lower", "Upper", "Duration" });
            if (values != null)
                return new TimedRange(values[0], values[1], values[2]);
            return base.ConvertFrom(context, culture, value);
        }

        [SuppressMessage("Microsoft.Performance", "CA1800")]
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");
            if (value is TimedRange)
            {
                TimedRange timedRange = (TimedRange)value;
                if (destinationType == typeof(string))
                    return MathTypeConverter.ConvertFromValues<float>(context, culture, new float[] { timedRange.Lower, timedRange.Upper, timedRange.Duration });
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo constructor = typeof(TimedRange).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float) });
                    if (constructor != null)
                        return new InstanceDescriptor(constructor, new object[] { timedRange.Lower, timedRange.Upper, timedRange.Duration });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
                throw new ArgumentNullException("propertyValues");
            return new TimedRange((float)propertyValues["Lower"], (float)propertyValues["Upper"], (float)propertyValues["Duration"]);
        }
    }

    //public class TimedRangeConverter : ExpandableObjectConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
    //    }

    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        return ((destinationType == typeof(TimedRange)) || base.CanConvertTo(context, destinationType));
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        string s = value as string;
    //        if (s == null || string.IsNullOrEmpty(s.Trim()))
    //            return base.ConvertFrom(context, culture, value);
    //        try
    //        {
    //            string[] parse = ((string)value).Split(new string[] { culture.TextInfo.ListSeparator }, StringSplitOptions.RemoveEmptyEntries);
    //            return new TimedRange(float.Parse(parse[0], culture), float.Parse(parse[1], culture), float.Parse(parse[2], culture));
    //        }
    //        catch
    //        {
    //            throw new ArgumentException("Invalid string format. Expected a string in the format 'lower,upper,duration'.");
    //        }
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (value is TimedRange)
    //        {
    //            TimedRange timedRange = (TimedRange)value;
    //            if (destinationType == typeof(string))
    //                return string.Format("{1}{0}{2}{0}{3}", culture.TextInfo.ListSeparator, timedRange.Lower, timedRange.Upper, timedRange.Duration);
    //            else if (destinationType == typeof(InstanceDescriptor))
    //            {
    //                ConstructorInfo constructor = typeof(TimedRange).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float) });
    //                if (constructor != null)
    //                    return new InstanceDescriptor(constructor, new object[] { timedRange.Lower, timedRange.Upper, timedRange.Duration });
    //            }
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }

    //    public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
    //    {
    //        if (propertyValues == null)
    //            throw new ArgumentNullException("propertyValues");
    //        try
    //        {
    //            return new TimedRange((float)propertyValues["Lower"], (float)propertyValues["Upper"], (float)propertyValues["Duration"]);
    //        }
    //        catch
    //        {
    //            throw new ArgumentException("Invalid properties.");
    //        }
    //    }

    //    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }

    //    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    //    {
    //        return TypeDescriptor.GetProperties(typeof(TimedRange), attributes).Sort(new string[] { "Lower", "Upper", "Duration"});
    //    }

    //    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }


    //}
}
