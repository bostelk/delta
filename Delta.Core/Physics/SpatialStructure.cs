using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Physics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Delta.Physics
{
    public abstract class SpatialStructure
    {
        public abstract void DrawDebug(ref Matrix view, ref Matrix projection);
    }
}
