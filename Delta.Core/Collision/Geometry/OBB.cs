using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision.Geometry
{
    /// <summary>
    /// An Orientated Bounding Box. ie. it will reflect rotations.
    /// </summary>
    public class OBB : Polygon
    {
        [ContentSerializer]
        public float HalfWidth { get; private set; }
        [ContentSerializer]
        public float HalfHeight { get; private set; }

        /// <summary>
        /// The Unit vector that points along the x-axis.
        /// </summary>
        public Vector2 Orientation
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }
        }

        public Vector2 PerpOrientation
        {
            get
            {
                return Vector2Extensions.PerpendicularLeft(Orientation);
            }
        }

        public Vector2 HalfWidthX
        {
            get
            {
                return new Vector2(HalfWidth, HalfWidth) * Orientation;
            }
        }

        public Vector2 HalfWidthY
        {
            get
            {
                return new Vector2(HalfHeight, HalfHeight) * PerpOrientation;
            }
        }

        public OBB() { }

        /// <summary>
        /// Create an Orientated Bounding Box
        /// </summary>
        /// <param name="width">Box width in pixels.</param>
        /// <param name="height">Box height in pixels.</param>
        public OBB(float width, float height) : base()
        {
            HalfWidth = width / 2;
            HalfHeight = height / 2;

            LocalVertices = new Vector2[] {
                new Vector2(HalfWidth, HalfHeight),
                new Vector2(-HalfWidth, HalfHeight),
                new Vector2(-HalfWidth, -HalfHeight),
                new Vector2(HalfWidth, -HalfHeight),
            };
            Calculate();
        }

        public void ProjectOntoAxis(ref Vector2 axisNormal, out Vector2 projection)
        {
            //Vector2.Dot(ref HalfWidthX, ref axisNormal, out projection.X);
            projection.X = Vector2.Dot(HalfWidthX, axisNormal);
            projection.Y = Vector2.Dot(HalfWidthY, axisNormal);
        }

    }
}
