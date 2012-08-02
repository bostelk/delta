using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;
using Delta.Extensions;
using System.Globalization;
using Delta.Transformations;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{
    public abstract class Emitter : TransformableEntity
    {
        [ContentSerializer]
        string _fadeInMethodString;
        [ContentSerializer]
        string _fadeOutMethodString;
        Interpolation.InterpolationMethod _fadeInInterpolator;
        Interpolation.InterpolationMethod _fadeOutInterpolator;

        [ContentSerializer]
        string _blendString;
        BlendState _blend;

        public float Frequency;
        public bool Explode;
        public Range QuantityRange;
        public Range LifespanRange;
        public Range VelocityMagnitudeRange;
        public Range AccelerationMagnitudeRange;
        public Range RotationRange;
        public Range VelocityAngleRange;
        public Range AccelerationAngleRange;
        public Range ScaleRange;
        public Range FrameIntervalRange;
        public Range FadeInRange;
        public Range FadeOutRange;

        // fuck you for not serializing blendstates; warning will hardcrash visual studio
        [ContentSerializerIgnore]
        public BlendState Blend
        {
            get
            {
                if (_blend == null)
                    _blend = BlendStateExtensions.Parse(_blendString);
                return _blend;
            }
        }

        // fuck you for not serializing delegates
        [ContentSerializerIgnore]
        public Interpolation.InterpolationMethod FadeInInterpolator
        {
            get
            {
                if (_fadeInInterpolator == null) 
                    _fadeInInterpolator = Interpolation.Parse(_fadeInMethodString); 
                return _fadeInInterpolator;
            }
        }

        // fuck you for not serializing delegates
        [ContentSerializerIgnore]
        public Interpolation.InterpolationMethod FadeOutInterpolator
        {
            get
            {
                if (_fadeOutInterpolator == null) 
                    _fadeOutInterpolator = Interpolation.Parse(_fadeOutMethodString); 
                return _fadeOutInterpolator;
            }
        }

        public Emitter()
        {
            VelocityAngleRange = new Range(0, 360);
            ScaleRange = new Range(1);
            QuantityRange = new Range(1, 1);
            _fadeInMethodString = "Linear";
            _fadeOutMethodString = "Linear";
            _blendString = "AlphaBlend";
        }

        protected internal override bool SetValue(string name, string value)
        {
            switch (name)
            {
                case "frequency":
                    Frequency = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "lifespan":
                    LifespanRange = Range.TryParse(value);
                    return true;
                case "rotation":
                case "rotationspeed":
                    RotationRange = Range.TryParse(value);
                    RotationRange.Lower = RotationRange.Lower.ToRadians();
                    RotationRange.Upper = RotationRange.Upper.ToRadians();
                    return true;
                case "v":
                case "vel":
                case "velocity":
                    VelocityMagnitudeRange = Range.TryParse(value);
                    return true;
                case "vangle":
                case "velangle":
                case "velocityangle":
                    VelocityAngleRange = Range.TryParse(value);
                    VelocityAngleRange.Lower = VelocityAngleRange.Lower.ToRadians();
                    VelocityAngleRange.Upper = VelocityAngleRange.Upper.ToRadians();
                    return true;
                case "a":
                case "accel":
                case "acceleration":
                    AccelerationMagnitudeRange = Range.TryParse(value);
                    return true;
                case "aangle":
                case "accelangle":
                case "accelerationangle":
                    AccelerationAngleRange = Range.TryParse(value);
                    AccelerationAngleRange.Lower = AccelerationAngleRange.Lower.ToRadians();
                    AccelerationAngleRange.Upper = AccelerationAngleRange.Upper.ToRadians();
                    return true;
                case "scale":
                case "size":
                    ScaleRange = Range.TryParse(value);
                    return true;
                case "frameinterval":
                case "frameduration":
                    FrameIntervalRange = Range.TryParse(value);
                    return true;
                case "explode":
                    Explode = true;
                    QuantityRange = Range.TryParse(value);
                    return true;
                case "quantity":
                    QuantityRange = Range.TryParse(value);
                    return true;
                case "fadein":
                    FadeInRange = Range.TryParse(value);
                    return true;
                case "fadeout":
                    FadeOutRange = Range.TryParse(value);
                    return true;
                case "fadeinmethod":
                    _fadeInMethodString = value;
                    return true;
                case "fadeoutmethod":
                    _fadeOutMethodString = value;
                    return true;
                case "blend":
                    _blendString = value;
                    return true;
            }
            return base.SetValue(name, value);
        }

        public override void Recycle()
        {
            base.Recycle();
            Frequency = 0;
            Explode = false;
            QuantityRange = new Range(1, 1);
            LifespanRange = Range.Empty;
            RotationRange = Range.Empty;
            VelocityMagnitudeRange = Range.Empty;
            VelocityAngleRange = Range.Empty;
            AccelerationMagnitudeRange = Range.Empty;
            AccelerationAngleRange = Range.Empty;
            ScaleRange = new Range(1, 1);
            FadeInRange = Range.Empty;
            FadeOutRange = Range.Empty;
            _fadeInMethodString = "Linear";
            _fadeOutMethodString = "Linear";
            _blendString = "AlphaBlend";
        }

        internal class Particle<T> : IRecyclable where T: TransformableEntity
        {
            public Emitter Emitter;

            public T Entity;

            /// <summary>
            /// The amount of time spent living.
            /// </summary>
            public float Life;

            /// <summary>
            /// The total amount of time to live.
            /// </summary>
            public float Lifespan;

            public Vector2 Acceleration;
            public Vector2 Velocity;
            public float AngularVelocity;

            public float FadeOutPercent;
            public float FadeInPercent;

            public bool IsDead { get { return Life >= Lifespan; } }

            public virtual void Recycle()
            {
                if (Entity != null)
                {
                    Entity.Recycle();
                    Entity = null;
                }
                Life = 0;
                Lifespan = 0;
                Acceleration = Vector2.Zero;
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
                FadeOutPercent = 0;
                FadeInPercent = 0;
            }

            public virtual void Update(DeltaGameTime time)
            {
                if (FadeInPercent > 0)
                    Entity.Alpha = Emitter.FadeInInterpolator(0, 1, Life / (FadeInPercent * Lifespan)); 
                if (FadeOutPercent > 0)
                    Entity.Alpha = Entity.Alpha - Emitter.FadeOutInterpolator(0, 1, (Life - (Lifespan - FadeOutPercent * Lifespan)) / (FadeOutPercent * Lifespan));
            }

            public virtual void Draw(DeltaGameTime time, SpriteBatch spriteBatch) { }
            public virtual void OnEmitted() { }

        }
    }
}
