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

namespace Delta.Graphics
{
    public class SpriteEmitter : Emitter
    {

        //internal class PixelParticle : Particle<Texture2D>
        //{
        //    public override void Recycle()
        //    {
        //        base.Recycle();

        //        //_particlePool.Release(this);
        //    }

        //    public override void OnEmitted()
        //    {
        //        //Transformer.ThisEntity(Entity).FadeTo(0, Lifespan);
        //        base.OnEmitted();
        //    }
        //}

        internal class SpriteParticle : Particle<SpriteEntity>
        {
            public override void Recycle()
            {
                base.Recycle();

                _particlePool.Release(this);
            }

            public override void OnEmitted()
            {
                Transformer.ThisEntity(Entity).FadeTo(0, Lifespan, Interpolation.EaseInCubic);
                base.OnEmitted();
            }
        }

        static Pool<SpriteEmitter> _pool;
        static Pool<SpriteParticle> _particlePool;
        
        List<SpriteParticle> _particles;
        
        [ContentSerializer]
        string _spriteSheet;
        [ContentSerializer]
        string _animationName;
        float _lastEmitTime;

        public float Frequency;
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
        public bool Explode;
        public int ExplodeQuantity;

        static SpriteEmitter()
        {
            _pool = new Pool<SpriteEmitter>(200);
            _particlePool = new Pool<SpriteParticle>(2000);
        }

        public static SpriteEmitter Create(string spriteSheet, string animationName)
        {
            SpriteEmitter emitter = _pool.Fetch();
            emitter._spriteSheet = spriteSheet;
            emitter._animationName = animationName;
            G.World.AboveGround.Add(emitter);
            return emitter;
        }

        static SpriteParticle CreateParticle()
        {
            SpriteParticle particle = _particlePool.Fetch();
            return particle;
        }

        public SpriteEmitter()
        {
            _particles = new List<SpriteParticle>(100);
            MinAngle = 0;
            MaxAngle = 360;
        }

        protected internal override bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "spritesheet":
                case "spritesheetname":
                    _spriteSheet = value;
                    return true;
                case "animation":
                case "animationname":
                    _animationName = value;
                    return true;
                case "frequency":
                    Frequency = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "speed":
                    OverRange speedRange = OverRange.Parse(value);
                    MinSpeed = speedRange.Lower;
                    MaxSpeed = speedRange.Upper;
                    return true;
                case "lifespan":
                    OverRange lifespanRange = OverRange.Parse(value);
                    MinLifespan = lifespanRange.Lower;
                    MaxLifespan = lifespanRange.Upper;
                    return true;
                case "rotation":
                    OverRange rotationRange = OverRange.Parse(value);
                    MinRotation = rotationRange.Lower.ToRadians();
                    MaxRotation = rotationRange.Upper.ToRadians();
                    return true;
                case "angle":
                    OverRange angleRange = OverRange.Parse(value);
                    MinAngle = angleRange.Lower.ToRadians();
                    MaxAngle = angleRange.Upper.ToRadians();
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
            SpriteParticle newParticle = _particlePool.Fetch();
            newParticle.Entity = SpriteEntity.Create(_spriteSheet);
            newParticle.Lifespan = G.Random.Between(MinLifespan, MaxLifespan);
            newParticle.AngularVelocity = G.Random.Between(MinRotation, MaxRotation);
            newParticle.Velocity = -Vector2Extensions.DirectionBetween(MinAngle, MaxAngle) * G.Random.Between(MinSpeed, MaxSpeed);
            newParticle.Entity.Position = G.Random.Between(Position - Size / 2, Position + Size / 2);
            newParticle.Entity.Origin = new Vector2(0.5f, 0.5f);
            newParticle.Entity.Play(_animationName, PlayOption.Random);
            newParticle.Entity.Pause();
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
                SpriteParticle particle = _particles[i];
                particle.Entity.InternalUpdate(time);
                particle.Lifespan -= time.ElapsedSeconds;
                particle.Entity.Position += particle.Velocity * time.ElapsedSeconds;
                particle.Entity.Rotation += particle.AngularVelocity * time.ElapsedSeconds;

                // re-asign to keep state changes; eg. lifespan.
                _particles[i] = particle; 

                if (particle.IsDead)
                {
                    particle.Recycle();
                    _particles.FastRemove<SpriteParticle>(i);
                    i--;
                }
            }
            base.LightUpdate(time);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _particles.Count; i++)
            {
                SpriteParticle particle = _particles[i];
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
            _spriteSheet = String.Empty;
            _animationName = String.Empty;
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
            Explode = false;
            ExplodeQuantity = 0;
            for (int i = 0; i < _particles.Count; i++)
            {
                SpriteParticle particle = _particles[i];
                particle.Recycle();
            }

            _pool.Release(this);
        }

    }
}
