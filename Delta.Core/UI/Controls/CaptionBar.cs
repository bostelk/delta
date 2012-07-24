using System;
using Microsoft.Xna.Framework;

namespace Delta.UI.Controls
{
    internal class CaptionBar : Label
    {
        internal CaptionButton _closeButton = new CaptionButton();
        internal CaptionButton _expandCollapseButton = new CaptionButton();

        public CaptionBar(Point size, bool closeButton)
            : base()
        {
            IsFocusable = false;
            Add(_closeButton);
            Add(_expandCollapseButton);
        }

       
    }
}