using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta
{

    public struct SpriteDirection
    {
    }

    public enum CardinalDirection
    {
        North,
        East,
        South,
        West
    }

    public enum IntercardinalDirection
    {
        None,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }
}
