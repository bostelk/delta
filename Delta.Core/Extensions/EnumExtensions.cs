using System;
using System.ComponentModel;
using System.Globalization;

namespace Delta
{
#pragma warning disable 1591
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumExtensions
    {
        #if XBOX //ripped from mscorlib.dll
        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="flag">An enumeration value.</param>
        /// <returns><b>true</b> if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, <b>false</b>.</returns>
        public static bool HasFlag(this Enum e, Enum flag)
        {
            if (e.GetType() != flag.GetType())
                throw new ArgumentException("Enum type does not match.");
            ulong num = ToUInt64(flag.GetValue());
            return ((ToUInt64(e.GetValue()) & num) == num);
        }

        static object GetValue(this Enum e)
        {
            return Enum.ToObject(e.GetType(), e);
        }

        static ulong ToUInt64(object value)
        {
            switch (Convert.GetTypeCode(value))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture);
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
            }
            throw new InvalidOperationException("Unknown enum type.");
        }
#endif
    }
}
