using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Structures;
using System.Globalization;

namespace Delta.Graphics
{
    public abstract class Emitter : Entity
    {
        public float Frequency;
        public bool Explode;
        public int Quantity;
        public Range LifespanRange;
        public Range SpeedRange;
        public Range AccelerationRange;
        public Range RotationRange;
        public Range AngleRange;
        public Range ScaleRange;
        public Range FrameIntervalRange;
        public Range FadeInRange;
        public Range FadeOutRange;

        protected internal override bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "frequency":
                    Frequency = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "speed":
                    SpeedRange = Range.Parse(value);
                    return true;
                case "lifespan":
                    LifespanRange = Range.Parse(value);
                    return true;
                case "rotation":
                case "rotationspeed":
                    RotationRange = Range.Parse(value);
                    RotationRange.Lower = RotationRange.Lower.ToRadians();
                    RotationRange.Upper = RotationRange.Upper.ToRadians();
                    return true;
                case "angle":
                    AngleRange = Range.Parse(value);
                    AngleRange.Lower = AngleRange.Lower.ToRadians();
                    AngleRange.Upper = AngleRange.Upper.ToRadians();
                    return true;
                case "scale":
                    ScaleRange = Range.Parse(value);
                    return true;
                case "acceleration":
                    AccelerationRange = Range.Parse(value);
                    return true;
                case "frameinterval":
                    FrameIntervalRange = Range.Parse(value);
                    return true;
                case "explode":
                    Explode = true;
                    Quantity = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "quantity":
                    Quantity = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "fadein":
                    FadeInRange = Range.Parse(value);
                    return true;
                case "fadeout":
                    FadeOutRange = Range.Parse(value);
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }

        internal class Particle<T> : IRecyclable where T: Entity
        {
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
                Entity.Recycle();
                Entity = null;
                Life = 0;
                Lifespan = 0;
                Acceleration = Vector2.Zero;
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
                FadeOutPercent = 0;
                FadeInPercent = 0;
            }

            public virtual void OnEmitted()
            {
            }

        }
    }
}
