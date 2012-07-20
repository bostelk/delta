using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace Delta.Structures
{
    /// <summary>
    /// Defines a lower and upper bound.
    /// </summary>
    public struct Range: IEquatable<Range>/*: IComparable, IFormattable, IConvertible, IComparable<float>, IEquatable<float>*/ {

        /// <summary>
        /// Pull the lower and upper values from strings in the format: range(lower, upper).
        /// </summary>
        static Regex _rangeRegex = new Regex(@"range\(\s*(?<value1>(\d*\.?\d+))\s*,\s*(?<value2>(\d*\.?\d+))\s*\)");

        // range(lower, upper, step, duration)
        //static Regex _steppedRangeRegex = new Regex(@"range\(\s*(?<value1>(\d*\.?\d+))\s*,\s*(?<value2>(\d*\.?\d+))\s*,\s*(?<step>(\d*\.?\d+))\s*,\s*(?<duration>(\d*\.?\d+)\s*)\)");

        public float Lower;

        public float Upper;

        public static Range Empty = new Range();

        public Range(float scalar)
        {
            Lower = scalar;
            Upper = scalar;
        }

        public Range(float lower, float upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public bool Equals(Range other)
        {
            return (Lower == other.Lower && Upper == other.Upper);
        }

        /// <summary>
        /// Returns a random numbers within the lower and uppper bounds of the range.
        /// </summary>
        /// <returns></returns>
        public float RandomWithin()
        {
            return G.Random.Between(Lower, Upper);
        }
    
        public bool IsEmpty
        {
            get
            {
                return Lower == default(float) && Upper == default(float);
            }
        }

        public static Range operator -(Range value1, float value2)
        {
            value1.Lower -= value2;
            value1.Upper -= value2;
            return value1;
        }

        public static Range operator +(Range value1, float value2)
        {
            value1.Lower += value2;
            value1.Upper += value2;
            return value1;
        }

        public static Range operator *(Range value1, float value2)
        {
            float increaseBy = value2 / 2;
            value1.Lower -= increaseBy;
            value1.Upper += increaseBy;
            return value1;
        }

        public static Range operator /(Range value1, float value2)
        {
            float decreaseBy = value2 / 2;
            value1.Lower += decreaseBy;
            value1.Upper -= decreaseBy;
            return value1;
        }

        public static Range TryParse(string value)
        {
            try
            {
                return Parse(value);
            }
            catch
            {
                return new Range(float.Parse(value, CultureInfo.InvariantCulture));
            }
        }

        public static Range Parse(string value)
        {
            Range range = Empty;
            value = value.Trim().ToLower();  // don't worry about matching whitespace inside of the string.
            Match match = _rangeRegex.Match(value);
            if (match.Success)
            {
                range.Lower = float.Parse(match.Groups["value1"].Value, CultureInfo.InvariantCulture);
                range.Upper = float.Parse(match.Groups["value2"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new Exception(String.Format("The string '{0}' is not in the correct format", value));
            }
            return range;
        }

    }

}
