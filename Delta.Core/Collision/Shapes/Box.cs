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

        public override void CalculateAABB(ref Matrix2D transform, out AABB aabb)
        {
            Vector2 halfWidth, halfHeight;
            CalculateExtents(ref transform, out halfWidth, out halfHeight);

            Vector2 halfWidthOther, halfHeightOther;
            CalculateOtherExtents(ref transform, out halfWidthOther, out halfHeightOther);
   
            aabb.Min = Vector2.Min(halfWidth, halfWidthOther) + Vector2.Min(halfHeight, halfHeightOther) - transform.Origin;
            aabb.Max = Vector2.Max(halfWidth, halfWidthOther) + Vector2.Max(halfHeight, halfHeightOther) - transform.Origin;
        }

        public void ProjectOnto(ref Matrix2D transform, ref Vector2 axisNormal, out Vector2 projection)
        {
            Vector2 halfWidth, halfHeight;
            CalculateExtents(ref transform, out halfWidth, out halfHeight);
            halfWidth -= transform.Origin; halfHeight -= transform.Origin;
            projection.X = Vector2.Dot(halfWidth, axisNormal);
            projection.Y = Vector2.Dot(halfHeight, axisNormal);
        }

        public void CalculateExtents(ref Matrix2D transform, out Vector2 halfWidth, out Vector2 halfHeight)
        {
            halfWidth = new Vector2(HalfWidth, 0);
            halfHeight = new Vector2(0, HalfHeight);
            transform.TransformVector(ref halfWidth);
            transform.TransformVector(ref halfHeight);
        }

        public void CalculateOtherExtents(ref Matrix2D transform, out Vector2 halfWidth, out Vector2 halfHeight)
        {
            halfWidth = new Vector2(-HalfWidth, 0);
            halfHeight = new Vector2(0, -HalfHeight);
            transform.TransformVector(ref halfWidth);
            transform.TransformVector(ref halfHeight);
        }
    }
}
