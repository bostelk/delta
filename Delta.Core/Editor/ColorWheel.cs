using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Delta.Editor
{
    public partial class ColorWheel : GraphicsDeviceControl
    {
        public class ColorPickedEventArgs : EventArgs
        {
            public Vector4 Color;
        }

        public delegate void ColorPickedEventHandler(Object sender, ColorPickedEventArgs e);
        public event ColorPickedEventHandler ColorPicked;

        struct VertexFormat : IVertexType
        {
            public Vector3 Position;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)
            );

            VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

            public static int SizeInBytes = sizeof(float) * 3;
        }

        ContentManager Content;
        Effect Effect;
		VertexFormat[] QuadVertexBuffer = new VertexFormat[4];
        VertexFormat[] TriangleBuffer = new VertexFormat[3];
        Vector3[] Hues = new Vector3[7];
        float SelectedHue = 0.0f;
        float HueStepRadians = (float)((System.Math.PI * 2.0f) / 6.0f);
        Vector3 SelectedColor = new Vector3(1, 0, 0);

        public ColorWheel()
        {
            InitializeComponent();
        }

        protected override void GDCInitialize()
        {
            Content = new ContentManager(Services);
            Content.RootDirectory = "Content";

            Effect = Content.Load<Effect>("ColorWheel");

            QuadVertexBuffer[0].Position = new Vector3(-1, -1, 0);
            QuadVertexBuffer[1].Position = new Vector3(-1, 1, 0);
            QuadVertexBuffer[2].Position = new Vector3(1, -1, 0);
            QuadVertexBuffer[3].Position = new Vector3(1, 1, 0);

            Hues[0] = new Vector3(1, 0, 0);
            Hues[1] = new Vector3(1, 1, 0);
            Hues[2] = new Vector3(0, 1, 0);
            Hues[3] = new Vector3(0, 1, 1);
            Hues[4] = new Vector3(0, 0, 1);
            Hues[5] = new Vector3(1, 0, 1);
            Hues[6] = new Vector3(1, 0, 0);

            base.GDCInitialize();
        }

        bool IsOnHueWheel(Vector2 Point)
        {
            float Length = Point.Length();
            return Length > 0.7 && Length <= 0.9;
        }

        float CrossZ(Vector2 A, Vector2 B)
        {
            return ( B.Y * A.X ) - ( B.X * A.Y );
        }

        float AngleBetweenVectors(Vector2 A, Vector2 B)
        {
            A.Normalize();
            B.Normalize();
            float Dot = Vector2.Dot(A, B);
            Dot = MathHelper.Clamp(Dot, -1.0f, 1.0f);
            float Angle = (float)System.Math.Acos(Dot);
            if (CrossZ(A, B) < 0) return ( 2.0f * (float)System.Math.PI ) - Angle;
            else return Angle;
        }
        
        float GetHue(Vector2 Point)
        {
            float Angle = AngleBetweenVectors(Point, new Vector2(1, 0));
            if (Angle < 0) Angle = ( 2.0f * (float)System.Math.PI ) + Angle;

            Angle /= HueStepRadians;
            return Angle;
        }

        float GetHueAngle(float Hue)
        {
            return Hue * HueStepRadians;
        }

        Vector3 GetColorFromHue(float Hue)
        {
            int BaseHue = (int)Hue;
            float OffsetHue = Hue - BaseHue;

            return Hues[BaseHue] * ( 1.0f - OffsetHue ) + Hues[BaseHue + 1] * OffsetHue;
        }

        void CalculateShadeTrianglePoints(float Hue)
        {
            Vector3 Point = new Vector3(0.7f, 0, 0);

            TriangleBuffer[0].Position = Vector3.Transform(Point, Matrix.CreateRotationZ(-GetHueAngle(Hue)));
            TriangleBuffer[1].Position = Vector3.Transform(Point, Matrix.CreateRotationZ(-GetHueAngle(Hue + 2)));
            TriangleBuffer[2].Position = Vector3.Transform(Point, Matrix.CreateRotationZ(-GetHueAngle(Hue + 4)));
        }

        bool IsOnShadeTriangle(Vector2 Pos)
        {
            CalculateShadeTrianglePoints(SelectedHue);
            Vector2[] LocalPoints = new Vector2[3] 
            {
                new Vector2(TriangleBuffer[0].Position.X, TriangleBuffer[0].Position.Y),
                new Vector2(TriangleBuffer[1].Position.X, TriangleBuffer[1].Position.Y),
                new Vector2(TriangleBuffer[2].Position.X, TriangleBuffer[2].Position.Y),
            };

            if (CrossZ(LocalPoints[1] - LocalPoints[0], Pos - LocalPoints[0]) > 0) return false;
            if (CrossZ(LocalPoints[2] - LocalPoints[1], Pos - LocalPoints[1]) > 0) return false;
            if (CrossZ(LocalPoints[0] - LocalPoints[2], Pos - LocalPoints[2]) > 0) return false;
            return true;
        }

        Vector3 ApplyShade(Vector2 Pos, float Hue)
        {
            float ChordLength = ( 2.0f * 0.7f * (float)System.Math.Sin(( (float)System.Math.PI * 2.0f / 3.0f ) / 2.0f) ) - 0.2f;
            CalculateShadeTrianglePoints(Hue);
            Vector3 Color = GetColorFromHue(Hue);

            Vector2 HuePoint = new Vector2(TriangleBuffer[0].Position.X, TriangleBuffer[0].Position.Y);
            Vector2 WhitePoint = new Vector2(TriangleBuffer[2].Position.X, TriangleBuffer[2].Position.Y);


            float HueDistance = ( ( HuePoint - Pos ).Length() / ChordLength ) - 0.1f;
            HueDistance = MathHelper.Clamp(HueDistance, 0, 1);

            Vector3 Result = Color * (1.0f - HueDistance );

            float WhiteDistance = ( ( WhitePoint - Pos ).Length() / ChordLength ) - 0.1f;
            WhiteDistance = MathHelper.Clamp(WhiteDistance, 0, 1);

            Result += new Vector3(1, 1, 1) * ( 1.0f - WhiteDistance );

            Result.X = MathHelper.Clamp(Result.X, 0, 1); 
            Result.Y = MathHelper.Clamp(Result.Y, 0, 1); 
            Result.Z = MathHelper.Clamp(Result.Z, 0, 1);

            return Result;
        }

        protected override void GDCDraw()
        {
            try
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);

                Effect.CurrentTechnique = Effect.Techniques["DrawHueWheel"];
                Effect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, QuadVertexBuffer, 0, 2);

                CalculateShadeTrianglePoints(SelectedHue);

                Effect.Parameters["HuePoint"].SetValue(new Vector2(TriangleBuffer[0].Position.X, TriangleBuffer[0].Position.Y));
                Effect.Parameters["BlackPoint"].SetValue(new Vector2(TriangleBuffer[1].Position.X, TriangleBuffer[1].Position.Y));
                Effect.Parameters["WhitePoint"].SetValue(new Vector2(TriangleBuffer[2].Position.X, TriangleBuffer[2].Position.Y));
                Effect.Parameters["Hue"].SetValue(SelectedHue);

                Effect.CurrentTechnique = Effect.Techniques["DrawShadeTriangle"];
                Effect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, TriangleBuffer, 0, 1);


                base.GDCDraw();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        Vector2 TransformMouseCoord(Vector2 Coord)
        {
            Coord.Y = Height - Coord.Y;
            Coord /= new Vector2(Width, Height);
            Coord *= 2.0f;
            Coord -= new Vector2(1, 1);
            return Coord;
        }

        bool IsMouseDown = false;

        void CalculateValue(Vector2 Pos)
        {
            Pos = TransformMouseCoord(Pos);
            if (IsOnHueWheel(Pos))
            {
                SelectedHue = GetHue(Pos);
                SelectedColor = GetColorFromHue(SelectedHue);
            }
            else if (IsOnShadeTriangle(Pos))
                SelectedColor = ApplyShade(Pos, SelectedHue);
            
        }

        private void ColorWheel_MouseDown(object sender, MouseEventArgs e)
        {
            CalculateValue(new Vector2(e.X, e.Y));
            if (ColorPicked != null) ColorPicked(this, new ColorPickedEventArgs
            {
                Color = new Vector4(SelectedColor, 1)
            });
            IsMouseDown = true;
            Invalidate();
        }

        private void ColorWheel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                CalculateValue(new Vector2(e.X, e.Y));
                if (ColorPicked != null) ColorPicked(this, new ColorPickedEventArgs
                {
                    Color = new Vector4(SelectedColor, 1)
                });
            }
            Invalidate();
        }

        private void ColorWheel_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }
    }
}
