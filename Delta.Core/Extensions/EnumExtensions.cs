using System;
using System.ComponentModel;
using System.Globalization;

namespace Delta
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="flag">An enumeration value.</param>
        /// <returns><b>true</b> if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, <b>false</b>.</returns>
        public static bool HasFlag(this Enum e, Enum flag)
        {
            ulong num = Convert.ToUInt64(flag.GetValue());
            return ((Convert.ToUInt64(e.GetValue()) & num) == num);
        }

        static object GetValue(this Enum e)
        {
            return Enum.ToObject(e.GetType(), e);
        }
    }
}
