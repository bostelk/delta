using System;

namespace Delta.UI.Controls
{
    public class TweakerLabel : Label
    {
        ICustomizable CustomizableObject { get; set; }

        public TweakerLabel()
            : base()
        {
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            base.LightUpdate(time);
            if (CustomizableObject != null)
            {
                Text.Clear();
                Text.Append("Object: ");
            }
        }

    }
}
