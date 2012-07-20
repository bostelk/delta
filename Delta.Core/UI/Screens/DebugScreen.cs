using System;
using Microsoft.Xna.Framework;
using Delta.UI.Controls;

namespace Delta.UI
{
    public class DebugScreen : Screen
    {
        PerformanceMetrics _performanceMetrics = new PerformanceMetrics();

        public DebugScreen()
            : base()
        {
            _performanceMetrics.ForeColor = Color.White;
            _performanceMetrics.BackColor = Color.Gray;
            Add(_performanceMetrics);
            Label lbl = new Label() { BackColor = Color.Gray, Position = new Vector2(50, 50), Size = new Vector2(300, 200), AutoSize = false, IsWordWrapped = true};
            lbl.Text.Append("Hi, this is a really long string that I hope gets wrapped properly when I test it. \n Custom new line. \n Kyle seems made at me cause I'm taking forever to get this done. Whatever, I don't care. I have to go through and clip each control by it's bounding area so text will never be rendered outside the control's bounds. For now, it's good enough. I'll look into it more in alittle bit.");
            lbl.Invalidate();
            Add(lbl);
            //Add(new Controls.Textbox() { BackColor = Color.Gray, Position = new Vector2(50, 50), Size = new Vector2(100, 100), AutoSize = false, VerticalTextAlignment = Controls.VerticalTextAlignment.Center, HorizontalTextAlignment = Controls.HorizontalTextAlignment.Center, WordWrap = true});
        }
    }
}
