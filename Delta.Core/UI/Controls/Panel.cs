using System;
using Microsoft.Xna.Framework;

namespace Delta.UI.Controls
{
    public class Panel : Control
    {
        public Panel() //no difference between a control and a panel right now lol.
            : base()
        {
            BackColor = new Color(170, 0, 150, 255) * 0.5f;
            HighlightedColor = Color.MediumBlue * 0.75f;
            ClickedColor = new Color(170, 0, 150, 255) * 0.75f;
            FocusedColor = new Color(170, 0, 150, 255) * 0.5f;
        }
    }
}
