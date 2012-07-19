using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta
{
    public static partial class StringBuilderExtensions
    {
        // These digits are here in a static array to support hex with simple, easily-understandable code. 
        // Since A-Z don't sit next to 0-9 in the ascii table.
        private static readonly char[] ms_digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private static readonly uint ms_default_decimal_places = 5; //< Matches standard .NET formatting dp's
        private static readonly char ms_default_pad_char = '0';

        public static StringBuilder Concat(this StringBuilder stringBuilder, uint value, uint padAmount, char padChar, uint baseValue)
        {
            // Calculate length of integer when written out
            uint length = 0;
            uint length_calc = value;
            do
            {
                length_calc /= baseValue;
                length++;
            }
            while (length_calc > 0);

            // Pad out space for writing.
            stringBuilder.Append(padChar, (int)Math.Max(padAmount, length));

            int strpos = stringBuilder.Length;

            // We're writing backwards, one character at a time.
            while (length > 0)
            {
                strpos--;

                // Lookup from static char array, to cover hex values too
                stringBuilder[strpos] = ms_digits[value % baseValue];

                value /= baseValue;
                length--;
            }

            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, uint value)
        {
            stringBuilder.Concat(value, 0, ms_default_pad_char, 10);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, uint value, uint pad)
        {
            stringBuilder.Concat(value, pad, ms_default_pad_char, 10);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, uint value, uint pad, char padChar)
        {
            stringBuilder.Concat(value, pad, padChar, 10);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, int value, uint pad, char padChar, uint baseValue)
        {
            if (value < 0)  // Deal with negative numbers
            {
                stringBuilder.Append('-');
                uint num = uint.MaxValue - ((uint)value) + 1; //< This is to deal with Int32.MinValue
                stringBuilder.Concat(num, pad, padChar, baseValue);
            }
            else
                stringBuilder.Concat((uint)value, pad, padChar, baseValue);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, int value)
        {
            stringBuilder.Concat(value, 0, ms_default_pad_char, 10);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, int value, uint pad)
        {
            stringBuilder.Concat(value, pad, ms_default_pad_char, 10);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, int value, uint pad, char padChar)
        {
            stringBuilder.Concat(value, pad, padChar, 10);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, float value, uint decimalPlaces, uint padAmount, char padChar)
        {
            int num = 0;
            if (decimalPlaces == 0)
            {
                if (value >= 0.0f) //round up
                    num = (int)(value + 0.5f);
                else // Round down for negative numbers
                    num = (int)(value - 0.5f);
                stringBuilder.Concat(num, padAmount, padChar, 10);
            }
            else
            {
                num = (int)value;
                stringBuilder.Concat(num, padAmount, padChar, 10); //int
                stringBuilder.Append('.'); //decimal point
                float remainder = Math.Abs(value - num); //remainder
                do // Multiply up to become an int that we can print
                {
                    remainder *= 10;
                    decimalPlaces--;
                }
                while (decimalPlaces > 0);
                // Round up. It's guaranteed to be a positive number, so no extra work required here.
                remainder += 0.5f;
                // All done, print that as an int!
                stringBuilder.Concat((uint)remainder, 0, '0', 10);
            }
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, float value)
        {
            stringBuilder.Concat(value, ms_default_decimal_places, 0, ms_default_pad_char);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, float value, uint decimalPlaces)
        {
            stringBuilder.Concat(value, decimalPlaces, 0, ms_default_pad_char);
            return stringBuilder;
        }

        public static StringBuilder Concat(this StringBuilder stringBuilder, float value, uint decimalPlaces, uint pad)
        {
            stringBuilder.Concat(value, decimalPlaces, pad, ms_default_pad_char);
            return stringBuilder;
        }
    }
}
