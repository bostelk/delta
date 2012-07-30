using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace Delta.Structures
{
    internal class TimedRangeConverter : ExpandableObjectConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            if (t == typeof(string))
                return true;
            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
        {
            if (value is string)
            {
                try
                {
                    return TimedRange.Parse((string)value);
                }
                catch
                {
                    throw new ArgumentException("Invalid string format. Expected string format: 'a TimedRange formatted string'.");
                }
            }
            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is TimedRange)
            {
                TimedRange timedRange = (TimedRange)value;
                return "a TimedRange formatted string";
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    /// <summary>
    /// Defines a timed range; starting at the lower bound reaching the upper bound when the duration has full elapsed.
    /// </summary>
    [TypeConverter(typeof(TimedRangeConverter))]
    public struct TimedRange : IEquatable<TimedRange>
    {

        /// <summary>
        /// Pull the lower, upper and duration values from strings in the format: range(lower, upper, duration).
        /// </summary>
        static Regex _timedRangeRegex = new Regex(@"range\(\s*(?<value1>(\d*\.?\d+))\s*,\s*(?<value2>(\d*\.?\d+))\s*,\s*(?<duration>(\d*\.?\d+)\s*)\)");

        public float Lower;

        public float Upper;

        public float Duration;

        public static TimedRange Empty = new TimedRange();

        public TimedRange(float scalar)
        {
            Lower = scalar;
            Upper = scalar;
            Duration = 0;
        }

        public TimedRange(float lower, float upper)
        {
            Lower = lower;
            Upper = upper;
            Duration = 0;
        }

        public bool Equals(TimedRange other)
        {
            return (Lower == other.Lower && Upper == other.Upper && Duration == other.Duration);
        }

        public static TimedRange Parse(string value)
        {
            TimedRange range = Empty;
            value = value.Trim().ToLower();  // don't worry about matching whitespace inside of the string.
            Match match = _timedRangeRegex.Match(value);
            if (match.Success)
            {
                range.Lower = float.Parse(match.Groups["value1"].Value, CultureInfo.InvariantCulture);
                range.Upper = float.Parse(match.Groups["value2"].Value, CultureInfo.InvariantCulture);
                range.Duration = float.Parse(match.Groups["duration"].Value, CultureInfo.InvariantCulture);
            }
            return range;
        }

        public static explicit operator Range(TimedRange timedRange)
        {
            return new Range(timedRange.Lower, timedRange.Upper);
        }

        public bool IsEmpty()
        {
            return Lower == default(float) && Upper == default(float) && Duration == default(float);
        }
    }

}
