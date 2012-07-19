using System;
using Delta;
using Microsoft.Xna.Framework;

namespace Delta.Movement
{
    /// <summary>
    /// A static class that contains predefined methods of interpolation.
    /// </summary>
    public static class Interpolation
    {
        public delegate float InterpolationMethod(float start, float end, float amount);

        public static readonly InterpolationMethod Linear = LinearInterpolation;

        public static readonly InterpolationMethod EaseInQuad = EaseInQuadInterpolation;

        public static readonly InterpolationMethod EaseOutQuad = EaseOutQuadInterpolation;

        public static readonly InterpolationMethod EaseInOutQuad = EaseInOutQuadInterpolation;

        public static readonly InterpolationMethod EaseInCubic = EaseInCubicInterpolation;

        public static readonly InterpolationMethod EaseOutCubic = EaseOutCubicInterpolation;

        public static readonly InterpolationMethod EaseInOutCubic = EaseInOutCubicInterpolation;

        public static readonly InterpolationMethod EaseInQuart = EaseInQuartInterpolation;

        public static readonly InterpolationMethod EaseOutQuart = EaseOutQuartInterpolation;

        public static readonly InterpolationMethod EaseInOutQuart = EaseInOutQuartInterpolation;

        public static readonly InterpolationMethod EaseInQuint = EaseInQuintInterpolation;

        public static readonly InterpolationMethod EaseOutQuint = EaseOutQuintInterpolation;

        public static readonly InterpolationMethod EaseInOutQuint = EaseInOutQuintInterpolation;

        public static readonly InterpolationMethod EaseInSine = EaseInSineInterpolation;

        public static readonly InterpolationMethod EaseOutSine = EaseOutSineInterpolation;

        public static readonly InterpolationMethod EaseInOutSine = EaseInOutSineInterpolation;

        public static readonly InterpolationMethod EaseInExpo = EaseInExpoInterpolation;

        public static readonly InterpolationMethod EaseOutExpo = EaseOutExpoInterpolation;

        public static readonly InterpolationMethod EaseInOutExpo = EaseInOutExpoInterpolation;

        public static readonly InterpolationMethod EaseInCircle = EaseInCircInterpolation;

        public static readonly InterpolationMethod EaseOutCircle = EaseOutCircInterpolation;

        public static readonly InterpolationMethod EaseInOutCircle = EaseInOutCircInterpolation;

        public static readonly InterpolationMethod EaseInBounce = EaseInBounceInterpolation;

        public static readonly InterpolationMethod EaseOutBounce = EaseOutBounceInterpolation;

        public static readonly InterpolationMethod EaseInOutBounce = EaseInOutBounceInterpolation;

