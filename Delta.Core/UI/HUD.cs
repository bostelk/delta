using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseMove();
                if (handled)
                    break;
            }
            return handled;
        }

        internal bool ProcessMouseDown()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseDown();
                if (handled)
                    break;
            }
            return handled;
        }

        internal bool ProcessMouseUp()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseUp();
                if (handled)
                    break;
            }
            return handled;
        }
    }
}
