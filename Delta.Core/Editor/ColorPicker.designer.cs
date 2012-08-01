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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.redText = new System.Windows.Forms.TextBox();
            this.alphaText = new System.Windows.Forms.TextBox();
            this.blueText = new System.Windows.Forms.TextBox();
            this.greenText = new System.Windows.Forms.TextBox();
            this.colorPreview = new System.Windows.Forms.PictureBox();
            this.colorWheel = new Delta.Editor.ColorWheel();
            ((System.ComponentModel.ISupportInitialize)(this.colorPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Green";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(176, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Blue";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(176, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Red";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(176, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Alpha";
            // 
            // redText
            // 
            this.redText.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redText.Location = new System.Drawing.Point(224, 14);
            this.redText.MaxLength = 3;
            this.redText.Name = "redText";
            this.redText.Size = new System.Drawing.Size(46, 23);
            this.redText.TabIndex = 6;
            this.redText.TextChanged += new System.EventHandler(this.redText_TextChanged);
            // 
            // alphaText
            // 
            this.alphaText.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaText.Location = new System.Drawing.Point(224, 111);
            this.alphaText.MaxLength = 3;
            this.alphaText.Name = "alphaText";
            this.alphaText.Size = new System.Drawing.Size(46, 23);
            this.alphaText.TabIndex = 7;
            this.alphaText.TextChanged += new System.EventHandler(this.alphaText_TextChanged);
            // 
            // blueText
            // 
            this.blueText.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blueText.Location = new System.Drawing.Point(224, 79);
            this.blueText.MaxLength = 3;
            this.blueText.Name = "blueText";
            this.blueText.Size = new System.Drawing.Size(46, 23);
            this.blueText.TabIndex = 8;
            this.blueText.TextChanged += new System.EventHandler(this.blueText_TextChanged);
            // 
            // greenText
            // 
            this.greenText.Font = new System.Drawing.Font("Segoe UI Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.greenText.Location = new System.Drawing.Point(224, 47);
            this.greenText.MaxLength = 3;
            this.greenText.Name = "greenText";
            this.greenText.Size = new System.Drawing.Size(46, 23);
            this.greenText.TabIndex = 9;
            this.greenText.TextChanged += new System.EventHandler(this.greenText_TextChanged);
            // 
            // colorPreview
            // 
            this.colorPreview.InitialImage = null;
            this.colorPreview.Location = new System.Drawing.Point(180, 140);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.colorPreview);
            this.Controls.Add(this.greenText);
            this.Controls.Add(this.blueText);
            this.Controls.Add(this.alphaText);
            this.Controls.Add(this.redText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colorWheel);
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(282, 170);
            ((System.ComponentModel.ISupportInitialize)(this.colorPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorWheel colorWheel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox redText;
        private System.Windows.Forms.TextBox alphaText;
        private System.Windows.Forms.TextBox blueText;
        private System.Windows.Forms.TextBox greenText;
        private System.Windows.Forms.PictureBox colorPreview;
    }
}