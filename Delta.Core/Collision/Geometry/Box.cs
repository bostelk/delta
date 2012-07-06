using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Collision.Geometry
{
    /// <summary>
    /// A box.
    /// </summary>
    public class Box : Polygon
    {
        public Vector2 Position;

        [ContentSerializer]
        public int HalfWidth { get; set; }
        
        [ContentSerializer]
        public int HalfHeight { get; set; }

        public Box() { _localVertices = new Vector2[0]; }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)(Position.X - HalfWidth), (int)(Position.Y - HalfHeight), (int)HalfWidth * 2, (int)HalfHeight * 2);
        }

    }
}
