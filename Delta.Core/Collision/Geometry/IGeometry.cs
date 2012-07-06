using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision.Geometry
{
    public interface IGeometry
    {
        Vector2 Position { get; set; }
        Vector2[] Normals { get; }
        Vector2[] Vertices { get; }
    }
}
