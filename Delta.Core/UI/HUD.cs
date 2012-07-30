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
