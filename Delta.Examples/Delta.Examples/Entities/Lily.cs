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
        const float SPEED = 50;

        SpriteEntity _sprite;

        public Vector2 Input { get; set; }
        public Vector2 Velocity;

        float _trailInterval = 0.09f;
        float _trailTime = 0;

        Collider collider;

        public Lily() : base("Lily")
        {
            _sprite = SpriteEntity.Create(@"Graphics\SpriteSheets\16x16");
            _sprite.Origin = new Vector2(0.5f, 0.5f);
            _sprite.Play("blackspike");
        }

        protected override void LateInitialize()
        {
            G.Collision.AddCollider(collider = Collider.Create(this, new Box(16, 16)));           
            base.LateInitialize();
        }

        //public void SwitchBody()
        //{
        //    if (Polygon is Circle)
        //    {
        //        Polygon = new OBB(16, 16);
        //    }
        //    else if (Polygon is OBB)
        //    {
        //        Polygon = new Circle(8);
        //    }
        //}

        protected override void LightUpdate(DeltaTime time)
        {
            Vector2 direction = G.Input.ArrowDirection;
            Vector2Extensions.SafeNormalize(ref direction);
            Velocity = ((G.Input.Keyboard.IsDown(Keys.LeftShift)) ? direction * 2.5f : direction) * SPEED;
            Position += Velocity * (float)time.ElapsedSeconds;

            // if boosting leave a motion trail
            if (G.World.SecondsPast(_trailTime + _trailInterval) && G.Input.Keyboard.IsDown(Keys.LeftShift))
            {
                Visuals.CreateTrail(@"Graphics\SpriteSheets\16x16", "blackspike", Position + (direction * -2 * time.ElapsedSeconds));
                _trailTime = (float)time.TotalSeconds;
            }

            _sprite.Position = Position;
            _sprite.InternalUpdate(time);
            Layer = Position.Y;

            collider.Position = Position;
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            _sprite.InternalDraw(time, spriteBatch);
        }
    }
}
