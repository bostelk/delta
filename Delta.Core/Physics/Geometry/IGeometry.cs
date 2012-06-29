using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics.Geometry
{
    public interface IGeometry
    {
        Vector2[] Normals { get; }
        Vector2[] LocalVertices { get; set; }
    }
}
