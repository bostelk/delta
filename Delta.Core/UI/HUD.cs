using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Delta.UI.Controls;

namespace Delta.UI
{
    public class HUD : EntityParent<Control>
    {
        public HUD()
            : base()
        {
            Add(new PerformanceMetrics());
        }

        public void RegisterForTweaking(object obj)
        {
            Dictionary<string, Delta.Tweaker.ITweak> tweakables = Tweaker.FindTweakables(obj);
            Label tweakingLabel = new Label();
            tweakingLabel.Text.Append(String.Format("Object: {0}\n", obj.ToString()));
            foreach(KeyValuePair<string, Delta.Tweaker.ITweak> tweakable in tweakables) {
                tweakingLabel.Text.Append(String.Format("Variable: {0} Value {1}\n", tweakable.Value.VariableName, tweakable.Value.GetValue()));
            }
            tweakingLabel.Position = new Vector2(0, 120);
            Add(tweakingLabel);
        }

        internal bool ProcessMouseMove()
        {
            bool handled = false;
            //foreach (Control control in this)
            //{
            //    handled = control.ProcessMouseMove();
            //    if (handled)
            //        break;
            //}
            return handled;
        }

        internal bool ProcessMouseDown()
        {
            bool handled = false;
            //foreach (Control control in this)
            //{
            //    handled = control.ProcessMouseDown();
            //    if (handled)
            //        break;
            //}
            return handled;
        }

        internal bool ProcessMouseUp()
        {
            bool handled = false;
            //foreach (Control control in this)
            //{
            //    handled = control.ProcessMouseUp();
            //    if (handled)
            //        break;
            //}
            return handled;
        }
    }
}
