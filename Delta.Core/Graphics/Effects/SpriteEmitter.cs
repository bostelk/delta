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
                base.OnEmitted();
            }

            public void Update(DeltaTime time)
            {
                if (FadeInPercent > 0)
                    Entity.Alpha = Interpolation.EaseInCubic(0, 1, Life / (FadeInPercent * Lifespan)); 
                if (FadeOutPercent > 0)
                    Entity.Alpha = Entity.Alpha - Interpolation.EaseOutCubic(0, 1, (Life - (Lifespan - FadeOutPercent * Lifespan)) / (FadeOutPercent * Lifespan));
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
            AngleRange = new Range(0, 360);
            ScaleRange = new Range(1, 1);
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
            }
            return base.ImportCustomValues(name, value);
        }

        public void Emit()
        {
            SpriteParticle newParticle = _particlePool.Fetch();
            newParticle.Entity = SpriteEntity.Create(_spriteSheet);
            newParticle.Lifespan = LifespanRange.RandomWithin();
            newParticle.AngularVelocity = RotationRange.RandomWithin();
            newParticle.Velocity = Vector2Extensions.DirectionBetween(AngleRange.Lower, AngleRange.Upper) * SpeedRange.RandomWithin();
            newParticle.Velocity.Y *= -1;
            newParticle.FadeInPercent = FadeInRange.RandomWithin();
            newParticle.FadeOutPercent = FadeOutRange.RandomWithin();
            newParticle.Entity.Scale = G.Random.Between(new Vector2(ScaleRange.Lower), new Vector2(ScaleRange.Upper));
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
                particle.Life += time.ElapsedSeconds;
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
            Explode = false;
            Quantity = 1;
            LifespanRange = Range.Empty;
            SpeedRange = Range.Empty;
            RotationRange = Range.Empty;
            AngleRange = Range.Empty;
            ScaleRange = new Range(1, 1);
            FadeInRange = Range.Empty;
            FadeOutRange = Range.Empty;

            for (int i = 0; i < _particles.Count; i++)
            {
                SpriteParticle particle = _particles[i];
                particle.Recycle();
            }

            _pool.Release(this);
        }

    }
}
