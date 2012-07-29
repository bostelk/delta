using System;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI.Screens
{
    public abstract class Transition
    {
        public Transition()
            : base()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalUpdate(DeltaGameTime time)
        {
            OnBeginUpdate(time);
            Update(time);
            OnEndUpdate(time);
        }

        protected virtual void Update(DeltaGameTime time)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            if (CanDraw())
            {
                OnBeginDraw(time, spriteBatch);
                Draw(time, spriteBatch);
                OnEndDraw(time, spriteBatch);
            }
        }

        protected virtual void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
        }

        protected virtual void OnBeginUpdate(DeltaGameTime time)
        {
        }

        protected virtual void OnEndUpdate(DeltaGameTime time)
        {
        }

        protected virtual void OnBeginDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
        }

        protected virtual void OnEndDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
        }

    }
}
