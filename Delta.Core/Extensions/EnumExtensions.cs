using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision;

namespace Delta.Extensions
{
	public static class EnumExtensions
	{

        public static bool HasFlagFast(this DebugViewFlags value, DebugViewFlags flag)
        {
            return ((value & flag) == flag);
        }

	}
}
