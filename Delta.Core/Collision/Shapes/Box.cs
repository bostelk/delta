using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision
{
    /// <summary>
    /// An Orientated Bounding Box. ie. it will reflect rotations.
    /// </summary>
    public class Box : ConvexShape
    {
        float _halfWidth;
        public float HalfWidth 
        {
            get
            {
                return _halfWidth;
            }
            set
            {
                // can't have negative of zero values.
                _halfWidth = Math.Max((float)MathExtensions.EPSILON, value);
                OnShapeChanged();
            }
        }

        float _halfHeight;
        public float HalfHeight
        {
            get
            {
                return _halfHeight;
            }
            set
            {
                // can't have negative of zero values.
                _halfHeight = Math.Max((float)MathExtensions.EPSILON, value);
                OnShapeChanged();
            }
        }

        public Box() : this(1, 1) { }

        /// <summary>
        /// Create an Orientated Bounding Box
        /// </summary>
        /// <param name="width">Box width in pixels.</param>
        /// <param name="height">Box height in pixels.</param>
        public Box(float width, float height) : base()
        {
            HalfWidth = width / 2;
            HalfHeight = height / 2;

            // defined in a clock-wise fasion starting from the top-left corner.
            Vertices = new Vector2[] {
                new Vector2(-HalfWidth, HalfHeight),
                new Vector2(HalfWidth, HalfHeight),
                new Vector2(HalfWidth, -HalfHeight),
                new Vector2(-HalfWidth, -HalfHeight),
            };

            Normals = new Vector2[] {
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),
            };
        }

        public override void CalculateAABB(ref Transform transform, out AABB aabb)
        {
            aabb.Min = transform.Origin - new Vector2(HalfWidth, HalfHeight);
            aabb.Max = transform.Origin + new Vector2(HalfWidth, HalfHeight);
        }

        public void ProjectOnto(ref Transform transform, ref Vector2 axisNormal, out Vector2 projection)
        {
            Vector2 halfwidthX, halfwidthY;
            CalculateExtents(ref transform, out halfwidthX, out halfwidthY);
            projection.X = Vector2.Dot(halfwidthX, axisNormal);
            projection.Y = Vector2.Dot(halfwidthY, axisNormal);
        }

        public void CalculateExtents(ref Transform transform, out Vector2 halfwidthX, out Vector2 halfwidthY)
        {
            Vector2 orientation;
            CalculateOrientation(ref transform, out orientation);
            halfwidthX = orientation * HalfWidth;
            halfwidthY = Vector2Extensions.PerpendicularLeft(orientation) * HalfHeight;
        }

    }
}
