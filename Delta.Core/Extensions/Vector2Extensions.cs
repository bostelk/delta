using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Globalization;

namespace Delta
{
    public static class Vector2Extensions
    {
        public const double EPSILON = 9.9999997473787516E-05;
        public const double EPSILONSQR = EPSILON * EPSILON;

        public static void SafeNormalize(ref Vector2 v)
        {
            // if (v.LengthSquared() < EPSILONSQR) a more safe check.
            if (v != Vector2.Zero)
                Vector2.Normalize(ref v, out v);
            Debug.Assert(v.X.IsValid() && v.Y.IsValid());
        }

        public static float AngleBetween(this Vector2 a, Vector2 b)
        {
            SafeNormalize(ref a);
            SafeNormalize(ref b);
            return (float) Math.Cos(Vector2.Dot(a, b));
        }

        /// <summary>
        /// Rotate the vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="theta">Angle in radians.</param>
        /// <returns></returns>
        public static Vector2 Rotate(this Vector2 v, float theta)
        {
            // early exit if theta is 0 or PI ?
            Vector2 result = Vector2.Zero;
            float cos = (float)Math.Cos(theta);
            float sin = (float)Math.Sin(theta);
            result.X = v.X * cos - v.Y * sin;
            result.Y = v.X * sin + v.Y * cos;
            return result;
        }

        public static Vector2 RandomDirection()
        {
            Vector2 result = default(Vector2);
            result.X = (float)Math.Cos(G.Random.Next() * MathHelper.TwoPi);
            result.Y = (float)Math.Sin(G.Random.Next() * MathHelper.TwoPi);
            return result;
        }

        public static Vector2 DirectionBetween(float startAngle, float finishAngle)
        {
            Vector2 result = Vector2.Zero;
            return result;
        }

        public static Vector2 PerpendicularLeft(Vector2 v)
        {
            Vector2 result = Vector2.Zero;
            result.X = -v.Y;
            result.Y = v.X;
            return result;
        }

        public static Vector2 PerpendicularRight(Vector2 v)
        {
            Vector2 result = Vector2.Zero;
            result.X = v.Y;
            result.Y = -v.X;
            return result;
        }

        public static Vector2 ProjectOnto(Vector2 a, Vector2 b)
        {
            Vector2 result = Vector2.Zero;
            SafeNormalize(ref b);
            result = Vector2.Dot(a, b) * b; 
            return result;
        }

       public static bool AlmostEqual(Vector2 a, Vector2 b)
        {
            return (FloatExtensions.AlmostEqual(a.X, b.X, (float)EPSILON) && FloatExtensions.AlmostEqual(a.Y, b.Y, (float)EPSILON));
        }

        public static bool WithinDistanceQuick(Vector2 a, Vector2 b, float targetDistanceSqr)
        {
            float distanceSqr = Vector2.DistanceSquared(a, b);
            return distanceSqr < targetDistanceSqr;
        }

        public static bool WithinDistance(Vector2 a, Vector2 b, float targetDistance)
        {
            float distanceSqr = Vector2.Distance(a, b);
            return distanceSqr < targetDistance;
        }

        public static Point ToPoint(this Vector2 v)
        {
            Point p = Point.Zero;
            p.X = (int) v.X;
            p.Y = (int) v.Y;
            return p;
        }

        public static float CrossProduct(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - b.X * a.Y;
        }

        public static bool IsParallel(Vector2 a, Vector2 b)
        {
            return CrossProduct(a, b) < EPSILON;
        }

        public static Vector2 Parse(string value, IFormatProvider provider)
        {
            string[] split = value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length >= 2)
            {
                return new Vector2(
                    float.Parse(split[0], provider),
                    float.Parse(split[1], provider)
                    );
            }
            else
                return new Vector2(
                    float.Parse(split[0], provider),
                    float.Parse(split[0], provider)
                    );
        }

        public static Vector2 Parse(string value)
        {
            return Parse(value, CultureInfo.InvariantCulture);
        }

    }
}
