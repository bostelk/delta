using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision
{
    public class Circle : ConvexShape
    {
        const int SEGMENTS = 32;

        [ContentSerializer]
        public float Radius;

        [ContentSerializerIgnore]
        public float Circumference
        {
            get
            {
                return Radius * (float)Math.PI * 2f;
            }
        }

        [ContentSerializerIgnore]
        public float Area
        {
            get
            {
                return 0;
            }
        }

        public Circle() : this(1f) { }

        public Circle(float radius) : this(radius, SEGMENTS) { }

        public Circle(float radius, int segments)
        {
            Radius = radius;
            Vertices = new Vector2[segments];

            double theta = 0;
            double angleInterval = Math.PI * 2 / segments;
            for (int i = 0; i < segments; i ++)
            {
                Vertices[i] = default(Vector2);
                Vertices[i].X = (float)Math.Cos(theta) * radius;
                Vertices[i].Y = (float)Math.Sin(theta) * radius;
                theta += angleInterval;
            }

            CalculateNormals();
        }

        public override void CalculateAABB(ref Transform transform, out AABB aabb)
        {
            aabb.Min = transform.Origin - new Vector2(Radius, Radius);
            aabb.Max = transform.Origin + new Vector2(Radius, Radius);
        }
        
        public void CalculateExtents(ref Transform transform, out Vector2 extents)
        {
            Vector2 orientation;
            CalculateOrientation(ref transform, out orientation);
            extents = orientation * Radius;
        }

        public void ProjectOnto(ref Vector2 axis, out Vector2 projection) 
        {
            projection = Radius * axis;
        }
    }
}
