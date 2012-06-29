using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics.Geometry
{
    public class Box : Polygon
    {
        public object Tag;

        public float Width;

        public float Height;

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

        public Vector2 HalfSize
        {
            get
            {
                return new Vector2(Width / 2, Height / 2);
            }
        }

        public Vector2 HalfWidthX
        {
            get
            {
                return new Vector2(HalfSize.X, HalfSize.X) * Facing;
            }
        }

        public Vector2 HalfWidthY
        {
            get
            {
                return new Vector2(HalfSize.Y, HalfSize.Y) * Vector2Extensions.PerpendicularLeft(Facing);
            }
        }

        public Box(float width, float height) : base()
        {
            Width = width;
            Height = height;

            // define the vertices: top-left, top-right, bottom-right, bottom left.
            LocalVertices = new Vector2[] {
                new Vector2(width / 2, height / 2),
                new Vector2(-width / 2, height / 2),
                new Vector2(-width / 2, -height / 2),
                new Vector2(width / 2, -height / 2),
            };
        }

        public void ProjectOntoAxis(Vector2 axis, out Vector2 projection)
        {
            Vector2 vector = Vector2.Zero;
            vector.X = Vector2.Dot(HalfWidthX, axis);
            vector.Y = Vector2.Dot(HalfWidthY, axis);
            projection = vector;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)(Position.X - HalfSize.X), (int)(Position.Y - HalfSize.Y), (int)Width, (int)Height);
        }
    }
}
