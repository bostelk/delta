using System;
using Microsoft.Xna.Framework;
using Delta.UI.Controls;

namespace Delta.UI
{
    public class DebugScreen : BaseScreen
    {
        PerformanceMetrics _performanceMetrics = new PerformanceMetrics();

        public DebugScreen()
            : base()
        {
            _performanceMetrics.ForeColor = Color.White;
            _performanceMetrics.BackColor = Color.Gray;
            Add(_performanceMetrics);
            Label lbl = new Label() { BackColor = Color.Gray, Position = new Vector2(900, 50), Size = new Vector2(300, 200), AutoSize = false, IsWordWrapped = true};
            lbl.Text.Append("*yawn* \n//Lucas's To Do List:\n1. Don't append any render text beyond the border.\n2. Clip each control by starting a new SpriteBatch.Begin() for each control.\n3. Ignore Textbox for now, lol.\n4. Screen transitions.");
            lbl.Invalidate();
            Add(lbl);
            //Add(new Controls.Textbox() { BackColor = Color.Gray, Position = new Vector2(50, 50), Size = new Vector2(100, 100), AutoSize = false, VerticalTextAlignment = Controls.VerticalTextAlignment.Center, HorizontalTextAlignment = Controls.HorizontalTextAlignment.Center, WordWrap = true});
        }
    }
}
