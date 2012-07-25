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

namespace Delta.Examples.Entities
{
    public class Lily : Entity
    {
        const float SPEED = 30;

        SpriteEntity _sprite;

        public Vector2 Input { get; set; }
        public Vector2 Velocity;

        float _trailInterval = 0.09f;
        float _trailTime = 0;

        LilySpriteController _spriteController;

        public Lily() : base("Lily")
        {
            _sprite = SpriteEntity.Create(@"Graphics\SpriteSheets\16x16");
            _sprite.Origin = new Vector2(0.5f, 0.5f);
            _sprite.Play("walkdown");

            _spriteController = new LilySpriteController(_sprite);
        }

        protected override void LateInitialize()
        {
            WrappedBody = Collider.CreateBody(new Box(16, 16));
            base.LateInitialize();
        }

        public void SwitchBody()
        {
            if (WrappedBody.Shape is Circle)
            {
                WrappedBody.Shape= new Box(16, 16);
            }
            else if (WrappedBody.Shape is Box)
            {
                WrappedBody.Shape = new Circle(8);
            }
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (G.Input.Keyboard.IsDown(Keys.Q))
                Rotation = (Rotation + (0.5f));
            else if (G.Input.Keyboard.IsDown(Keys.E))
                Rotation = (Rotation - (0.5f));
            Rotation = Rotation.Wrap(0, 360);

            Vector2 direction = Vector2.Zero;
            if (G.Input.Keyboard.IsDown(Keys.Left))
            {
                direction -= Vector2.UnitX;
            }
            if (G.Input.Keyboard.IsDown(Keys.Right))
            {
                direction += Vector2.UnitX;
            }
            if (G.Input.Keyboard.IsDown(Keys.Up))
            {
                direction -= Vector2.UnitY;
            }
            if (G.Input.Keyboard.IsDown(Keys.Down))
            {
                direction += Vector2.UnitY;
            }
            Vector2Extensions.SafeNormalize(ref direction);
            _spriteController.Walk(direction);

            Velocity = ((G.Input.Keyboard.IsDown(Keys.LeftShift)) ? direction * 2.5f : direction) * SPEED;
            Position += Velocity * (float)time.ElapsedSeconds;

            if (G.Input.Keyboard.IsPressed(Keys.Tab))
                SwitchBody();

            // if boosting leave a motion trail
            //if (G.World.SecondsPast(_trailTime + _trailInterval) && G.Input.Keyboard.IsDown(Keys.LeftShift))
            //{
            //    Visuals.CreateTrail(@"Graphics\SpriteSheets\16x16", _spriteController.ToString(), Position + (direction * -2 * time.ElapsedSeconds));
            //    _trailTime = (float)time.TotalSeconds;
            //}

            _sprite.Position = Position;
            _sprite.InternalUpdate(time);
            Layer = Position.Y;

            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            _sprite.InternalDraw(time, spriteBatch);
        }
    }
}
