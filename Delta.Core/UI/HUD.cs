using System;
using Microsoft.Xna.Framework;

namespace Delta.UI
{
    public class HUD : BaseControl
    {
        public HUD()
            : base()
        {
            Add(new PerformanceMetrics() { BackColor = Color.DarkGray, HighlightedColor = Color.DarkGreen, FocusedColor = Color.DarkOrange, ClickedColor = Color.DarkOrchid });
        }
    }
}
