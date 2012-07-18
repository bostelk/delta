using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Delta;
using Microsoft.Xna.Framework.Content;

namespace Delta.Examples
{

    //public class PerformanceMetrics : EntityBase
    //{
    //    Stopwatch _stopwatch;
    //    StringBuilder _stringBuilder;
    //    int _frames;

    //    public SpriteFont Font
    //    {
    //        get;
    //        set;
    //    }

    //    public Color Color
    //    {
    //        get;
    //        set;
    //    }

    //    /// <summary>
    //    /// The Frames per second.
    //    /// </summary>
    //    public float Fps
    //    {
    //        get;
    //        private set;
    //    }

    //    /// <summary>
    //    /// The Managed Memory in MB.
    //    /// </summary>
    //    public float ManagedMemory
    //    {
    //        get;
    //        private set;
    //    }

    //    public PerformanceMetrics() 
    //    {
    //        _stringBuilder = new StringBuilder(4);
    //        _stopwatch = new Stopwatch();

    //        Fps = 0;
    //        Color = Color.White;
    //    }

    //    public override void LoadContent()
    //    {
    //        Font = G.Font;
    //        _stopwatch.Start();
    //    }

    //    protected override void LightUpdate(DeltaTime time)
    //    {
    //        if (_stopwatch.Elapsed > TimeSpan.FromSeconds(1))
    //        {
    //            Fps = (float)_frames / (float)_stopwatch.Elapsed.TotalSeconds;
    //            ManagedMemory = (float)Math.Round((double)((float)(GC.GetTotalMemory(false) / 1024L) / 1024f), 4);

    //            _stopwatch.Reset();
    //            _stopwatch.Start();
    //            _frames = 0;

    //            // let's not generate garbage... hint: strings are immutable
    //            _stringBuilder.Length = 0;
    //            _stringBuilder.Append("FPS: ");
    //            _stringBuilder.Append((int)Fps);
    //            _stringBuilder.Append("\nMEM: ");
    //            _stringBuilder.Append(ManagedMemory);
    //            _stringBuilder.Append(" mb");
    //            //_stringBuilder.Append("\nTEX MEM: ");
    //            //_stringBuilder.Append("??");
    //            //_stringBuilder.Append(" mb");
    //        }
    //    }

    //    protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
    //    {
    //        Font = G.Font; // loadcontent isn't called yet; not attached to a world.
    //        spriteBatch.DrawString(Font, _stringBuilder, Vector2.Zero, Color);
    //        _frames++;
    //    }
    //}
}
