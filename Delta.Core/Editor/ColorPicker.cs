using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Delta.Extensions;

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

        private void SelectColor(Vector4 value)
        {
            _color = value;

            this.colorHex.Text = ColorExtensions.ToHexadecimal(new Microsoft.Xna.Framework.Color(value), false);

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

        private void colorHex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _color = ColorExtensions.ToColor(colorHex.Text).ToVector4();
            }
            catch (Exception) { }
            SetColorPreivew();
        }

    }
}
