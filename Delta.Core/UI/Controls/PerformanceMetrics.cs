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
        int _frames = 0;
        long _previousManagedMemory = 0;
        long _managedMemory = 0;
        long _deltaManagedMemory = 0;
        float _timer = 0;
        int _fps = 0;

        public PerformanceMetrics()
        {
        }

        protected override void LightUpdate(DeltaTime time)
        {
            _timer += time.ElapsedSeconds;
            if (_timer >= 1)
            {
                _fps = (int)(_frames / _timer);
                _previousManagedMemory = _managedMemory;
                _managedMemory = GC.GetTotalMemory(false);
                _deltaManagedMemory = _managedMemory - _previousManagedMemory;
                _frames = 0;
                _timer = 0;
            }
            Text.Clear();
            Text.Append("FPS: ");
            Text.Concat(_fps);
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
            if (_deltaManagedMemory > 0)
            {
                if (_deltaManagedMemory < 1024)
                {
                    Text.Append(" (+");
                    Text.Concat(_deltaManagedMemory);
                    Text.Append(" B/sec)");
                }
                else if (_deltaManagedMemory < (1024 * 1024))
                {
                    Text.Append(" (+");
                    Text.Concat((float)(_deltaManagedMemory / 1024f), 2);
                    Text.Append(" kB/sec)");
                }
                else if (_deltaManagedMemory < (1024 * 1024 * 1024))
                {
                    Text.Append(" (+");
                    Text.Concat((float)(_deltaManagedMemory / 1024f / 1024f), 2);
                    Text.Append(" mB/sec)");
                }
            }
            Invalidate();
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            _frames++;
        }
    }
}
