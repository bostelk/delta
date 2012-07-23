using System;
using Microsoft.Xna.Framework;

namespace Delta.UI
{
    public class HUD : EntityCollection<Control>
    {
        public HUD()
            : base()
        {
            Add(new PerformanceMetrics());
        }

        internal bool ProcessMouseMove()
        {
            bool handled = false;
            foreach (Control control in this)
            {
                handled = control.ProcessMouseMove();
                if (handled)
                    break;
            }
            return handled;
        }

        internal bool ProcessMouseDown()
        {
            bool handled = false;
            foreach (Control control in this)
            {
                handled = control.ProcessMouseDown();
                if (handled)
                    break;
            }
            return handled;
        }

        internal bool ProcessMouseUp()
        {
            bool handled = false;
            foreach (Control control in this)
            {
                handled = control.ProcessMouseUp();
                if (handled)
                    break;
            }
            return handled;
        }
    }
}
