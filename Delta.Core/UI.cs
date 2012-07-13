using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class UI : EntityCollection
    {
        static DeltaTime _time = new DeltaTime();

        public static Camera Camera { get; private set; }
        public static DeltaTime Time { get { return _time; } }

        public UI()
        {
            Camera = new Camera();
        }

        internal virtual void Update(GameTime gameTime)
        {
            _time.IsRunningSlowly = gameTime.IsRunningSlowly;
            _time.ElapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time.TotalSeconds += _time.ElapsedSeconds;
            Camera.Update(_time);
            base.Update(_time);
        }

        internal virtual void Draw()
        {
            G.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.View);
            base.Draw(_time, G.SpriteBatch);
            Camera.Draw(_time, G.SpriteBatch);
#if DEBUG
            G.SpriteBatch.DrawRectangle(Camera.ViewingArea, Color.Gray, false);
#endif
            G.SpriteBatch.End();
        }
    }
}
