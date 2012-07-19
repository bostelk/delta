using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Structures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Delta.Movement;
using System.Globalization;
using Delta.Entities;

namespace Delta.Graphics
{
    public class PixelEmitter : Emitter
    {

        internal class PixelParticle : Particle<TextureEntity>
        {
            public override void Recycle()
            {
                base.Recycle();

                _particlePool.Release(this);
            }

            public override void OnEmitted()
            {
                Entity.Alpha = 0;
                Transformer.ThisEntity(Entity).FadeTo(1, Life / 2, Interpolation.EaseInCubic);
                base.OnEmitted();
            }
        }

        static Pool<PixelEmitter> _pool;
        static Pool<PixelParticle> _particlePool;
        
        List<PixelParticle> _particles;
        
        float _lastEmitTime;

        public float Frequency;
        public Color Color;
        public float MaxLifespan;
        public float MinLifespan;
        public float MinSpeed;
        public float MaxSpeed;
        public float MinAcceleration;
        public float MaxAcceleration;
        public float MinRotation;
        public float MaxRotation;
        public float MinAngle;
        public float MaxAngle;
        public float MinScale;
        public float MaxScale;
        public float MinFrameInterval;
        public float MaxFrameInterval;
        public bool Explode;
        public int ExplodeQuantity;

        static PixelEmitter()
        {
            _pool = new Pool<PixelEmitter>(200);
            _particlePool = new Pool<PixelParticle>(2000);
        }

        public static PixelEmitter Create()
        {
            PixelEmitter emitter = _pool.Fetch();
            G.World.AboveGround.Add(emitter);
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
            MinAngle = 0;
            MaxAngle = 360;
            MinScale = 1;
            MaxScale = 1;
        }

        protected internal override bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "color":
                    Color = value.ToColor();
                    return true;
                case "frequency":
                    Frequency = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "speed":
                    Range speedRange = Range.Parse(value);
                    MinSpeed = speedRange.Lower;
                    MaxSpeed = speedRange.Upper;
                    return true;
                case "lifespan":
                    Range lifespanRange = Range.Parse(value);
                    MinLifespan = lifespanRange.Lower;
                    MaxLifespan = lifespanRange.Upper;
                    return true;
                case "rotation":
                    Range rotationRange = Range.Parse(value);
                    MinRotation = rotationRange.Lower.ToRadians();
                    MaxRotation = rotationRange.Upper.ToRadians();
                    return true;
                case "angle":
                    Range angleRange = Range.Parse(value);
                    MinAngle = angleRange.Lower.ToRadians();
                    MaxAngle = angleRange.Upper.ToRadians();
                    return true;
                case "scale":
                    Range scaleRange = Range.Parse(value);
                    MinScale = scaleRange.Lower;
                    MaxScale = scaleRange.Upper;
                    return true;
                case "acceleration":
                    Range accelerationRange = Range.Parse(value);
                    MinAcceleration = accelerationRange.Lower;
                    MaxAcceleration = accelerationRange.Upper;
                    return true;
                case "frameinterval":
                    Range frameIntervalRange = Range.Parse(value);
                    MinFrameInterval = frameIntervalRange.Lower;
                    MaxFrameInterval = frameIntervalRange.Upper;
                    return true;
                case "explode":
                    Explode = true;
                    ExplodeQuantity = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }

        public void Emit()
        {
            PixelParticle newParticle = _particlePool.Fetch();
            newParticle.Entity = TextureEntity.Create();
            newParticle.Life = G.Random.Between(MinLifespan, MaxLifespan);
            newParticle.AngularVelocity = G.Random.Between(MinRotation, MaxRotation);
            newParticle.Velocity = -Vector2Extensions.DirectionBetween(MinAngle, MaxAngle) * G.Random.Between(MinSpeed, MaxSpeed);
            newParticle.Entity.Tint = Color;
            newParticle.Entity.Scale = G.Random.Between(new Vector2(MinScale), new Vector2(MaxScale));
            newParticle.Entity.Origin = new Vector2(0.5f, 0.5f);
            newParticle.Entity.Position = G.Random.Between(Position, Position + Size); // tiled gives up the position as top-let
            newParticle.Entity.InternalLoadContent(); // otherwise the sprite will not play because the spritessheet has not been loaded.
            newParticle.OnEmitted();
            _particles.Add(newParticle);
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (G.World.SecondsPast(_lastEmitTime + Frequency))
            {
                if (Explode)
                    for (int i = 0; i < ExplodeQuantity; i++)
                        Emit();
                else
                    Emit();
                _lastEmitTime = time.TotalSeconds;
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                PixelParticle particle = _particles[i];
                particle.Entity.InternalUpdate(time);
                particle.Life -= time.ElapsedSeconds;
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

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                PixelParticle particle = _particles[i];
                particle.Entity.InternalDraw(time, spriteBatch);
            }
            base.Draw(time, spriteBatch);
        }

        protected internal override void OnRemoved()
        {
            Recycle();
            base.OnRemoved();
        }

        public override void Recycle()
        {
            base.Recycle();
            _lastEmitTime = 0;
            Frequency = 0;
            MaxLifespan = 0;
            MinLifespan = 0;
            MinSpeed = 0;
            MaxSpeed = 0;
            MinRotation = 0;
            MaxRotation = 0;
            MinAngle = 0;
            MaxAngle = 360;
            MinScale = 1;
            MaxScale = 1;
            Explode = false;
            ExplodeQuantity = 0;
            for (int i = 0; i < _particles.Count; i++)
            {
                PixelParticle particle = _particles[i];
                particle.Recycle();
            }

            _pool.Release(this);
        }

    }
}
