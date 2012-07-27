using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    [Flags]
    /// <summary>
    /// Application collision filtering
    /// </summary>
    public enum CollisionGroups : ushort
    {
        None = 0,
        Group1 = 1 << 0,
        Group2 = 1 << 1,
        Group3 = 1 << 2,
        Group4 = 1 << 3,
        Group5 = 1 << 4,
        Group6 = 1 << 5,
        Group7 = 1 << 6,
        Group8 = 1 << 7,
        Group9 = 1 << 8,
        Group10 = 1 << 9,
        Group11 = 1 << 10,
        Group12 = 1 << 11,
        Group13 = 1 << 12,
        Group14 = 1 << 13,
        Group15 = 1 << 14,
        Group16 = 1 << 15,
        All = 65535
    }
}
