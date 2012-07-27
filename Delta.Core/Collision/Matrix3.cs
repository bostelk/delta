using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Globalization;

namespace Delta.Collision
{
    /// <summary>
    /// A 2x2 Transformation matrix; currently lacking a lot of code...
    /// </summary>
    [DebuggerDisplay("{ {{M11} {M12} {M13}} {{M21} {M22} {M23}} {{M31} {M32} {M33}} }")]
    public struct Matrix3 : IEquatable<Matrix3>
    {
        public float M11;
        public float M12;
        public float M13;
        public float M21;
        public float M22;
        public float M23;
        public float M31;
        public float M32;
        public float M33;

        public static Matrix3 Zero = new Matrix3();
        public static Matrix3 Identity = new Matrix3() {
            M11 = 1, M12 = 0, M13 = 0,
            M21 = 0, M22 = 1, M23 = 0,
            M31 = 0, M32 = 0, M33 = 1,
        };

        public Vector2 Origin
        {
            get
            {
                return new Vector2(M31, M32);
            }
        }

        public Matrix3(ref Matrix3 m)
        {
            M11 = m.M11; M12 = m.M12; M13 = m.M13;
            M21 = m.M21; M22 = m.M22; M23 = m.M23;
            M31 = m.M31; M32 = m.M32; M33 = m.M33;
        }

        public Matrix3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
        {
            M11 = m11; M12 = m12; M13 = m13;
            M21 = m21; M22 = m22; M23 = m23;
            M31 = m31; M32 = m32; M33 = m33;
        }

        public static Matrix3 CreateTranslation(Vector2 delta)
        {
            Matrix3 result = Matrix3.Identity;
            result.M31 = delta.X;
            result.M32 = delta.Y;
            return result;
        }

        public static void CreateTranslation(ref Vector2 delta, out Matrix3 result)
        {
            result = Matrix3.Identity;
            result.M31 = delta.X;
            result.M32 = delta.Y;
        }

        // clock-wise rotation.
        public static Matrix3 CreateRotation(float radians)
        {
            Matrix3 result = Matrix3.Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            result.M11 = cos;
            result.M12 = -sin;
            result.M21 = sin;
            result.M22 = cos; 
            return result;
        }
        
        public static void CreateRotation(float radians, out Matrix3 result)
        {
            result = Matrix3.Identity;
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            result.M11 = cos; 
            result.M12 = -sin;
            result.M21 = sin;
            result.M22 = cos; 
        }

        public static Matrix3 CreateScale(Vector2 scale) 
        {
            Matrix3 result = Matrix3.Identity;
            result.M11 = scale.X;
            result.M22 = scale.Y;
            return result;
        }
        
        public static void CreateScale(ref Vector2 scale, out Matrix3 result) 
        {
            result = Matrix3.Identity;
            result.M11 = scale.X;
            result.M22 = scale.Y;
        }

        public static void Multiply(ref Matrix3 a, ref Matrix3 b, out Matrix3 c)
        {
            c = Matrix3.Identity;
            c.M11 = a.M11 * b.M11 + a.M21 * b.M12 + a.M31 * b.M13; 
            c.M12 = a.M12 * b.M11 + a.M22 * b.M12 + a.M32 * b.M13; 
            c.M13 = a.M13 * b.M11 + a.M23 * b.M12 + a.M33 * b.M13; 

            c.M21 = a.M11 * b.M21 + a.M21 * b.M22 + a.M31 * b.M23;
            c.M22 = a.M12 * b.M21 + a.M22 * b.M22 + a.M32 * b.M23;
            c.M23 = a.M13 * b.M21 + a.M23 * b.M22 + a.M33 * b.M23;

            c.M31 = a.M11 * b.M31 + a.M21 * b.M32 + a.M31 * b.M33;
            c.M32 = a.M12 * b.M31 + a.M22 * b.M32 + a.M32 * b.M33;
            c.M33 = a.M13 * b.M31 + a.M23 * b.M32 + a.M33 * b.M33;
        }

        public static Matrix3 operator *(Matrix3 a, Matrix3 b) 
        {
            Matrix3 c = Matrix3.Identity;
            c.M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31; 
            c.M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32;
            c.M13 = a.M11 * b.M13 + a.M12 * b.M32 + a.M13 * b.M33;

            c.M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31; 
            c.M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32;
            c.M23 = a.M21 * b.M13 + a.M22 * b.M32 + a.M23 * b.M33;

            c.M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31; 
            c.M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32;
            c.M33 = a.M31 * b.M13 + a.M32 * b.M32 + a.M33 * b.M33;
            return c;
        }

        public void TransformVector(ref Vector2 v)
        {
            /*
             *      | M11 M12 M13 |   |Vx|
             *      | M21 M22 M23 | x |Vy|
             *      | M31 M32 M33 |   |1 |
             */
            float x = v.X;
            float y = v.Y;
            v.X = M11 * x + M12 * y + M31;
            v.Y = M21 * x + M22 * y + M32;
        }

        public bool Equals(Matrix3 other)
        {
            return M11 == other.M11 && M12 == other.M12 && M13 == other.M13 &&
                   M21 == other.M21 && M22 == other.M22 && M23 == other.M23 &&
                   M31 == other.M31 && M32 == other.M32 && M33 == other.M33;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Matrix3)
                result = Equals((Matrix3)obj);
            return result;
        }

        public override int GetHashCode()
        {
            return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() +
                   M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() +
                   M31.GetHashCode() + M23.GetHashCode() + M33.GetHashCode();
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Concat(new string[]
			{
				"{ ",
				string.Format(currentCulture, "{{M11:{0} M12:{1} M13:{2}}} ", new object[]
				{
					this.M11.ToString(currentCulture),
					this.M12.ToString(currentCulture),
					this.M13.ToString(currentCulture),
				}),
				string.Format(currentCulture, "{{M21:{0} M22:{1} M23:{2}}} ", new object[]
				{
					this.M21.ToString(currentCulture),
					this.M22.ToString(currentCulture),
					this.M23.ToString(currentCulture),
				}),
				string.Format(currentCulture, "{{M31:{0} M32:{1} M33:{2}}} ", new object[]
				{
					this.M31.ToString(currentCulture),
					this.M32.ToString(currentCulture),
					this.M33.ToString(currentCulture),
				}),
				"}"
			});
        }

    }
}
