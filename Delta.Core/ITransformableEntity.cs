using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta
{
    public interface ITransformableEntity
    {
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
        Vector2 Scale { get; set; }
        float Rotation { get; set; }
        Color Tint { get; set; }
    }
}
