using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI.Controls
{
    public class ScrollingBackground : TransformableEntity
    {
        /// <summary>
        /// Controls the horizontal and verial scrolling speed.
        /// </summary>
        public Vector2 ScrollSpeed;

        public Texture2D LowerBackground;

        public Texture2D UpperBackground;

        float scroll1, scroll2;

        Vector2 lowerOffset, upperOffset;

        public ScrollingBackground()
        {
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            Size = new Vector2((int)G.ScreenArea.Width, (int)G.ScreenArea.Height);

            scroll1 += time.ElapsedSeconds * 16.0f;
            while (scroll1 > 32.0f)
                scroll1 -= 32.0f;

            scroll2 += time.ElapsedSeconds * 20.0f;
            while (scroll2 > 32.0f)
                scroll2 -= 32.0f;

            int xOffset = 32 - (int)scroll2;
            int yOffset = (int)scroll2;

            lowerOffset = new Vector2(-32 + xOffset, -32 + yOffset);

            xOffset = (int)scroll1;
            yOffset = (int)scroll1;

            upperOffset = new Vector2(-32 + xOffset, -32 + yOffset);

            base.LightUpdate(time);
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            if (LowerBackground == null || UpperBackground == null) return;
            Rectangle sizeRect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            spriteBatch.DrawTiledArea(LowerBackground, lowerOffset, LowerBackground.Bounds, sizeRect);
            spriteBatch.DrawTiledArea(UpperBackground, upperOffset, UpperBackground.Bounds, sizeRect);
            base.Draw(time, spriteBatch);
        }
    }
}
