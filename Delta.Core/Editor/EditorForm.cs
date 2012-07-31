using System;
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

        Delta.Graphics.SpriteEntity entity = new Delta.Graphics.SpriteEntity("Game Object 1", "Sprite 1");

        private void EditorForm_Load(object sender, EventArgs e)
        {
            grdProperty.SelectedObject = entity;
        }
    }
}
