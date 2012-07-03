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
    public class Box : Polygon
    {
        public float Width;
        public float Height;

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)(Position.X - Width / 2), (int)(Position.Y - Height / 2), (int)Width, (int)Height);
        }
    }
}
