using System;
using Microsoft.Xna.Framework;

namespace Delta.UI
{
    public class DebugScreen : Screen
    {
        PerformanceMetrics _performanceMetrics = new PerformanceMetrics();

        public DebugScreen()
            : base()
        {
            _performanceMetrics.AutoSize = true;
            _performanceMetrics.Position = new Vector2(200, 200);
            _performanceMetrics.ForeColor = Color.Yellow;
            _performanceMetrics.BackColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Add(_performanceMetrics);
        }
    }
}
