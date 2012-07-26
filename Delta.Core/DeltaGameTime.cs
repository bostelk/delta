using System;
using Microsoft.Xna.Framework;

namespace Delta
{
    public sealed class DeltaGameTime
    {
        public float TotalSeconds { get; internal set; }
        public float ElapsedSeconds { get; internal set; }
    }
}
