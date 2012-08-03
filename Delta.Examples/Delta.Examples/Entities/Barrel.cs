using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Collision;

namespace Delta.Examples.Entities
{
    public class Barrel : TransformableEntity
    {
        AnimatedSpriteEntity _sprite;

        public Barrel()
        {
            _sprite = AnimatedSpriteEntity.Create(@"Graphics\SpriteSheets\16x16");
            _sprite.Play("barrel");
        }

        protected override void Initialize()
        {
            WrappedBody = CollisionBody.CreateBody(new Box(16, 16));
            WrappedBody.OnCollisionEvent += OnCollision;
            WrappedBody.SetGroup(CollisionGroups.Group2);
            base.Initialize();
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            _sprite.InternalUpdate(time);
            _sprite.Position = Position - (_sprite.Size * new Vector2(0.5f, 0.5f));
            Depth = Position.Y;
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            _sprite.InternalDraw(time, spriteBatch);
            base.Draw(time, spriteBatch);
        }

        protected bool OnCollision(IWrappedBody them, Vector2 normal)
        {
            Lily link = them.Owner as Lily;
            if (link != null)
            {
                Explode();
                RemoveNextFrame();
            }
            return true;
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
