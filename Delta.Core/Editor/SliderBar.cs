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
    public partial class SliderBar : GraphicsDeviceControl
    {
        bool _isMouseDown = false;
        ResourceContentManager _embedded;
        BasicEffect Effect;
        SpriteBatch SpriteBatch;
		VertexPositionColor[] QuadVertexBuffer = new VertexPositionColor[4];

        public class BarChangedEventArgs : EventArgs
        {
            public Range Range;
        }

        public delegate void BarChangedEventHandler(Object sender, BarChangedEventArgs e);
        public event BarChangedEventHandler BarChanged;

        float _min;
        public float Min
        {
            get { return _min; }
            set { _min = Math.Min(value, Max); UpdateQuad(); }
        }

        float _max;
        public float Max
        {
            get { return _max; }
            set { _max = Math.Max(value, Min); UpdateQuad(); }
        }

        float _lower;
        public float Lower
        {
            get { return _lower; }
            set { _lower = MathHelper.Clamp(value, Min, Upper); UpdateQuad(); }
        }

        float _upper;
        public float Upper
        {
            get { return _upper; }
            set { _upper = MathHelper.Clamp(value, Lower, Max); UpdateQuad(); }
        }

        public float Step;

        SpriteFont font;

        Microsoft.Xna.Framework.Color StartColor = Microsoft.Xna.Framework.Color.DarkGreen;
        Microsoft.Xna.Framework.Color EndColor = Microsoft.Xna.Framework.Color.LightGreen;
        Microsoft.Xna.Framework.Color FontColor = Microsoft.Xna.Framework.Color.White;

        public SliderBar()
        {
            Min = -20;
            Max = 20;
            Step = 1;
            InitializeComponent();
        }

        protected override void GDCInitialize()
        {
            _embedded = new ResourceContentManager(Services, EmbeddedContent.ResourceManager);
            font = _embedded.Load<SpriteFont>("TinyFont");

            Effect = new BasicEffect(GraphicsDevice);
            Effect.VertexColorEnabled = true;

            SpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            UpdateQuad();

            base.GDCInitialize();
        }

        protected override void GDCDraw()
        {
            try
            {
                GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);

                Effect.View = Matrix.Identity;
                Effect.Projection = Matrix.CreateOrthographicOffCenter(0, Width, 0, Height, 0, 1f);
                Effect.CurrentTechnique.Passes[0].Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, QuadVertexBuffer, 0, 2);

                SpriteBatch.Begin();
                SpriteBatch.DrawString(font, String.Format("{0:0.##}:{1:0.##}", Lower, Upper), new Vector2(Width / 2, Height / 2), FontColor, TextAlignment.Center | TextAlignment.Middle);
                SpriteBatch.End();
                base.GDCDraw();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        Vector2 TransformMouse(Vector2 position)
        {
            position = position.Clamp(Vector2.Zero, new Vector2(Width, Height));
            // 0,0 is now at the center
            position -= new Vector2(Width / 2, Height / 2);
            return position;
        }

        void CalculateValue(Vector2 position)
        {
            position = TransformMouse(position);
            float xper = position.X / (Width / 2);
            float value = xper * Max;
            
            // move the end that the cursor is closest to.
            float v = 1 + (value / Max);
            float near = 1 + (Lower / Max);
            float far = 1 + (Upper / Max);
            if (Math.Abs(v - near) < Math.Abs(v - far))
                Lower = value;
            else
                Upper = value;
        }

        void UpdateQuad()
        {
            float nearEnd = (1 + (Lower / Max)) * Width / 2;
            float farEnd = (1 + (Upper / Max)) * Width / 2;
            QuadVertexBuffer[0] = new VertexPositionColor(new Vector3(nearEnd, 0, 0), StartColor);
            QuadVertexBuffer[1] = new VertexPositionColor(new Vector3(nearEnd, Height, 0), StartColor);
            QuadVertexBuffer[2] = new VertexPositionColor(new Vector3(farEnd, 0, 0), Microsoft.Xna.Framework.Color.Lerp(StartColor, EndColor, Upper / Max));
            QuadVertexBuffer[3] = new VertexPositionColor(new Vector3(farEnd, Height, 0), Microsoft.Xna.Framework.Color.Lerp(StartColor, EndColor, Upper / Max));

        }

        private void SliderBar_MouseDown(object sender, MouseEventArgs e)
        {
            CalculateValue(new Vector2(e.X, e.Y));
            if (BarChanged != null)
            {
                BarChanged(this, new BarChangedEventArgs
                {
                    Range = new Range(Lower, Upper)
                });
            }
            _isMouseDown = true;
            Invalidate();
        }

        private void SliderBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                CalculateValue(new Vector2(e.X, e.Y));
                if (BarChanged != null)
                {
                    BarChanged(this, new BarChangedEventArgs
                    {
                        Range = new Range(Lower, Upper)
                    });
                }
            }
            Invalidate();
        }

        private void SliderBar_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }

    }
}
