using System;
using Microsoft.Xna.Framework;

namespace Delta
{
    public sealed class DeltaTime
    {
        public float TotalSeconds { get; internal set; }
        public float ElapsedSeconds { get; internal set; }
        public bool IsRunningSlowly { get; internal set; }
    }
}
