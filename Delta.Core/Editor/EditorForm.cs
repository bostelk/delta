﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Delta.Editor
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    Hide();
                    e.Cancel = true;
                    break;
            }

        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
        }

        private void grdProperty_Enter(object sender, EventArgs e)
        {
            grdProperty.Refresh();
        }
    }
}
