using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.UI
{
    public class BaseScreen : BaseControl
    {
        public override void Add(BaseControl item)
        {
            base.Add(item);
            item.ParentScreen = this;
        }

        public override void Remove(BaseControl item)
        {
            base.Remove(item);
            item.ParentScreen = null;
        }
    }
}
