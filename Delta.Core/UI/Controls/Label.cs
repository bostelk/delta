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
        public HorizontalTextAlignment HorizontalTextAlignment { get; set; }
        public VerticalTextAlignment VerticalTextAlignment { get; set; }

        public Label()
            : base()
        {
            Text = new StringBuilder();
            ForeColor = Color.White;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Font = G.Font;
        }

        protected virtual void UpdateTextSize()
        {
            _textSize = Font.MeasureString(Text);
        }

        public void UpdateTextPosition()
        {
            //horizontal alignment
            if ((HorizontalTextAlignment & HorizontalTextAlignment.Left) != 0)
                _textPosition.X = Position.X;
            else if ((HorizontalTextAlignment & HorizontalTextAlignment.Center) != 0)
                _textPosition.X = Position.X + -_textSize.X * 0.5f;
            else if ((HorizontalTextAlignment & HorizontalTextAlignment.Right) != 0)
                _textPosition.X = Position.X + -_textSize.X;
            //vertical alignment
            if ((VerticalTextAlignment & VerticalTextAlignment.Top) != 0)
                _textPosition.Y = Position.Y;
            else if ((VerticalTextAlignment & VerticalTextAlignment.Center) != 0)
                _textPosition.X = Position.Y + -_textSize.Y * 0.5f;
            else if ((VerticalTextAlignment & VerticalTextAlignment.Bottom) != 0)
                _textPosition.X = Position.Y + -_textSize.Y;
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, _textPosition, ForeColor);
        }
    }
}
