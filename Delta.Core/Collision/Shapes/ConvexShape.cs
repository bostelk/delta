using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    public abstract class ConvexShape : CollisionShape
    {
        public Vector2 Centroid { get; protected set; }

        public ConvexShape()
        {
        }

        protected virtual void CalculateNormals()
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Normals[i] = Vector2Extensions.PerpendicularLeft(Vertices[(i + 1) % Vertices.Length] - Vertices[i]);
                Normals[i].Normalize();
            }
        }

        protected virtual void CalculateExtents()
        {
            float farthestVertexX = -float.MaxValue;
            float farthestVertexY = -float.MaxValue;
            for (int i = 0; i < Vertices.Length; i++)
            {
                float vertexDistanceX = Math.Abs(Vertices[i].X - Centroid.X);
                float vertexDistanceY = Math.Abs(Vertices[i].Y - Centroid.Y);
                if (vertexDistanceX > farthestVertexX)
                    farthestVertexX = vertexDistanceX;
                if (vertexDistanceY > farthestVertexY)
                    farthestVertexY = vertexDistanceY;
            }
        }

        protected virtual void CalculateCentroid()
        {
            Vector2 result = Vector2.Zero;
            float xTotal = 0;
            float yTotal = 0;
            for (int i = 0; i < Vertices.Length; i++)
            {
                xTotal += Vertices[i].X;
                yTotal += Vertices[i].Y;
            }
            result.X = xTotal / Vertices.Length;
            result.Y = yTotal / Vertices.Length;
            Centroid = result;
        }

        public float BoundingRadius
        {
            get
            {
                float farthestVertex = float.MinValue;
                for (int i = 0; i < Vertices.Length; i++)
                {
                    float vertexDistance = Vector2.DistanceSquared(Centroid, Vertices[i]);
                    if (vertexDistance > farthestVertex)
                    {
                        farthestVertex = vertexDistance;
                    }
                }
                return (float)Math.Sqrt(farthestVertex);
            }
        }

        public void CalculateOrientation(ref Matrix3 transform, out Vector2 orientation)
        {
            orientation = Vector2.UnitX;
            transform.TransformVector(ref orientation);
            orientation -= transform.Origin;
            //orientation.X = (float)Math.Cos(transform.Rotation);
            //orientation.Y = (float)Math.Sin(transform.Rotation);
        }

        public void ProjectOnto(ref Vector2 axis, out float min, out float max)
        {
            Vector2.Dot(ref Vertices[0], ref axis, out min);
            max = min;
            float projected = 0;

            for (int i = 1; i < Vertices.Length; i++)
            {
                Vector2.Dot(ref Vertices[i], ref axis, out projected);
                if (projected > max)
                    max = projected;
                if (projected < min)
                    min = projected;
            }
        }

        ///// <summary>
        ///// A less expensive check before we waste cycles on a narrow phase detection.
        ///// </summary>
        ///// <returns>If the two polygons are about to interesect.</returns>
        //public static bool TestRadiusOverlap(Polygon polyA, Polygon polyB)
        //{
        //    return (polyA.Position - polyB.Position).LengthSquared() <= (polyA.BoundingRadius + polyB.BoundingRadius).Square();
        //}

    }
}
