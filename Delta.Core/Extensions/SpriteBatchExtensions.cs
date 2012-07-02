using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Delta
{
    [Flags]
    public enum TextAlignment
    {
        Left = 2 << 1,
        Right = 2 << 2,
        Center = 2 << 3,
        Middle = 2 << 4,
        Top = 2 << 5,
        Bottom =2 << 6,
    }

    public static class SpriteBatchExtensions
    {
        public static void DrawStringOutline(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color outline)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1f, 1f), outline);
			spriteBatch.DrawString(font, text, position + new Vector2(-1f, -1f), outline);
			spriteBatch.DrawString(font, text, position + new Vector2(1f, -1f), outline);
            spriteBatch.DrawString(font, text, position + new Vector2(-1f, 1f), outline);
            spriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, TextAlignment alignment)
        {
            Vector2 textSize = font.MeasureString(text);
            Vector2 offset = Vector2.Zero;
            switch (alignment)
            {
                case TextAlignment.Left:
                    offset.X = 0;
                    break;
                case TextAlignment.Center:
                    offset.X = -textSize.X / 2;
                    break;
                case TextAlignment.Right:
                    offset.X = textSize.X;
                    break;
                case TextAlignment.Bottom:
                    offset.Y = -textSize.Y;
                    break;
                case TextAlignment.Middle:
                    offset.Y = textSize.X;
                    break;
                case TextAlignment.Top:
                    offset.Y = 0;
                    break;
            }
            spriteBatch.DrawString(font, text, position + offset, color);
        }

        public static void DrawStringShadow(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1f, 1f), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 from, Vector2 to, Color color, float thickness)
        {
            Vector2 direction = to - from;
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            spriteBatch.Draw(G.PixelTexture, from, null, color, angle, Vector2.Zero, new Vector2(direction.Length(), thickness), SpriteEffects.None, 0);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 from, float angle, float length, Color color, float thickness)
        {
            spriteBatch.Draw(G.PixelTexture, from, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, bool fill)
        {
            if (fill)
            {
                spriteBatch.Draw(G.PixelTexture, rectangle, color);
            }
            else
            {
                spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color, 1);
                spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom), color, 1);
                spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), color, 1);
                spriteBatch.DrawLine(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color, 1);
            }
        }

        public static void DrawPixel(this SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(G.PixelTexture, position, color);
        }

    }
}
