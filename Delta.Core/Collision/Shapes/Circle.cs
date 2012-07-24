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

        float _radius;
        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                // can't have negative or zero values.
                _radius = Math.Max((float)MathExtensions.EPSILON, value);
            }
        }

        [ContentSerializerIgnore]
        public float Circumference
        {
            get
            {
                return Radius * (float)Math.PI * 2f;
            }
        }

        public Circle() : this(1f) { }

        public Circle(float radius) : this(radius, SEGMENTS) { }

        public Circle(float radius, int segments)
        {
            Radius = radius;
            Vertices = new Vector2[segments];
            Normals = new Vector2[segments];

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

        public override void CalculateAABB(ref Matrix2D transform, out AABB aabb)
        {
            Vector2 radius = new Vector2(Radius, Radius);
            aabb.Min = transform.Origin - radius;
            aabb.Max = transform.Origin + radius;
        }
        
        public void CalculateExtents(ref Matrix2D transform, out Vector2 extents)
        {
            extents = new Vector2(Radius, 0);
            transform.TransformVector(ref extents);
        }

        public void ProjectOnto(ref Vector2 axis, out Vector2 projection) 
        {
            projection = Radius * axis;
        }

    }
}
