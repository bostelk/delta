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
    public partial class RangePicker : UserControl
    {
        public RangePicker()
        {
            InitializeComponent();
        }

        private Range _range;
        public Range SelectedRange 
        {
            get
            {
                return _range;
            }
            set
            {
                SelectRange(value);
            }
        }

        void SelectRange(Range value)
        {
            _range = value;
            progressBar1.Lower = _range.Lower;
            progressBar1.Upper = _range.Upper;
            this.Invalidate();
        }

        public void rangePicker_BarChanged(object sender, SliderBar.BarChangedEventArgs e)
        {
            _range = e.Range;
            this.Invalidate();
        }

    }
}
