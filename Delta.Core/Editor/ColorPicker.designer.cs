namespace Delta.Editor
{
    partial class ColorPicker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && ( components != null ))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorUnit = new System.Windows.Forms.Label();
            this.colorHex = new System.Windows.Forms.TextBox();
            this.colorPreview = new System.Windows.Forms.PictureBox();
            this.colorWheel = new Delta.Editor.ColorWheel();
            ((System.ComponentModel.ISupportInitialize)(this.colorPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // colorUnit
            // 
            this.colorUnit.AutoSize = true;
            this.colorUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorUnit.Location = new System.Drawing.Point(62, 177);
            this.colorUnit.Name = "colorUnit";
            this.colorUnit.Size = new System.Drawing.Size(14, 15);
            this.colorUnit.TabIndex = 4;
            this.colorUnit.Text = "#";
            // 
            // colorHex
            // 
            this.colorHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.colorHex.Location = new System.Drawing.Point(80, 175);
            this.colorHex.MaxLength = 8;
            this.colorHex.Name = "colorHex";
            this.colorHex.Size = new System.Drawing.Size(65, 21);
            this.colorHex.TabIndex = 6;
            this.colorHex.TextChanged += new System.EventHandler(this.colorHex_TextChanged);
            // 
            // colorPreview
            // 
            this.colorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPreview.InitialImage = null;
            this.colorPreview.Location = new System.Drawing.Point(151, 176);
            this.colorPreview.Name = "colorPreview";
            this.colorPreview.Size = new System.Drawing.Size(19, 17);
            this.colorPreview.TabIndex = 11;
            this.colorPreview.TabStop = false;
            // 
            // colorWheel
            // 
            this.colorWheel.Location = new System.Drawing.Point(0, 0);
            this.colorWheel.Name = "colorWheel";
            this.colorWheel.Size = new System.Drawing.Size(170, 170);
            this.colorWheel.TabIndex = 0;
            this.colorWheel.ColorPicked += new Delta.Editor.ColorWheel.ColorPickedEventHandler(this.colorWheel_ColorPicked);
            // 
            // ColorPicker
            // 
            this.Controls.Add(this.colorPreview);
            this.Controls.Add(this.colorHex);
            this.Controls.Add(this.colorUnit);
            this.Controls.Add(this.colorWheel);
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(200, 201);
            ((System.ComponentModel.ISupportInitialize)(this.colorPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorWheel colorWheel;
        private System.Windows.Forms.Label colorUnit;
        private System.Windows.Forms.TextBox colorHex;
        private System.Windows.Forms.PictureBox colorPreview;
    }
}