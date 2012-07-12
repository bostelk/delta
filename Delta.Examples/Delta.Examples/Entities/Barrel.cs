using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Collision;
using Delta.Collision.Geometry;

namespace Delta.Examples.Entities
{
    public class Barrel : CollideableEntity
    {
        SpriteEntity _sprite;

        public Barrel()
        {
            _sprite = new SpriteEntity(@"Graphics\SpriteSheets\16x16");
            _sprite.Origin = new Vector2(0.5f, 0.5f);
            _sprite.Play("barrel");
            Collider = new Collider(this, new OBB(16, 16));
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            _sprite.InternalUpdate(gameTime);
            _sprite.Position = Position;
            base.LightUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sprite.InternalDraw(gameTime, spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }

        public override bool OnCollision(Collider them, Vector2 normal)
        {
            BoxLink link = them.Tag as BoxLink;
            if (link != null && link.Velocity.LengthSquared() > MathExtensions.Square(50))
            {
                Explode();
                RemoveNextUpdate = true;
                G.Collision.RemoveColider(Collider);
            }
            return base.OnCollision(them, normal);
        }

        public void Explode()
        {
            G.Audio.PlaySound("SFX_LargeExplosion", true);
            Visuals.Create(@"Graphics\SpriteSheets\16x16", "explode", Position);
            Visuals.CreateShatter(@"Graphics\SpriteSheets\16x16", "barrelDebris", Position, 13);
            G.World.Camera.Shake(10, 0.5f, ShakeAxis.X | ShakeAxis.Y);
        }
    }
}
