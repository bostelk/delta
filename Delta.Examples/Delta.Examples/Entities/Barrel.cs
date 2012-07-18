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
            _sprite = SpriteEntity.Create(@"Graphics\SpriteSheets\16x16");
            _sprite.Origin = new Vector2(0.5f, 0.5f);
            _sprite.Play("barrel");
            Polygon = new OBB(16, 16);
        }

        protected override void LightUpdate(DeltaTime time)
        {
            _sprite.InternalUpdate(time);
            _sprite.Position = Position;
            Collider.Geom.Position = Position;
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            _sprite.InternalDraw(time, spriteBatch);
            base.Draw(time, spriteBatch);
        }

        protected override bool OnCollision(Collider them, Vector2 normal)
        {
            Lily link = them.Tag as Lily;
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
