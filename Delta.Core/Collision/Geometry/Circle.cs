using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision.Geometry
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
            double theta = 0;
            for (int i = 0; i < segments; i ++)
            {
                LocalVertices[i] = default(Vector2);
                LocalVertices[i].X = (float)Math.Cos(theta) * radius;
                LocalVertices[i].Y = (float)Math.Sin(theta) * radius;
                theta+= angleInterval;
            }
            Calculate();
        }

        /// <summary>
        /// Keeping the polygon fresh.
        /// </summary>
        protected override void Calculate()
        {
            _vertices = new Vector2[LocalVertices.Length];
            _normals = new Vector2[LocalVertices.Length];
            for (int i = 0; i < _vertices.Length; i++)
            {
                // transform the vertices by the rotation and position
                _vertices[i] = LocalVertices[i];

                // calculate the new normals
                _normals[i] = Vector2Extensions.PerpendicularLeft(_vertices[(i + 1) % _vertices.Length] - _vertices[i]);
                _normals[i].Normalize();
            }
            _aabb = new AABB((int)Radius, (int)Radius) { Position = Position };
        }

        public void ProjectOntoAxis(ref Vector2 axis, out Vector2 projection) {
            projection = Radius * axis;
        }
    }
}
