using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Input;

namespace Delta
{
    public class World : EntityParent<IEntity>
    {
        public static World Instance { get; set; }

        public float TimeScale { get; set; }
        public WorldTime Time;
        public Camera Camera { get; set; }

        public World()
        {
            Instance = this;
            Camera = new Camera();
            TimeScale = 1.0f;
        }

        protected internal override void BeginUpdate(GameTime gameTime)
        {
            Time.ElapsedWorldSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            Time.TotalWorldSeconds += Time.ElapsedWorldSeconds;
            Camera.InternalUpdate(gameTime);
            base.BeginUpdate(gameTime);
        }

        protected override void BeginDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.View);
            base.BeginDraw(gameTime, spriteBatch);
        }

        protected internal override void EndDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
#if DEBUG
            spriteBatch.DrawRectangle(Camera.ViewingArea, Color.Gray, false);
#endif
            base.EndDraw(gameTime, spriteBatch);
            Camera.Draw(gameTime, spriteBatch);
            spriteBatch.End();
#if DEBUG
            spriteBatch.Begin();
            spriteBatch.DrawString(G.Font, String.Format("Mouse: {0} . Viewing: {1}", Camera.ToWorldPosition(G.Input.Mouse.Position).ToString(), Camera.ViewingArea), new Vector2(0, G.ScreenArea.Bottom), Color.Red, TextAlignment.Bottom);
            spriteBatch.End();
#endif
        }
    }
}
