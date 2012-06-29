using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    public interface ITransformable
    {
        Vector2 Position { get; set; }
        Vector2 Scale { get; set; }
        float Rotation { get; set; }
        Color Tint { get; set; }
    }
}
