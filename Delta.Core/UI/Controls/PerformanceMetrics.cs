using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Delta.UI.Controls;

namespace Delta.UI
{

    public class PerformanceMetrics : Label
    {
        Stopwatch _stopwatch;
        int _frames;
        long _managedMemory = 0;

        public int FPS { get; private set; }

        public PerformanceMetrics()
        {
            _stopwatch = new Stopwatch();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _stopwatch.Start();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (_stopwatch.Elapsed > TimeSpan.FromSeconds(1))
            {
                FPS = (int)(_frames / _stopwatch.Elapsed.TotalSeconds);
                _managedMemory = GC.GetTotalMemory(false);
                _stopwatch.Restart();
                _frames = 0;
            }
            Text.Length = 0;
            Text.Append("FPS: ");
            Text.Concat(FPS);
            Text.AppendLine();
            Text.Append("MEM: ");
            if (_managedMemory < 1024)
            {
                Text.Concat(_managedMemory);
                Text.Append(" B");
            }
            else if (_managedMemory < (1024 * 1024))
            {
                Text.Concat((float)(_managedMemory / 1024f), 2);
                Text.Append(" kB");
            }
            else if (_managedMemory < (1024 * 1024 * 1024))
            {
                Text.Concat((float)(_managedMemory / 1024f / 1024f), 2);
                Text.Append(" mB");
            }
            UpdateTextSize();
            UpdateRenderSize();
            UpdateTextPosition();
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            _frames++;
        }
    }
}
