using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision
{
    public class Polygon : ConvexShape
    {

        public Polygon() { }

        public Polygon(params Vector2[] vertices)
        {
            Vertices = new Vector2[vertices.Length];
            Normals = new Vector2[vertices.Length];

            // re-center the polygon so that the centroid is on the origin.
            CalculateCentroid();
            if (!Vector2Extensions.AlmostZero(Centroid))
            {
                for (int i = 0; i < vertices.Length; i++)
                    Vertices[i] = Vertices[i] - Centroid;
            }
            CalculateCentroid();
        }

        public override void CalculateAABB(ref Matrix3 transform, out AABB aabb)
        {
            float farthestX, farthestY;
            farthestX = farthestY = 0;
            for (int i = 0; i < Vertices.Length; i++)
            {
                // calculate the new aabb
                float vertexDistanceX = Math.Abs(Vertices[i].X);
                float vertexDistanceY = Math.Abs(Vertices[i].Y);
                if (vertexDistanceX > farthestX)
                    farthestX = vertexDistanceX;
                if (vertexDistanceY > farthestY)
                    farthestY = vertexDistanceY;
            }
            aabb.Min = transform.Origin - new Vector2(farthestX, farthestY);
            aabb.Max = transform.Origin + new Vector2(farthestX, farthestY);
        }

    }
}
