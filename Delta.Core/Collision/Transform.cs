using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    /// <summary>
    /// A 2x2 Transformation matrix; currently lacking a lot of code...
    /// </summary>
    public struct Transform
    {
        public static Transform Identity = new Transform();

        public Vector2 Origin;
        public float Rotation;
    }
}
