using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Structures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Delta.Transformations;
using System.Globalization;
using Delta.Graphics;

namespace Delta.Graphics
{
    public class PixelEmitter : Emitter
    {

        internal class PixelEntity : TransformableEntity 
        {
            static Pool<PixelEntity> _pool;

            static PixelEntity()
            {
                _pool = new Pool<PixelEntity>(100);
            }

            public static PixelEntity Create()
            {
                return _pool.Fetch();
            }

            public override void Recycle()
            {
                base.Recycle();
                _pool.Release(this);
            }
        }

        internal class PixelParticle : Particle<PixelEntity>
        {
            public override void Recycle()
            {
                base.Recycle();
                _particlePool.Release(this);
            }

            public override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(G.PixelTexture, Entity.Position, null , Entity.Tint, Entity.Rotation, Entity.Origin * Entity.Size, Entity.Scale, SpriteEffects.None, 0);
                base.Draw(time, spriteBatch);
            }
        }

        static Pool<PixelEmitter> _pool;
        static Pool<PixelParticle> _particlePool;
        
        List<PixelParticle> _particles;
        
        float _lastEmitTime;

        public List<Color> Colors;

        static PixelEmitter()
        {
            _pool = new Pool<PixelEmitter>(200);
            _particlePool = new Pool<PixelParticle>(2000);
        }

        public static PixelEmitter Create()
        {
            PixelEmitter emitter = _pool.Fetch();
            G.World.AboveGround.UnsafeAdd(emitter);
            return emitter;
        }

        static PixelParticle CreateParticle()
        {
            PixelParticle particle = _particlePool.Fetch();
            return particle;
        }

        public PixelEmitter()
        {
            _particles = new List<PixelParticle>(100);
            Colors = new List<Color>();
        }

        protected internal override bool SetValue(string name, string value)
        {
            switch (name)
            {
                case "color":
                case "colors":
                    string[] split = value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string item in split) {
                        Colors.Add(item.ToColor());
                    }
                    return true;
            }
            return base.SetValue(name, value);
        }

        public void Emit()
        {
            PixelParticle newParticle = _particlePool.Fetch();
            newParticle.Emitter = this;
            newParticle.Lifespan = _lifespanRange.RandomWithin();
            newParticle.Acceleration = Vector2Extensions.DirectionBetween(_accelerationAngleRange.Lower, _accelerationAngleRange.Upper) * _accelerationMagnitudeRange.RandomWithin();
            newParticle.AngularVelocity = _rotationRange.RandomWithin();
            newParticle.Velocity = Vector2Extensions.DirectionBetween(_velocityAngleRange.Lower, _velocityAngleRange.Upper) * _velocityMagnitudeRange.RandomWithin();
            newParticle.FadeInPercent = _fadeInRange.RandomWithin();
            newParticle.FadeOutPercent = _fadeOutRange.RandomWithin();
            newParticle.Entity = PixelEntity.Create();
            newParticle.Entity.Tint = Colors[G.Random.Next(Colors.Count)];
            newParticle.Entity.Scale = G.Random.Between(new Vector2(_scaleRange.Lower), new Vector2(_scaleRange.Upper));
            newParticle.Entity.Origin = new Vector2(0.5f, 0.5f);
            newParticle.Entity.Position = G.Random.Between(Position, Position + Size); // tiled gives up the position as top-let
            newParticle.OnEmitted();
            _particles.Add(newParticle);
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            if (G.World.SecondsPast(_lastEmitTime + _frequency))
            {
                int quantity = (int) (_quantityRange.RandomWithin() + .5);
                for (int i = 0; i < quantity; i++)
                    Emit();
                _lastEmitTime = time.TotalSeconds;
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                PixelParticle particle = _particles[i];
                particle.Update(time);
                particle.Life += time.ElapsedSeconds;
                particle.Velocity += particle.Acceleration * time.ElapsedSeconds;
                particle.Entity.Position += particle.Velocity * time.ElapsedSeconds;
                particle.Entity.Rotation += particle.AngularVelocity * time.ElapsedSeconds;

                // re-asign to keep state changes; eg. lifespan.
                _particles[i] = particle; 

                if (particle.IsDead)
                {
                    particle.Recycle();
                    _particles.FastRemove<PixelParticle>(i);
                    i--;
                }
            }
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, Blend, SamplerState.PointClamp, null, null, null, G.World.Camera.View);
            for (int i = 0; i < _particles.Count; i++)
            {
                PixelParticle particle = _particles[i];
                particle.Draw(time, spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, G.World.Camera.View);
            base.Draw(time, spriteBatch);
        }

        public override void Recycle()
        {
            base.Recycle();
            _lastEmitTime = 0;

            for (int i = 0; i < _particles.Count; i++)
            {
                PixelParticle particle = _particles[i];
                particle.Recycle();
            }

            _pool.Release(this);
        }

        protected internal override void OnRemoved()
        {
            Recycle();
            base.OnRemoved();
        }

    }
}