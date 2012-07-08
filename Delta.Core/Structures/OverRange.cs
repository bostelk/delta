using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace Delta.Structures
{
    public struct OverRange<T> where T: IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
        [ContentSerializer]
        public T Value1;

        [ContentSerializer]
        public T Value2;

        [ContentSerializer]
        public float Duration;

        public static OverRange<float> ParseFloat(string value) {
            OverRange<float> range = new OverRange<float>();
            // don't worry about matching whitespace we'll strip it off instead. also, let float.Parse handle matching floats
            Regex rangeRegex = new Regex(@"range\((?<value1>(.+)),(?<value2>(.+)),(?<duration>(.+))\)");
            Match match = rangeRegex.Match(value.Trim());
            range.Value1 = float.Parse(match.Groups["value1"].Value, CultureInfo.InvariantCulture);
            range.Value2 = float.Parse(match.Groups["value2"].Value, CultureInfo.InvariantCulture);
            range.Duration = float.Parse(match.Groups["duration"].Value, CultureInfo.InvariantCulture);
            return range;
        }

        public static OverRange<int> ParseInt(string value) {
            OverRange<int> range = new OverRange<int>();
            // don't worry about matching whitespace we'll strip it off instead. also, let float.Parse handle matching floats
            Regex rangeRegex = new Regex(@"range\((?<value1>(.+)),(?<value2>(.+)),(?<duration>(.+))\)");
            Match match = rangeRegex.Match(value.Trim());
            range.Value1 = int.Parse(match.Groups["value1"].Value, CultureInfo.InvariantCulture);
            range.Value2 = int.Parse(match.Groups["value2"].Value, CultureInfo.InvariantCulture);
            range.Duration = int.Parse(match.Groups["duration"].Value, CultureInfo.InvariantCulture);
            return range;
        }
    }

    public static class OverRangeExtensions
    {
        public static bool IsEmpty(this OverRange<float> range)
        {
            return range.Value1 == default(float) && range.Value2 == default(float) && range.Duration == default(float);
        }
    }
}
