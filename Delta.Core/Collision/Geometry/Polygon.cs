using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision.Geometry
{
    public class Polygon : IGeometry
    {
        /// <summary>
        /// Defined as the center of the Polygon.
        /// </summary>
        Vector2 _position;
        [ContentSerializer]
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; Calculate(); }
        }
        
        /// <summary>
        /// The Rotation of the Polygon in radians.
        /// </summary>
        float _rotation;
        [ContentSerializer]
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; Calculate(); }
        }

        /// <summary>
        /// Vertices relative to the Polygon position.
        /// </summary>
        protected Vector2[] _localVertices;
        [ContentSerializer]
        public Vector2[] LocalVertices
        {
            get
            {
                return _localVertices;
            }
            set
            {
                _localVertices = value;
                Calculate();
            }
        }

        /// <summary>
        /// Absolute position of the Vertices. Transformed by polygon center and rotation.
        /// </summary>
        protected Vector2[] _vertices;
        [ContentSerializerIgnore]
        public Vector2[] Vertices
        {
            get
            {
                return _vertices;
            }
        }

        /// <summary>
        /// Normals to polygon edges.
        /// </summary>
        protected Vector2[] _normals;
        [ContentSerializerIgnore]
        public Vector2[] Normals
        {
            get
            {
                return _normals;
            }
        }

        /// <summary>
        /// Minimum bounding area of the polygon.
        /// </summary>
        protected AABB _aabb;
        [ContentSerializerIgnore]
        public AABB AABB
        {
            get
            {
                if (_aabb == null)
                    Calculate();
                return _aabb;
            }
        }

        /// <summary>
        /// The minimum radius required to encapsulate the entire polygon.
        /// </summary>
        [ContentSerializerIgnore]
        public float BoundingRadius
        {
            get
            {
                float farthestVertex = float.MinValue;
                for (int i = 0; i < Vertices.Length; i++)
                {
                    float vertexDistance = Vector2.DistanceSquared(Position, Vertices[i]);
                    if (vertexDistance > farthestVertex)
                    {
                        farthestVertex = vertexDistance;
                    }
                }
                return (float)Math.Sqrt(farthestVertex);
            }
        }

        [ContentSerializerIgnore]
        public Matrix Transform
        {
            get
            {
                return Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            }
        }

        public Polygon() { }

        public Polygon(params Vector2[] vertices)
        {
            // transform the vertices so that 0,0 defines the center
            float xTotal = 0;
            float yTotal = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                xTotal += vertices[i].X;
                yTotal += vertices[i].Y;
            }
            Vector2 center = default(Vector2);
            center.X = xTotal / vertices.Length;
            center.Y = yTotal / vertices.Length;

            _localVertices = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                _localVertices[i].X = -center.X + vertices[i].X;
                _localVertices[i].Y = -center.Y + vertices[i].Y;
            }
            Calculate();
        }

        /// <summary>
        /// Realign the bounding box and calulate transformations.
        /// </summary>
        protected virtual void Calculate()
        {
            if (LocalVertices == null || LocalVertices.Length == 0)
                return;
            _vertices = new Vector2[LocalVertices.Length];
            _normals = new Vector2[LocalVertices.Length];
            float farthestVertexX = -float.MaxValue;
            float farthestVertexY = -float.MaxValue;
            for (int i = 0; i < _vertices.Length; i++)
            {
                // transform the vertices by the rotation and position
                _vertices[i] = Position + Vector2Extensions.Rotate(LocalVertices[i], Rotation);

                // calculate the new normals
                _normals[i] = Vector2Extensions.PerpendicularLeft(_vertices[(i + 1) % _vertices.Length] - _vertices[i]);
                _normals[i].Normalize();

                // calculate the new aabb
                float vertexDistanceX = Math.Abs(Position.X - _vertices[i].X);
                float vertexDistanceY = Math.Abs(Position.Y - _vertices[i].Y);
                if (vertexDistanceX > farthestVertexX)
                    farthestVertexX = vertexDistanceX;
                if (vertexDistanceY > farthestVertexY)
                    farthestVertexY = vertexDistanceY;
            }
            _aabb = new AABB((int)farthestVertexX, (int)farthestVertexY) { Position = Position };
        }

        public void ProjectOntoAxis(ref Vector2 axisNormal, out float min, out float max)
        {
            Vector2.Dot(ref Vertices[0], ref axisNormal, out min);
            max = min;
            float projected = 0;

            for (int i = 1; i < Vertices.Length; i++)
            {
                Vector2.Dot(ref Vertices[i], ref axisNormal, out projected);
                if (projected > max)
                    max = projected;
                if (projected < min)
                    min = projected;
            }
        }

        public static Polygon CreateRectangle(float width, float height)
        {
            float halfWidth = width / 2; float halfHeight = height / 2;
            return new Polygon(new Vector2[] {
                new Vector2(halfWidth, -halfHeight),
                new Vector2(halfWidth, halfHeight),
                new Vector2(-halfWidth, halfHeight),
                new Vector2(-halfWidth, -halfHeight),
            });
        }

        /// <summary>
        /// A less expensive check before we waste cycles on a narrow phase detection.
        /// </summary>
        /// <returns>If the two polygons are about to interesect.</returns>
        public static bool TestRadiusOverlap(Polygon polyA, Polygon polyB)
        {
            return (polyA.Position - polyB.Position).LengthSquared() <= (polyA.BoundingRadius + polyB.BoundingRadius).Square();
        }

    }
}
