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
    public class SpriteEmitter : Emitter
    {

        internal class SpriteParticle : Particle<SpriteEntity>
        {
            float _trailInterval = 0.1f;
            float _lastTrailTime = 0;

            public override void Recycle()
            {
                base.Recycle();

                _particlePool.Release(this);
            }

            public override void OnEmitted()
            {
                //Entity.Alpha = 0;
                //Transformer.ThisEntity(Entity).FadeTo(1, Lifespan / 2, Interpolation.EaseInCubic).FadeTo(0, Lifespan / 2, Interpolation.EaseOutCubic);
                base.OnEmitted();
            }

            public void Update(DeltaTime time)
            {
                //if (G.World.SecondsPast(_lastTrailTime + _trailInterval))
                //{
                //    Visuals.CreateTrail(Entity, Entity.Position);
                //    _lastTrailTime = time.TotalSeconds;
                //}

                Entity.InternalUpdate(time);
            }

            public void Draw(DeltaTime time, SpriteBatch spriteBatch)
            {
                Entity.InternalDraw(time, spriteBatch);
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
        public float MinScale;
        public float MaxScale;
        public float MinFrameInterval;
        public float MaxFrameInterval;
        public bool Explode;
        public int Quantity;

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
            MinScale = 1;
            MaxScale = 1;
            Quantity = 1;
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
                case "scale":
                    OverRange scaleRange = OverRange.Parse(value);
                    MinScale = scaleRange.Lower;
                    MaxScale = scaleRange.Upper;
                    return true;
                case "acceleration":
                    OverRange accelerationRange = OverRange.Parse(value);
                    MinAcceleration = accelerationRange.Lower;
                    MaxAcceleration = accelerationRange.Upper;
                    return true;
                case "frameinterval":
                    OverRange frameIntervalRange = OverRange.Parse(value);
                    MinFrameInterval = frameIntervalRange.Lower;
                    MaxFrameInterval = frameIntervalRange.Upper;
                    return true;
                case "explode":
                    Explode = true;
                    Quantity = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "quantity":
                    Quantity = int.Parse(value, CultureInfo.InvariantCulture);
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
            newParticle.Velocity = Vector2Extensions.DirectionBetween(MinAngle, MaxAngle) * G.Random.Between(MinSpeed, MaxSpeed);
            newParticle.Velocity.Y *= -1;
            newParticle.Entity.Scale = G.Random.Between(new Vector2(MinScale), new Vector2(MaxScale));
            newParticle.Entity.Origin = new Vector2(0.5f, 0.5f);
            newParticle.Entity.Position = G.Random.Between(Position, Position + Size); // tiled gives up the position as top-let
            newParticle.Entity.LoadContent(); // otherwise the sprite will not play because the spritessheet has not been loaded.
            newParticle.Entity.Play(_animationName);
            //`newParticle.Entity.Pause();
            newParticle.OnEmitted();
            _particles.Add(newParticle);
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (G.World.SecondsPast(_lastEmitTime + Frequency))
            {
                for (int i = 0; i < Quantity; i++)
                    Emit();
                _lastEmitTime = time.TotalSeconds;
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                SpriteParticle particle = _particles[i];
                particle.Update(time);
                particle.Lifespan -= time.ElapsedSeconds;
                particle.Velocity += particle.Acceleration * time.ElapsedSeconds;
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
                particle.Draw(time, spriteBatch);
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
            MinScale = 1;
            MaxScale = 1;
            Explode = false;
            Quantity = 1;
            for (int i = 0; i < _particles.Count; i++)
            {
                SpriteParticle particle = _particles[i];
                particle.Recycle();
            }

            _pool.Release(this);
        }

    }
}
