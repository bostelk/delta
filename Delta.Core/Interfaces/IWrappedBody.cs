using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Physics
{
    public interface IWrappedBody
    {
        Vector2 SimulationPosition { get; set; }
        float SimulationRotation { get; set; }
    }
}
