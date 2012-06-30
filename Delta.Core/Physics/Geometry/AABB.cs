using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics.Geometry
{
    /// <summary>
    /// An Axis-Aligned box. ie. it will not reflect rotations;
    /// </summary>
    public class AABB : Box
    {
        public AABB(float width, float height) : base()
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

    }
}
