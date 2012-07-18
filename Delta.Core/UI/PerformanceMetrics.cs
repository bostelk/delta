using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Delta.UI
{

    public class PerformanceMetrics : Control
    {
        Stopwatch _stopwatch;
        StringBuilder _stringBuilder;
        int _frames;

        public Color Color { get; set; }

        public float FPS { get; set; }
        public float ManagedMemory { get; set; }

        public PerformanceMetrics()
        {
            _stringBuilder = new StringBuilder(4);
            _stopwatch = new Stopwatch();

            FPS = 0;
            Color = Color.White;
        }

        public override void LoadContent()
        {
            _stopwatch.Start();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (_stopwatch.Elapsed > TimeSpan.FromSeconds(1))
            {
                FPS = (int)(_frames / _stopwatch.Elapsed.TotalSeconds);
                ManagedMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                _stopwatch.Restart();
                _frames = 0;

                // let's not generate garbage... hint: strings are immutable
                _stringBuilder.Length = 0;
                _stringBuilder.Append("FPS: ");
                _stringBuilder.Append((int)FPS);
                _stringBuilder.Append("\nMEM: ");
                _stringBuilder.Append(ManagedMemory);
                _stringBuilder.Append(" mb");
                //_stringBuilder.Append("\nTEX MEM: ");
                //_stringBuilder.Append("??");
                //_stringBuilder.Append(" mb");
            }
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(G.Font, _stringBuilder, Position, Color);
            _frames++;
        }
    }
}
