using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Delta;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Delta.Examples.Entities
{
    public class ProgressBar : TransformableEntity
    {
        public enum Mode
        {
            None,
            Drain,
            Charge,
        }

        public const float FULL = 1.0f;
        public const float EMPTY = 0.0f;

        Rectangle _bar;

        public Mode BarMode;
        public Color FillColor;
        public float RechargeRate;

        float _fillPercent;
        /// <summary>
        /// The percent of the bar that is FULL.
        /// </summary>
        public float FillPercent {
            get
            {
                return _fillPercent;
            }
            set
            {
                _fillPercent = MathHelper.Clamp(value, 0, 1);
                if (IsFull && OnFull != null)
                    OnFull();
                if (IsEmpty && OnEmpty != null)
                    OnEmpty();
            }
        }

        public bool AllowRecharge
        {
            get;
            set;
        }

        public bool IsHorizontal
        {
            get
            {
                return _bar.Width > _bar.Height;
            }
        }

        public bool IsVerical
        {
            get
            {
                return _bar.Width < _bar.Height;
            }
        }

        public bool DrawPercentage
        {
            get;
            set;
        }

        public SpriteFont Font
        {
            get;
            set;
        }

        public float Width
        {
            get { return _bar.Width; }
            set { _bar.Width = (int)Math.Abs(value); }
        }

        public float Height
        {
            get { return _bar.Height; }
            set { _bar.Height = (int)Math.Abs(value); }
        }

        public Action OnEmpty
        {
            get;
            set;
        }

        public Action OnFull
        {
            get;
            set;
        }

        //public override Vector2 Position
        //{
        //    get
        //    {
        //        return _bar.Center.ToVector2();
        //    }
        //    set
        //    {
        //        _bar.X = (int)value.X + _bar.Width / 2;
        //        _bar.Y = (int)value.Y + _bar.Height / 2;
        //    }
        //}

        public bool IsFull
        {
            get
            {
                return FillPercent == FULL;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return FillPercent == EMPTY;
            }
        }

        public ProgressBar()
        {
            //ID = "ProgressBar";
            FillPercent = 1.0f;
            RechargeRate = 0.01f;
            FillColor = Color.YellowGreen;
        }

        //public override void LoadContent()
        //{
        //    Position = new Vector2(G.HUD.Camera.ViewingArea.Width * 0.02f, G.HUD.Camera.ViewingArea.Height * 0.55f);
        //    base.LoadContent();
        //}

        //protected override void LightUpdate(GameTime gameTime)
        //{
        //    if (AllowRecharge && !IsFull)
        //    {
        //        FillPercent = MathHelper.Lerp(FillPercent, 1.0f, RechargeRate);
        //        if (IsFull && OnFull != null)
        //            OnFull();
        //    }
        //    base.LightUpdate(gameTime);
        //}

        //protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    spriteBatch.DrawRectangle(new Rectangle((int)Position.X + 5, (int)(Position.Y + 2 + (1 - _fillPercent) * 60.0f), 8, (int) (60.0f * _fillPercent)), FillColor, true);
        //    if (Font != null && DrawPercentage)
        //        spriteBatch.DrawString(G.Font, (int)(_fillPercent * 100.0f) + "%", new Vector2(Position.X - 10, Position.Y + 2 + 30), Color.White);
        //}
        
    }
}
