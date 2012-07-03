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

namespace Delta
{
    public static class Vector2Extensions
    {
        public const double EPSILON = 9.9999997473787516E-05;
        public const double EPSILONSQR = EPSILON * EPSILON;
        
        public static void TryNormalize(this Vector2 v)
        {
            if (v != Vector2.Zero)
                v.Normalize();
            Debug.Assert(v.X.IsValid() && v.Y.IsValid());
        }

        public static float AngleBetween(this Vector2 a, Vector2 b)
        {
            a.TryNormalize();
            b.TryNormalize();
            return (float) Math.Cos(Vector2.Dot(a, b));
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
            b.TryNormalize();
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
    }
}
