using System;
using Microsoft.Xna.Framework;

namespace Delta.UI.Controls
{
    public class Button : Label
    {
        public Button() //no difference between a label and a button right now lol.
            : base()
        {
            BackColor = Color.RoyalBlue * 0.5f;
            HighlightedColor = Color.Blue * 0.75f;
            PressedColor = Color.Navy;
            FocusedColor = Color.Yellow * 0.5f;
            AutoSize = false;
            IsWordWrapped = false;
        }
    }
}
