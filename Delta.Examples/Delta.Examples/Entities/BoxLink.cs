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
using Delta.Collision;
using Delta.Collision.Geometry;

namespace Delta.Examples.Entities
{
    public class BoxLink : CollideableEntity
    {
        const float SPEED = 50;

        SpriteEntity _sprite;

        public Vector2 Input { get; set; }
        public Vector2 Velocity;

        float _trailInterval = 0.09f;
        float _trailTime = 0;

        public BoxLink()
        {
            _sprite = new SpriteEntity(@"Graphics\SpriteSheets\16x16");
            _sprite.Origin = new Vector2(0.5f, 0.5f);
            _sprite.Play("blackspike");

            Collider = new Collider()
            {
                Tag = this,
                Mass = 1f,
                Geom = new Circle(8)
            };
        }

        public void SwitchBody()
        {
            if (Polygon is Circle)
            {
                Polygon = new OBB(16, 16);
            }
            else if (Polygon is OBB)
            {
                Polygon = new Circle(8);
            }
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            Velocity = Input * SPEED;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // if boosting leave a motion trail
            if (G.World.SecondsPast(_trailTime + _trailInterval) && G.Input.Keyboard.Held(Keys.LeftShift))
            {
                Visuals.CreateTrail(@"Graphics\SpriteSheets\16x16", "blackspike", Position + Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                _trailTime = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            _sprite.Position = Position;
            _sprite.InternalUpdate(gameTime);
            base.LightUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            _sprite.InternalDraw(gameTime, spriteBatch);
        }
    }
}
