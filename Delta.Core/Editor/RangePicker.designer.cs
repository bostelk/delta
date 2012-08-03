namespace Delta.Editor
{
    partial class RangePicker
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
            this.progressBar1 = new Delta.Editor.SliderBar();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(200, 19);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.BarChanged += new Delta.Editor.SliderBar.BarChangedEventHandler(this.rangePicker_BarChanged);
            // 
            // RangePicker
            // 
            this.Controls.Add(this.progressBar1);
            this.Name = "RangePicker";
            this.Size = new System.Drawing.Size(200, 19);
            this.ResumeLayout(false);

        }

        #endregion

        private SliderBar progressBar1;

    }
}