        /// <summary>
        /// Parse the value in the form of method follower by ease: method.ease. Ex: quad.in, quad.out, quad.inout.
        /// </summary>
        /// <param name="value">Interpolation method expressed as a string.</param>
        /// <returns>Interpolation Method</returns>
        public static InterpolationMethod Parse(string value)
        {
            string[] split = value.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length <= 1) return Linear;
            switch (split[0])
            {
                case "quad":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInQuad;
                        case "out":
                            return EaseOutQuad;
                        case "inout":
                            return EaseInOutQuad;
                        default:
                            return Linear;
                    }
                case "cubic":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInCubic;
                        case "out":
                            return EaseOutCubic;
                        case "inout":
                            return EaseInOutCubic;
                        default:
                            return Linear;
                    }
                case "quart":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInQuart;
                        case "out":
                            return EaseOutQuart;
                        case "inout":
                            return EaseInOutQuart;
                        default:
                            return Linear;
                    }
                case "quint":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInQuart;
                        case "out":
                            return EaseOutQuart;
                        case "inout":
                            return EaseInOutQuart;
                        default:
                            return Linear;
                    }
                case "sine":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInSine;
                        case "out":
                            return EaseOutSine;
                        case "inout":
                            return EaseInOutSine;
                        default:
                            return Linear;
                    }
                case "expo":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInExpo;
                        case "out":
                            return EaseOutExpo;
                        case "inout":
                            return EaseInOutExpo;
                        default:
                            return Linear;
                    }
                case "circle":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInCircle;
                        case "out":
                            return EaseOutCircle;
                        case "inout":
                            return EaseInOutCircle;
                        default:
                            return Linear;
                    }
                case "bounce":
                    switch (split[1])
                    {
                        case "in":
                            return EaseInBounce;
                        case "out":
                            return EaseOutBounce;
                        case "inout":
                            return EaseInOutBounce;
                        default:
                            return Linear;
                    }
                default:
                    return Linear;
            }
        }

        /**
         * Linear interpolation (same as Mathf.Lerp)
         */
        static float LinearInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * amount + start;
        }

        /**
         * quadratic easing in - accelerating from zero velocity
         */
        static float EaseInQuadInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * amount * amount + start;
        }

        /**
         * quadratic easing out - decelerating to zero velocity
         */
        static float EaseOutQuadInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return -(end - start) * amount * (amount - 2) + start;
        }

        /**
         * quadratic easing in/out - acceleration until halfway, then deceleration
         */
        static float EaseInOutQuadInterpolation(float start, float end, float amount)
        {
            // clamp amount so that it cannot be greater than duration
            amount = MathExtensions.Clamp(amount * 2, 0f, 2f);
            if (amount < 1) return (end - start) / 2 * amount * amount + start;
            amount--;
            return -(end - start) / 2 * (amount * (amount - 2) - 1) + start;
        }

        /**
         * cubic easing in - accelerating from zero velocity
         */
        static float EaseInCubicInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * amount * amount * amount + start;
        }

        /**
         * cubic easing out - decelerating to zero velocity
         */
        static float EaseOutCubicInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            amount--;
            return (end - start) * (amount * amount * amount + 1) + start;
        }

        /**
         * cubic easing in/out - acceleration until halfway, then deceleration
         */
        static float EaseInOutCubicInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount * 2, 0f, 2f);
            if (amount < 1) return (end - start) / 2 * amount * amount * amount + start;
            amount -= 2;
            return (end - start) / 2 * (amount * amount * amount + 2) + start;
        }

        /**
         * quartic easing in - accelerating from zero velocity
         */
        static float EaseInQuartInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * amount * amount * amount * amount + start;
        }

        /**
         * quartic easing out - decelerating to zero velocity
         */
        static float EaseOutQuartInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            amount--;
            return -(end - start) * (amount * amount * amount * amount - 1) + start;
        }

        /**
         * quartic easing in/out - acceleration until halfway, then deceleration
         */
        static float EaseInOutQuartInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount * 2, 0f, 2f);
            if (amount < 1) return (end - start) / 2 * amount * amount * amount * amount + start;
            amount -= 2;
            return -(end - start) / 2 * (amount * amount * amount * amount - 2) + start;
        }


        /**
         * quintic easing in - accelerating from zero velocity
         */
        static float EaseInQuintInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * amount * amount * amount * amount * amount + start;
        }

        /**
         * quintic easing out - decelerating to zero velocity
         */
        static float EaseOutQuintInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            amount--;
            return (end - start) * (amount * amount * amount * amount * amount + 1) + start;
        }

        /**
         * quintic easing in/out - acceleration until halfway, then deceleration
         */
        static float EaseInOutQuintInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount * 2, 0f, 2f);
            if (amount < 1) return (end - start) / 2 * amount * amount * amount * amount * amount + start;
            amount -= 2;
            return (end - start) / 2 * (amount * amount * amount * amount * amount + 2) + start;
        }

        /**
         * sinusoidal easing in - accelerating from zero velocity
         */
        static float EaseInSineInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return -(end - start) * (float)Math.Cos(amount * ((float)MathHelper.PiOver2)) + (end - start) + start;
        }

        /**
         * sinusoidal easing out - decelerating to zero velocity
         */
        static float EaseOutSineInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * (float)Math.Sin(amount * ((float)MathHelper.PiOver2)) + start;
        }

        /**
         * sinusoidal easing in/out - accelerating until halfway, then decelerating
         */
        static float EaseInOutSineInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return -(end - start) / 2 * ((float)Math.Cos((float)Math.PI * amount) - 1) + start;
        }

        /**
         * exponential easing in - accelerating from zero velocity
         */
        static float EaseInExpoInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * (float)Math.Pow(2, 10 * (amount - 1)) + start;
        }

        /**
         * exponential easing out - decelerating to zero velocity
         */
        static float EaseOutExpoInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return (end - start) * (-(float)Math.Pow(2, -10 * amount) + 1) + start;
        }

        /**
         * exponential easing in/out - accelerating until halfway, then decelerating
         */
        static float EaseInOutExpoInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount * 2, 0f, 2f);
            if (amount < 1) return (end - start) / 2 * (float)Math.Pow(2, 10 * (amount - 1)) + start;
            amount--;
            return (end - start) / 2 * (-(float)Math.Pow(2, -10 * amount) + 2) + start;
        }

        /**
         * circular easing in - accelerating from zero velocity
         */
        static float EaseInCircInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            return -(end - start) * ((float)Math.Sqrt(1 - amount * amount) - 1) + start;
        }

        /**
         * circular easing out - decelerating to zero velocity
         */
        static float EaseOutCircInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount, 0f, 1f);
            amount--;
            return (end - start) * (float)Math.Sqrt(1 - amount * amount) + start;
        }

        /**
         * circular easing in/out - acceleration until halfway, then deceleration
         */
        static float EaseInOutCircInterpolation(float start, float end, float amount)
        {
            amount = MathExtensions.Clamp(amount * 2, amount, 2f);
            if (amount < 1) return -(end - start) / 2 * ((float)Math.Sqrt(1 - amount * amount) - 1) + start;
            amount -= 2;
            return (end - start) / 2 * ((float)Math.Sqrt(1 - amount * amount) + 1) + start;
        }

        const float coef1 = 1 / 2.75f; const float coef2 = 2 / 2.75f;
        const float coef3 = 2.5f / 2.75f; const float coef4 = 1.5f / 2.75f;
        const float coef5 = 2.25f / 2.75f; const float coef6 = 2.625f / 2.75f;

        static float EaseOutBounceInterpolation(float start, float end, float amount)
        {
            if (amount < coef1)
            {
                return (end - start) * (7.5625f * amount * amount) + start;
            }
            else if (amount < coef2)
            {
                return (end - start) * (7.5625f * (amount -= (coef4)) * amount + 0.75f) + start;
            }
            else if (amount < coef3)
            {
                return (end - start) * (7.5625f * (amount -= (coef5)) * amount + 0.9375f) + start;
            }
            else
            {
                return (end - start) * (7.5625f * (amount -= (coef6)) * amount + 0.984375f) + start;
            }
        }

        static float EaseInBounceInterpolation(float start, float end, float amount)
        {
            return (end - start) - EaseOutBounceInterpolation(1 - amount, 0, (end - start) / 1) + start;
        }

        static float EaseInOutBounceInterpolation(float start, float end, float amount)
        {
            if (amount < 0.5f)
                return EaseInBounceInterpolation(amount * 2, 0, (end - start) / 1) * .5f + start;
            else
                return EaseOutBounceInterpolation(amount * 2 - 1, 0, (end - start) / 1) * .5f + (end - start) * .5f + start;
        }

    }
}
