using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics.Geometry
{
    public class Circle : Polygon
    {
        const int SEGMENTS = 32;

        public float Radius;

        public float Circumference
        {
            get
            {
                return Radius * (float)Math.PI * 2f;
            }
        }

        public float Area
        {
            get
            {
                return 0;
            }
        }

        public Vector2 Orientation
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation) * Radius, (float)Math.Sin(Rotation) * Radius);
            }
        }

        public Circle(float radius) : this(radius, SEGMENTS) { }

        public Circle(float radius, int segments)
        {
            Radius = radius;
            LocalVertices = new Vector2[segments];
            double angleInterval = Math.PI * 2 / segments;
            for (double theta = 0; theta < Math.PI * 2; theta+= angleInterval)
            {
                Vector2 position = default(Vector2);
                position.X = (float)Math.Cos(theta) * radius;
                position.Y = (float)Math.Sin(theta) * radius;
            }
            Calculate();
        }

        public void ProjectOntoAxis(Vector2 axis, out Vector2 projection) {
            projection = Radius * axis;
        }
    }
}
