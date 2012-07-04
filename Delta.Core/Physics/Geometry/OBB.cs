using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics.Geometry
{
    /// <summary>
    /// An Orientated Bounding Box. ie. it will reflect rotations.
    /// </summary>
    public class OBB : Polygon
    {
        public object Tag;
        
        public float HalfWidth;
        public float HalfHeight;

        /// <summary>
        /// The Unit vector that points along the x-axis.
        /// </summary>
        public Vector2 Facing
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            }
        }

        public Vector2 PerpFacing
        {
            get
            {
                return Vector2Extensions.PerpendicularLeft(Facing);
            }
        }

        public Vector2 HalfWidthX
        {
            get
            {
                return new Vector2(HalfWidth, HalfWidth) * Facing;
            }
        }

        public Vector2 HalfWidthY
        {
            get
            {
                return new Vector2(HalfHeight, HalfHeight) * Vector2Extensions.PerpendicularLeft(Facing);
            }
        }

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

        public void ProjectOntoAxis(Vector2 axisNormal, out Vector2 projection)
        {
            projection.X = Vector2.Dot(HalfWidthX, axisNormal);
            projection.Y = Vector2.Dot(HalfWidthY, axisNormal);
        }

    }
}
