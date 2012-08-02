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
    public class GameHud : TransformableEntity
    {
        Texture2D _healthBar;
        Texture2D _ammoBar;

        Vector2 _leftPosition;
        Vector2 _rightPosition;

        float _leftFillPercent;
        public float LeftFillPercent {
            get
            {
                return _leftFillPercent;
            }
            set
            {
                _leftFillPercent = MathHelper.Clamp(value, 0, 1);
            }
        }

        float _rightFillPercent;
        public float RightFillPercent
        {
            get
            {
                return _rightFillPercent;
            }
            set
            {
                _rightFillPercent = MathHelper.Clamp(value, 0, 1);
            }
        }

        public Color LeftBarColor;
        public Color RightBarColor;

        public GameHud() : base()
        {
            Name = "GameHud";
            RightFillPercent = 0.0f;
            LeftFillPercent = 0.0f;
            RightBarColor = new Color(56, 136, 216).SetAlpha(0.95f);
            LeftBarColor = new Color(208, 48, 208).SetAlpha(0.95f);
        }

        protected override void LoadContent()
        {
            _healthBar = G.Content.Load<Texture2D>(@"Graphics\HealthBar");
            _ammoBar = G.Content.Load<Texture2D>(@"Graphics\AmmoBar");
            //_leftPosition = new Vector2(G.UI.Camera.ViewingArea.Width * 0.02f, G.UI.Camera.ViewingArea.Height * 0.55f);
            //_rightPosition = new Vector2(G.UI.Camera.ViewingArea.Width * 0.93f, G.UI.Camera.ViewingArea.Height * 0.55f);
            base.LoadContent();
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangleOutline(new Rectangle((int)_leftPosition.X + 5, (int)(_leftPosition.Y + 2 + (1 - _leftFillPercent) * 60.0f), 8, (int)(60.0f * _leftFillPercent)), LeftBarColor.SetAlpha(Alpha), true);
            //spriteBatch.DrawRectangleOutline(new Rectangle((int)_rightPosition.X + 5, (int)(_rightPosition.Y + 2 + (1 - _rightFillPercent) * 60.0f), 8, (int)(60.0f * _rightFillPercent)), RightBarColor.SetAlpha(Alpha), true);
            spriteBatch.Draw(_healthBar, _leftPosition, Color.White.SetAlpha(Alpha));
            spriteBatch.Draw(_ammoBar, _rightPosition, Color.White.SetAlpha(Alpha));
            spriteBatch.DrawString(G.Font, (int)(_leftFillPercent * 100.0f) + "%", new Vector2(_leftPosition.X + 10, _leftPosition.Y + 2 + 30), Color.White.SetAlpha(Alpha), TextAlignment.Center);
            spriteBatch.DrawString(G.Font, (int)(_rightFillPercent * 100.0f) + "%", new Vector2(_rightPosition.X + 10, _rightPosition.Y + 2 + 30), Color.White.SetAlpha(Alpha), TextAlignment.Center);
        }
        
    }
}
