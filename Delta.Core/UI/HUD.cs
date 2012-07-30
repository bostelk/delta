using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Delta.UI.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI
{
    public class HUD : EntityParent<Control>
    {
        public HUD()
            : base()
        {
            Name = "hud";
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

#if WINDOWS
        internal bool ProcessMouseMove()
        {
            bool handled = false;
            for (int x = Children.Count - 1; x >= 0; x--)
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
            for (int x = Children.Count - 1; x >= 0; x--)
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
            for (int x = Children.Count - 1; x >= 0; x--)
            {
                handled = Children[x].ProcessMouseUp();
                if (handled)
                    break;
            }
            return handled;
        }
#endif

        protected override void OnBeginDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnBeginDraw(time, spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, SpriteBatchExtensions._cullRasterizerState, null);
        }

        protected override void OnEndDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnEndDraw(time, spriteBatch);
            spriteBatch.End();
        }
    }
}
