using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class UI : EntityParent<IEntity>, IGameComponent, Microsoft.Xna.Framework.IUpdateable, IDrawable
    {
        public static UI Instance { get; set; }

        public float TimeScale { get; set; }
        public WorldTime Time;
        public Camera Camera { get; set; }

        public UI()
        {
            Instance = this;
            Camera = new Camera();
            TimeScale = 1.0f;
        }

        void IGameComponent.Initialize()
        {
        }

        bool Microsoft.Xna.Framework.IUpdateable.Enabled
        {
            get { return IsEnabled; }
        }


        int Microsoft.Xna.Framework.IUpdateable.UpdateOrder
        {
            get { return 0; }
        }

        event EventHandler<EventArgs> Microsoft.Xna.Framework.IUpdateable.UpdateOrderChanged
        {
            add { }
            remove { }
        }

        event EventHandler<EventArgs> Microsoft.Xna.Framework.IUpdateable.EnabledChanged
        {
            add { }
            remove { }
        }

        bool IDrawable.Visible
        {
            get { return IsVisible; }
        }

        int IDrawable.DrawOrder
        {
            get { return 0; }
        }

        event EventHandler<EventArgs> IDrawable.DrawOrderChanged
        {
            add { }
            remove { }
        }

        event EventHandler<EventArgs> IDrawable.VisibleChanged
        {
            add { }
            remove { }
        }

        void Microsoft.Xna.Framework.IUpdateable.Update(GameTime gameTime)
        {
            InternalUpdate(gameTime);
        }

        void IDrawable.Draw(GameTime gameTime)
        {
            InternalDraw(gameTime, G.SpriteBatch);
        }

        protected internal override void BeginUpdate(GameTime gameTime)
        {
            Time.ElapsedWorldSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds * TimeScale;
            Time.TotalWorldSeconds += Time.ElapsedWorldSeconds;
            Camera.Update(gameTime);
            base.BeginUpdate(gameTime);
        }

        protected override void BeginDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.View);
            base.BeginDraw(gameTime, spriteBatch);
        }

        protected internal override void EndDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.EndDraw(gameTime, spriteBatch);
            Camera.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
