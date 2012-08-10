using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Delta.Transformations;
using System.Globalization;
using System.ComponentModel;

namespace Delta.Graphics
{
    public class PixelEmitter : Emitter
    {

        internal class PixelEntity : TransformableEntity 
        {
            public static PixelEntity Create()
            {
                return Pool.Fetch<PixelEntity>();
            }
        }

        internal class PixelParticle : Particle<PixelEntity>
        {
            public override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(G.PixelTexture, Entity.Position, null , Entity.Tint, Entity.Rotation, Entity.Origin * Entity.Size, Entity.Scale, SpriteEffects.None, 0);
                base.Draw(time, spriteBatch);
            }
        }
        
        List<PixelParticle> _particles;
        float _lastEmitTime;

        [ContentSerializer, DisplayName("Colors"),Description(""), Category("Pixel Emitter"), Browsable(true), ReadOnly(false), DefaultValue(false)]
        public List<Color> Colors { get; set; }

        public static PixelEmitter Create()
        {
            PixelEmitter emitter = Pool.Fetch<PixelEmitter>();
            G.World.AboveGround.UnsafeAdd(emitter);
            return emitter;
        }

        static PixelParticle CreateParticle()
        {
            PixelParticle particle = Pool.Fetch<PixelParticle>();
            return particle;
        }

        public PixelEmitter()
        {
            _particles = new List<PixelParticle>(100);
            Colors = new List<Color>();
        }

        protected override void Recycle(bool isReleasing)
        {
            _lastEmitTime = 0;
            if (isReleasing)
            {
                for (int i = 0; i < _particles.Count; i++)
                {
                    PixelParticle particle = _particles[i];
                    particle.Recycle();
                }
            }
            base.Recycle(isReleasing);
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
            PixelParticle newParticle = Pool.Fetch<PixelParticle>();
            newParticle.Emitter = this;
            newParticle.Lifespan = _lifespanRange.RandomWithin();
            newParticle.Acceleration = Vector2Extensions.DirectionBetween(_accelerationAngleRange.Lower.ToRadians(), _accelerationAngleRange.Upper.ToRadians()) * _accelerationMagnitudeRange.RandomWithin();
            newParticle.AngularVelocity = _rotationRange.RandomWithin();
            newParticle.Velocity = Vector2Extensions.DirectionBetween(_velocityAngleRange.Lower.ToRadians(), _velocityAngleRange.Upper.ToRadians()) * _velocityMagnitudeRange.RandomWithin();
            newParticle.FadeInPercent = _fadeInRange.RandomWithin();
            newParticle.FadeOutPercent = _fadeOutRange.RandomWithin();
            newParticle.Entity = PixelEntity.Create();
            newParticle.Entity.Tint = Colors[G.Random.Next(Colors.Count)];
            newParticle.Entity.Scale = G.Random.Between(new Vector2(_scaleRange.Lower), new Vector2(_scaleRange.Upper));
            newParticle.Entity.Origin = Vector2.One * 0.5f;
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

        protected internal override void OnRemoved()
        {
            Recycle();
            base.OnRemoved();
        }

    }
}