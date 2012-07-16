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
    public class BoxLink : Entity  //CollideableEntity
    {
        const float SPEED = 50;

        SpriteEntity _sprite;

        public Vector2 Input { get; set; }
        public Vector2 Velocity;

        float _trailInterval = 0.09f;
        float _trailTime = 0;

        Texture2D _texture;
        public BoxLink()
        {
            //_sprite = new SpriteEntity(@"Graphics\SpriteSheets\16x16");
            //_sprite.Origin = new Vector2(0.5f, 0.5f);
            //_sprite.Play("blackspike");

            //Collider = new Collider()
            //{
            //    Tag = this,
            //    Mass = 1f,
            //    Geom = new Circle(8)
            //};
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

        public override void LoadContent()
        {
            _texture = G.Content.Load<Texture2D>(@"Graphics\AnimatedMisc\spike");
            base.LoadContent();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            Vector2 direction = G.Input.Mouse.Held(Delta.Input.MouseButton.Left) ? G.World.Camera.ToWorldPosition(G.Input.Mouse.Position) - Position : Vector2.Zero;
            direction = G.Input.ArrowDirection;
            Vector2Extensions.SafeNormalize(ref direction);
            Velocity = ((G.Input.Keyboard.Held(Keys.LeftShift)) ? direction * 2.5f : direction) * SPEED;
            Position += Velocity * (float)time.ElapsedSeconds;

            // if boosting leave a motion trail
            //if (G.World.SecondsPast(_trailTime + _trailInterval) && G.Input.Keyboard.Held(Keys.LeftShift))
            //{
            //    Visuals.CreateTrail(@"Graphics\SpriteSheets\16x16", "blackspike", Position + Velocity * (float)time.ElapsedSeconds);
            //    _trailTime = (float)time.TotalSeconds;
            //}

            //_sprite.Position = Position;
            //_sprite.InternalUpdate(time);
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Tint, Rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), Scale, SpriteEffects.None, 0);
        }
    }
}
