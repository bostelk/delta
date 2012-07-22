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
        Vector2 _textOrigin = Vector2.Zero;
        StringBuilder _renderText = new StringBuilder();

        bool _isWordWrapped = false;
        public bool IsWordWrapped
        {
            get { return _isWordWrapped; }
            set 
            {
                if (_isWordWrapped != value)
                {
                    _isWordWrapped = value;
                    Invalidate();
                }
            }
        }

        bool _autoSize = true;
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (_autoSize != value)
                {
                    _autoSize = value;
                    Invalidate();
                }
            }
        }

        public StringBuilder Text { get; private set; }
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

        protected virtual void UpdateRenderText()
        {
            _renderText.Clear();
            if (IsWordWrapped)
                Text.WordWrap(ref _renderText, Font, Size, Vector2.One);
            else
                for (int i = 0; i < Text.Length; i++)
                    _renderText.Append(Text[i]);
        }

        protected virtual void UpdateTextSize()
        {
            _textSize = Font.MeasureString(_renderText);
        }

        protected override void UpdateRenderSize()
        {
            if (AutoSize)
                RenderSize = _textSize;
            else
                base.UpdateRenderSize();
        }

        protected virtual void UpdateTextPosition()
        {
            _textPosition = Position;
            _textOrigin = Vector2.Zero;
            if (!AutoSize)
            {
                //horizontal alignment
                if (Size.X >= _textSize.X)
                {
                    if ((HorizontalTextAlignment & HorizontalTextAlignment.Center) != 0)
                        _textPosition.X += (Size.X * 0.5f) - (_textSize.X * 0.5f);
                    else if ((HorizontalTextAlignment & HorizontalTextAlignment.Right) != 0)
                        _textPosition.X += Size.X - _textSize.X;
                }
                //vertical alignment
                if (Size.Y >= _textSize.Y)
                {
                    if ((VerticalTextAlignment & VerticalTextAlignment.Center) != 0)
                        _textPosition.Y += (Size.Y * 0.5f) - (_textSize.Y * 0.5f);
                    else if ((VerticalTextAlignment & VerticalTextAlignment.Bottom) != 0)
                        _textPosition.Y += Size.Y - _textSize.Y;
                }
            }
        }

        protected internal override void OnInvalidate()
        {
            UpdateRenderText();
            UpdateTextSize();
            UpdateRenderSize();
            UpdateTextPosition();
            base.OnInvalidate();
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            spriteBatch.DrawString(Font, _renderText, _textPosition, ForeColor, 0, _textOrigin, Vector2.One, SpriteEffects.None, 0);
        }

        public override string ToString()
        {
            return string.Format("{0}, Text: {1}", base.ToString(), _renderText);
        }
    }
}
