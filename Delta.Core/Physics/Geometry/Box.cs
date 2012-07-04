using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics.Geometry
{
    /// <summary>
    /// A box.
    /// </summary>
    public class Box
    {
        public Vector2 Position;
        public int HalfWidth { get; protected set; }
        public int HalfHeight { get; protected set; }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)(Position.X - HalfWidth), (int)(Position.Y - HalfHeight), (int)HalfWidth * 2, (int)HalfHeight * 2);
        }
    }
}
