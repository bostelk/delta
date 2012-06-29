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
        protected Vector2[] _vertices;
        bool _normalsDirty;

        /// <summary>
        /// Defined as the center of the Polygon.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The Rotation of the Polygon in radians.
        /// </summary>
        public float Rotation;

        public Vector2[] Normals
        {
            get
            {
                if (_normals == null || _normalsDirty)
                {
                    _normals = new Vector2[_vertices.Length];
                    for (int i = 0; i < _vertices.Length; i++)
                    {
                        _normals[i] = Vector2Extensions.PerpendicularLeft(_vertices[(i + 1) % _vertices.Length] - _vertices[i]);
                        _normals[i].Normalize();
                    }
                }
                return _normals;
            }
        }

        public Vector2[] LocalVertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                _vertices = value;
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

        public Vector2[] Verticies
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
            _vertices = vertices;
        }
    }
}
