using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Physics.Geometry
{
    public class Polygon : IGeometry
    {
        protected Vector2[] _normals;
        protected Vector2[] _localVertices;
        bool _normalsDirty;

        /// <summary>
        /// Defined as the center of the Polygon.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The Rotation of the Polygon in radians.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The minimum radius required to encapsulate the entire polygon.
        /// </summary>
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

        public AABB AABB
        {
            get
            {
                float farthestVertexX = float.MinValue;
                float farthestVertexY = float.MinValue;
                for (int i = 0; i < Vertices.Length; i++)
                {
                    float vertexDistanceX = Math.Abs(Position.X - Vertices[i].X);
                    float vertexDistanceY = Math.Abs(Position.Y - Vertices[i].Y);
                    if (vertexDistanceX > farthestVertexX)
                        farthestVertexX = vertexDistanceX;
                    if (vertexDistanceY > farthestVertexY)
                        farthestVertexY = vertexDistanceY;
                }
                return new AABB(farthestVertexX * 2, farthestVertexY * 2) { Position = Position };
            }
        }

        public Vector2[] Normals
        {
            get
            {
                if (_normals == null || _normalsDirty)
                {
                    _normals = new Vector2[Vertices.Length];
                    for (int i = 0; i < Vertices.Length; i++)
                    {
                        _normals[i] = Vector2Extensions.PerpendicularLeft(Vertices[(i + 1) % Vertices.Length] - Vertices[i]);
                        _normals[i].Normalize();
                    }
                    //_normalsDirty = false;
                }
                return _normals;
            }
        }

        /// <summary>
        /// Vertices relative to the Polygon position.
        /// </summary>
        public Vector2[] LocalVertices
        {
            get
            {
                return _localVertices;
            }
            set
            {
                _localVertices = value;
                _normalsDirty = true;
            }
        }

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(Position.X, Position.Y, 0);
            }
        }

        /// <summary>
        /// Absolute position of the Vertices. Transformed by polygon center and rotation.
        /// </summary>
        public Vector2[] Vertices
        {
            get
            {
                Vector2[] result = new Vector2[LocalVertices.Length];
                Matrix transform = Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(Position.X, Position.Y, 0);
                for (int i = 0; i < LocalVertices.Length; i++)
                {
                    result[i] = Vector2.Transform(LocalVertices[i], transform);
                }
                return result;
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
        }

        public void ProjectOntoAxis(Vector2 axisNormal, out float min, out float max)
        {
            min = Vector2.Dot(Vertices[0], axisNormal);
            max = min;
            float projected = 0;

            for(int i = 1; i < Vertices.Length; i++) {
                projected = Vector2.Dot(Vertices[i], axisNormal);
                if (projected > max)
                    max = projected;
                if (projected < min)
                    min = projected;
            }
        }

        /// <summary>
        /// A less expensive check before we waste cycles on a narrow phase detection.
        /// </summary>
        /// <returns>If the two polygons are about to interesect.</returns>
        public static bool BoundingRadiusIntersection(Polygon polyA, Polygon polyB)
        {
            return (polyA.Position - polyB.Position).LengthSquared() <= DeltaMath.Sqr(polyA.BoundingRadius + polyB.BoundingRadius);
        }

        /// <summary>
        /// A less expensive check before we waste cycles on a narrow phase detection.
        /// </summary>
        /// <returns>If the two polygons are about to intersect.</returns>
        public static bool AABBIntersection(Polygon polyA, Polygon polyB)
        {
            AABB a = polyA.AABB;
            AABB b = polyB.AABB;

            // Exit with no intersection if separated along an axis
            if (a.Max.X < b.Min.X || a.Min.X > b.Max.X) return false;
            if (a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y) return false;
            // Overlapping on all axes means AABBs are intersecting
            return true;
        }
    }
}
