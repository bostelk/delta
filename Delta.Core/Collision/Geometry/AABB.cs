using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision.Geometry
{
    /// <summary>
    /// An Axis-Aligned Bounding Box. ie. it will not reflect rotations;
    /// </summary>
    public class AABB
    {
        Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                Realign();
            }
        }

        public int HalfWidth { get; private set; }
        public int HalfHeight { get; private set; }
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }

        public Vector2[] Vertices
        {
            get
            {
                return new Vector2[] {
                    Position + new Vector2(HalfWidth, -HalfHeight),
                    Position + new Vector2(HalfWidth, HalfHeight),
                    Position + new Vector2(-HalfWidth, HalfHeight),
                    Position + new Vector2(-HalfWidth, -HalfHeight),
                };
            }
        }

        public AABB(int halfWidth, int halfHeight)
        {
            Position = Vector2.Zero;
            HalfWidth = halfWidth;
            HalfHeight = halfHeight;
        }

        private void Realign()
        {
            Min = _position - new Vector2(HalfWidth, HalfHeight);
            Max = _position + new Vector2(HalfWidth, HalfHeight);
        }

        public static bool TestContains(AABB a, Vector2 position)
        {
            return true;
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

    }
}
