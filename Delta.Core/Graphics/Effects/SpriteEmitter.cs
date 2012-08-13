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
    public class SpriteEmitter : Emitter
    {

        internal class AnimatedSpriteParticle : Particle<AnimatedSpriteEntity>
        {

            public AnimatedSpriteParticle()
            {
            }

            public override void Update(DeltaGameTime time)
            {
                Entity.InternalUpdate(time);
                base.Update(time);
            }

            public override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
            {
                Entity.InternalDraw(time, spriteBatch);
                base.Draw(time, spriteBatch);
            }

        }

        List<AnimatedSpriteParticle> _particles;
        float _lastEmitTime;
        
        [ContentSerializer]
        string _spriteSheet;
        [ContentSerializer]
        string _animationName;
        [ContentSerializer]
        Range _timeScaleRange;
        [ContentSerializer]
        AnimationOptions _spriteOptions;

        [ContentSerializerIgnore, DisplayName("Sprite Sheet"), Description(""), Category("Sprite Emitter"), Browsable(true), ReadOnly(false), DefaultValue(false)]
        public string SpriteSheet { get { return _spriteSheet; } set { _spriteSheet = value; } }
        [ContentSerializerIgnore, DisplayName("Animation Name"), Description(""), Category("Sprite Emitter"), Browsable(true), ReadOnly(false), DefaultValue(false)]
        public string AnimationName { get { return _animationName; } set { _animationName = value; } }
        [ContentSerializerIgnore, DisplayName("Time Scale"), Description(""), Category("Sprite Emitter"), Browsable(true), ReadOnly(false), DefaultValue(false), Editor(typeof(Delta.Editor.RangeUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Range TimeScaleRange { get { return _timeScaleRange; } set { _timeScaleRange = value; } }
        [ContentSerializerIgnore, DisplayName("Animation Options"), Description(""), Category("Sprite Emitter"), Browsable(true), ReadOnly(false), DefaultValue(false), Editor(typeof(Delta.Editor.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public AnimationOptions SpriteOptions { get { return _spriteOptions; } set { _spriteOptions = value; } }

        public static SpriteEmitter Create(string spriteSheet, string animationName)
        {
            SpriteEmitter emitter = Pool.Acquire<SpriteEmitter>();
            emitter._spriteSheet = spriteSheet;
            emitter._animationName = animationName;
            G.World.AboveGround.UnsafeAdd(emitter);
            return emitter;
        }

        static AnimatedSpriteParticle CreateParticle()
        {
            AnimatedSpriteParticle particle = Pool.Acquire<AnimatedSpriteParticle>();
            return particle;
        }

        public SpriteEmitter() : base()
        {
            _particles = new List<AnimatedSpriteParticle>(100);
            TimeScaleRange = new Range(1);
        }

        protected override void Recycle(bool isReleasing)
        {
            _spriteSheet = String.Empty;
            _animationName = String.Empty;
            _lastEmitTime = 0;
            TimeScaleRange = new Range(1);

            if (isReleasing)
            {
                for (int i = 0; i < _particles.Count; i++)
                {
                    AnimatedSpriteParticle particle = _particles[i];
                    particle.Recycle();
                }
            }
            base.Recycle(isReleasing);
        }


        protected internal override bool SetValue(string name, string value)
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
                case "timescale":
                    TimeScaleRange = Range.TryParse(value);
                    return true;
                case "looped":
                    SpriteOptions = bool.Parse(value) ? SpriteOptions | AnimationOptions.Looped : SpriteOptions;
                    return true;
                case "startrandom":
                case "random":
                    SpriteOptions = bool.Parse(value) ? SpriteOptions | AnimationOptions.StartOnRandomFrame : SpriteOptions;
                    return true;
            }
            return base.SetValue(name, value);
        }

        public void Emit()
        {
            AnimatedSpriteParticle newParticle = Pool.Acquire<AnimatedSpriteParticle>();
            newParticle.Emitter = this;
            newParticle.Entity = AnimatedSpriteEntity.Create(_spriteSheet);
            newParticle.Lifespan = LifespanRange.RandomWithin();
            newParticle.Acceleration = Vector2Extensions.DirectionBetween(AccelerationAngleRange.Lower.ToRadians(), AccelerationAngleRange.Upper.ToRadians()) * AccelerationMagnitudeRange.RandomWithin();
            newParticle.AngularVelocity = RotationRange.RandomWithin();
            newParticle.Velocity = Vector2Extensions.DirectionBetween(VelocityAngleRange.Lower.ToRadians(), VelocityAngleRange.Upper.ToRadians()) * VelocityMagnitudeRange.RandomWithin();
            newParticle.FadeInPercent = FadeInRange.RandomWithin();
            newParticle.FadeOutPercent = FadeOutRange.RandomWithin();
            newParticle.Entity.Tint = Tint;
            newParticle.Entity.TimeScale = TimeScaleRange.RandomWithin();
            newParticle.Entity.Scale = G.Random.Between(new Vector2(ScaleRange.Lower), new Vector2(ScaleRange.Upper));
            newParticle.Entity.Origin = Vector2.One * 0.5f;
            newParticle.Entity.Position = G.Random.Between(Position, Position + Size); // tiled gives up the position as top-let
            newParticle.Entity.InternalLoadContent(); // otherwise the sprite will not play because the spritessheet has not been loaded.
            newParticle.Entity.Play(_animationName, SpriteOptions);
            newParticle.OnEmitted();
            _particles.Add(newParticle);
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            if (G.World.SecondsPast(_lastEmitTime + _frequency))
            {
                int quantity = (int) (QuantityRange.RandomWithin() + .5);
                for (int i = 0; i < quantity; i++)
                    Emit();
                _lastEmitTime = time.TotalSeconds;
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                AnimatedSpriteParticle particle = _particles[i];
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
                    _particles.FastRemove<AnimatedSpriteParticle>(i);
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
                AnimatedSpriteParticle particle = _particles[i];
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
