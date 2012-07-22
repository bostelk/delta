using System;
using Microsoft.Xna.Framework;

namespace Delta.UI
{
    public class HUD : BaseControl
    {
        public HUD()
            : base()
        {
            Add(new Controls.Label()
            {
                AutoSize = false,
                Position = new Vector2(50, 50),
                Size = new Vector2(40, 40),
                BackColor = Color.White,
                HighlightedColor = Color.Transparent,
                FocusedColor = Color.Green,
                ClickedColor = Color.Orange,
            });
            Add(new Controls.Label()
            {
                AutoSize = false,
                Position = new Vector2(100, 100),
                Size = new Vector2(40, 40),
                BackColor = Color.White,
                HighlightedColor = Color.Yellow,
                FocusedColor = Color.Green,
                ClickedColor = Color.Orange,
            });
            Add(new PerformanceMetrics() { BackColor = Color.DarkGray, HighlightedColor = Color.DarkGreen, FocusedColor = Color.DarkOrange, ClickedColor = Color.DarkOrchid });
        }
    }
}
