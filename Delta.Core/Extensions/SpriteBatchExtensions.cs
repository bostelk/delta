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
        /// <summary>
        /// Align to X
        /// </summary>
        Center = 2 << 3,
        /// <summary>
        /// Align to Y
        /// </summary>
        Middle = 2 << 4,
        Top = 2 << 5,
        Bottom = 2 << 6,
    }

    public static class SpriteBatchExtensions
    {
        internal static RasterizerState _cullRasterizerState = new RasterizerState() { ScissorTestEnable = true };

        public static void Begin(this SpriteBatch spriteBatch, SpriteSortMode spriteSortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, Effect effect, Camera camera, Rectangle scissor)
        {
            spriteBatch.GraphicsDevice.ScissorRectangle = scissor;
            spriteBatch.Begin(spriteSortMode, blendState, samplerState, depthStencilState, _cullRasterizerState, effect, camera.View);
        }

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
            if ((alignment & TextAlignment.Left) != 0)
                offset.X = 0;
            else if ((alignment & TextAlignment.Center) != 0)
                offset.X = -(int)textSize.X / 2; // cast to int to avoid aliasing.
            else if ((alignment & TextAlignment.Right) != 0)
                offset.X = -textSize.X;
            if ((alignment & TextAlignment.Top) != 0)
                offset.Y = 0;
            else if ((alignment & TextAlignment.Middle) != 0)
                offset.Y -= (int)textSize.Y / 2; // cast to int to avoid aliasing.
            else if ((alignment & TextAlignment.Bottom) != 0)
                offset.Y = -textSize.Y;
            spriteBatch.DrawString(font, text, position + offset, color);
        }

        public static void DrawStringShadow(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, position + new Vector2(1f, 1f), Color.Black);
            spriteBatch.DrawString(font, text, position, color);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        {
            Vector2 direction = end - start;
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            spriteBatch.Draw(G.PixelTexture, start, null, color, angle, Vector2.Zero, new Vector2(direction.Length(), thickness), SpriteEffects.None, 0);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, float angle, float length, Color color, float thickness)
        {
            spriteBatch.Draw(G.PixelTexture, start, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        public static void DrawRectangleOutline(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color, 1);
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Left, rectangle.Bottom), color, 1);
            spriteBatch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), color, 1);
            spriteBatch.DrawLine(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color, 1);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(G.PixelTexture, rectangle, color);
        }

        public static void DrawRectangleOutline(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Vector2 borderSize, Color color)
        {
            DrawRectangle(spriteBatch, position, new Vector2(size.X, borderSize.Y), color); //top
            DrawRectangle(spriteBatch, position, new Vector2(borderSize.X, size.Y), color); //left
            DrawRectangle(spriteBatch, new Vector2(position.X + size.X - borderSize.X, position.Y), new Vector2(borderSize.X, size.Y), color); //right
            DrawRectangle(spriteBatch, new Vector2(position.X, position.Y + size.Y - borderSize.Y), new Vector2(size.Y, borderSize.Y), color); //bottom
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
        {
            spriteBatch.Draw(G.PixelTexture, position, null, color, 0, Vector2.Zero, size, SpriteEffects.None, 0); 
        }

        public static void DrawPixel(this SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(G.PixelTexture, position, color);
        }

    }
}
