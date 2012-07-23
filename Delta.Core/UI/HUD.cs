using System;
using Microsoft.Xna.Framework;

namespace Delta.UI
{
    public class HUD : BaseControl
    {
        public HUD()
            : base()
        {
            Add(new PerformanceMetrics());
        }
    }
}
