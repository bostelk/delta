using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI
{
    public class ScreenManager : EntityManager<Screen>
    {
#if DEBUG
        public DebugScreen DebugScreen { get; internal set; }
#endif
        public Screen FocusedScreen { get; internal set; }

        public ScreenManager()
            : base()
        {
#if DEBUG
            DebugScreen = new DebugScreen();
#endif
        }

        protected override void LightUpdate(DeltaTime time)
        {
            base.LightUpdate(time);
#if DEBUG
            DebugScreen.InternalUpdate(time);
#endif
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
#if DEBUG
            DebugScreen.InternalDraw(time, spriteBatch);
#endif
        }


    }
}
