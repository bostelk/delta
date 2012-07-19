using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI
{
    public class Screen : EntityCollection<Control>
    {
        public bool IsFocused { get; internal set; }
        public Control FocusedControl { get; internal set; }

        public Screen()
            : base()
        {
        }

        public override void Add(Control item)
        {
            base.Add(item);
            item.Screen = this;
            if (FocusedControl == null)
                FocusedControl = item;
        }

        public override void Remove(Control item)
        {
            item.Screen = null;
            if (FocusedControl == item)
                FocusedControl = null;
            base.Remove(item);
        }

        public void Focus()
        {
            IsFocused = true;
            G.UI.FocusedScreen = this;
        }

        internal void InternalFocusDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            Draw(time, spriteBatch);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            if (FocusedControl != null)
                FocusedControl.InternalFocusDraw(time, spriteBatch);
        }

    }
}
