using System;
using Microsoft.Xna.Framework;

namespace Delta.UI
{
    public class HUD : BaseControl
    {
        public HUD()
            : base()
        {
            Size = new Vector2(G.ScreenArea.Width, G.ScreenArea.Height);
            Add(new Controls.Label() { 
                Position = new Vector2(50, 50), 
                Size = new Vector2(40, 40), 
                BackColor = Color.White, 
                HighlightedColor = Color.Yellow,
                FocusedColor = Color.Green,
                ClickedColor = Color.Orange,
                IsFocusable = true
            });
            Add(new Controls.Label()
            {
                Position = new Vector2(100, 100),
                Size = new Vector2(40, 40),
                BackColor = Color.White,
                HighlightedColor = Color.Yellow,
                FocusedColor = Color.Green,
                ClickedColor = Color.Orange,
                IsFocusable = true
            });
        }
    }
}
