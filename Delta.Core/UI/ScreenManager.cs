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
        public Screen FocusedScreen { get; internal set; }

        public ScreenManager()
            : base()
        {
#if DEBUG
            Add(new DebugScreen());
#endif
        }
    }
}
