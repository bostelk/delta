using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace Delta.Structures
{
    public struct OverRange/*: IComparable, IFormattable, IConvertible, IComparable<float>, IEquatable<float>*/ {
        static Regex _rangeRegex = new Regex(@"range\(\s*(?<value1>(\d*\.?\d+))\s*,\s*(?<value2>(\d*\.?\d+))\s*,\s*(?<duration>(\d*\.?\d+)\s*)\)");
        static Regex _rangeStepRegex = new Regex(@"range\(\s*(?<value1>(\d*\.?\d+))\s*,\s*(?<value2>(\d*\.?\d+))\s*,\s*(?<step>(\d*\.?\d+))\s*,\s*(?<duration>(\d*\.?\d+)\s*)\)");

        public float Value1;

        public float Value2;

        public float Step;

        public float Duration;

        public static OverRange EmptyRange = new OverRange();

        public static OverRange Parse(string value)
        {
            OverRange range = EmptyRange;
            value = value.Trim();  // don't worry about matching whitespace inside of the string.
            Match match = _rangeRegex.Match(value);
            if (match.Success)
            {
                range.Value1 = float.Parse(match.Groups["value1"].Value, CultureInfo.InvariantCulture);
                range.Value2 = float.Parse(match.Groups["value2"].Value, CultureInfo.InvariantCulture);
                range.Duration = float.Parse(match.Groups["duration"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                match = _rangeStepRegex.Match(value);
                if (match.Success)
                {
                    match = _rangeStepRegex.Match(value);
                    range.Value1 = float.Parse(match.Groups["value1"].Value, CultureInfo.InvariantCulture);
                    range.Value2 = float.Parse(match.Groups["value2"].Value, CultureInfo.InvariantCulture);
                    range.Step = float.Parse(match.Groups["step"].Value, CultureInfo.InvariantCulture);
                    range.Duration = float.Parse(match.Groups["duration"].Value, CultureInfo.InvariantCulture);
                }
            }
            return range;
        }
    }

    public static class OverRangeExtensions
    {
        public static bool IsEmpty(this OverRange range)
        {
            return range.Value1 == default(float) && range.Value2 == default(float) && range.Duration == default(float);
        }
    }
}
