using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Delta.Editor
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        private Vector4 _color;

        public Vector4 SelectedColor {
            get
            {
                return _color;
            }
            set
            {
                SelectColor(value);
            }
        }

        private void SelectColor(Vector4 C)
        {
            _color = C;

            this.redText.Text = ( (int)( C.X * 255 ) ).ToString();
            this.greenText.Text = ( (int)( C.Y * 255 ) ).ToString();
            this.blueText.Text = ( (int)( C.Z * 255 ) ).ToString();
            this.alphaText.Text = "255";

            SetColorPreivew();
            this.Invalidate();
        }

        private void SetColorPreivew()
        {
            this.colorPreview.BackColor = System.Drawing.Color.FromArgb(255, (int)( _color.X * 255 ),
                (int)( _color.Y * 255 ), (int)( _color.Z * 255 ));
        }

        private void colorWheel_ColorPicked(object sender, ColorWheel.ColorPickedEventArgs e)
        {
            SelectColor(e.Color);
            this.Invalidate();
        }

        private void redText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _color.X = Convert.ToSingle(redText.Text) / 255.0f;
            }
            catch (Exception) { }
            SetColorPreivew();
        }

        private void greenText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _color.Y = Convert.ToSingle(greenText.Text) / 255.0f;
            }
            catch (Exception) { } 
            SetColorPreivew();
        }

        private void blueText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _color.Z = Convert.ToSingle(blueText.Text) / 255.0f;
            }
            catch (Exception) { }
            SetColorPreivew();
        }

        private void alphaText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _color.W = Convert.ToSingle(alphaText.Text) / 255.0f;
            }
            catch (Exception) { }
        }

    }
}
