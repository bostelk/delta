using System;

namespace Delta.UI
{
    public class DebugScreen : Screen
    {
        public DebugScreen()
            : base()
        {
            Add(new PerformanceMetrics());
        }
    }
}
