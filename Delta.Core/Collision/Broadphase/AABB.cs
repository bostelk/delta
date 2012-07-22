using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    /// <summary>
    /// An Axis-Aligned Bounding Box.
    /// </summary>
    public struct AABB : IEquatable<AABB>
    {
        public static AABB Zero = new AABB(Vector2.Zero, Vector2.Zero);

        public Vector2 Min;
        public Vector2 Max;

        public AABB(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }
             
        /// <summary>
        /// A less expensive check before we waste cycles on a narrow phase detection.
        /// </summary>
        /// <returns>If the two polygons are about to intersect.</returns>
        public static bool TestOverlap(AABB a, AABB b)
        {
            // Exit with no intersection if separated along an axis
            if (a.Max.X < b.Min.X || a.Min.X > b.Max.X) return false;
            if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y) return false;
            // Overlapping on all axes means AABBs are intersecting
            return true;
        }

        public bool Equals(AABB other)
        {
            return Min == other.Min && Max == other.Max;
        }
    }
}
