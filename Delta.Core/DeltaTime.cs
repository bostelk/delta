using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta
{
    public struct DeltaTime
    {
        public float TotalSeconds { get; internal set; }
        public float ElapsedSeconds { get; internal set; }
        public bool IsRunningSlowly { get; internal set; }
    }
}
