using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI
{
    public class UIManager : EntityManager<Control>
    {
        public Control FocusedControl { get; set; }

        public UIManager()
            : base()
        {
        }
    }
}
