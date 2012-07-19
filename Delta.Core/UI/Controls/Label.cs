using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI.Controls
{
    [Flags]
    public enum HorizontalTextAlignment
    {
        Left = 0x0,
        Right = 0x1,
        Center = 0x2
    }

    [Flags]
    public enum VerticalTextAlignment
    {
        Top = 0x0,
        Bottom = 0x1,
        Center = 0x2
    }

    public class Label : Control
    {
        Vector2 _textPosition = Vector2.Zero;
        Vector2 _textSize = Vector2.Zero;

        public StringBuilder Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color ForeColor { get; set; }
        public bool AutoSize { get; set; }
        public HorizontalTextAlignment HorizontalTextAlignment { get; set; }
        public VerticalTextAlignment VerticalTextAlignment { get; set; }

        public Label()
            : base()
        {
            AutoSize = true;
            Text = new StringBuilder();
            ForeColor = Color.White;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Font = G.Font;
        }

        protected override void UpdateRenderSize()
        {
            if (AutoSize)
                RenderSize = _textSize;
            else
                base.UpdateRenderSize();
        }

        protected virtual void UpdateTextSize()
        {
            _textSize = Font.MeasureString(Text);
            if (AutoSize)
                Size = _textSize;
        }

        protected virtual void UpdateTextPosition()
        {
            _textPosition = Position;
            if (!AutoSize)
            {
                //horizontal alignment
                _textPosition.X = 0;
                if (Size.X >= _textSize.X)
                {
                    if ((HorizontalTextAlignment & HorizontalTextAlignment.Center) != 0)
                        _textPosition.X += (Size.X * 0.5f) - (_textSize.X * 0.5f);
                    else if ((HorizontalTextAlignment & HorizontalTextAlignment.Right) != 0)
                        _textPosition.X += Size.X - _textSize.X;
                }
                //vertical alignment
                _textPosition.Y = 0;
                if (Size.Y >= _textSize.Y)
                {
                    if ((VerticalTextAlignment & VerticalTextAlignment.Center) != 0)
                        _textPosition.Y += (Size.Y * 0.5f) - (_textSize.Y * 0.5f);
                    else if ((VerticalTextAlignment & VerticalTextAlignment.Bottom) != 0)
                        _textPosition.Y += Size.Y - _textSize.Y;
                }
            }
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            spriteBatch.DrawString(Font, Text, _textPosition, ForeColor, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }
    }
}
