using System;
using Microsoft.Xna.Framework;

namespace Delta.UI.Controls
{
    public class Button : Label
    {
        public Button() //no difference between a label and a button right now lol.
            : base()
        {
            AutoSize = false;
            IsWordWrapped = false;
            VerticalTextAlignment = VerticalTextAlignment.Center;
            HorizontalTextAlignment = HorizontalTextAlignment.Center;
        }
    }
}
