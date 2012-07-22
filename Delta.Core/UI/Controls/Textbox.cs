using System;
using Microsoft.Xna.Framework.Input;

namespace Delta.UI.Controls
{
    public class Textbox : Label
    {

        public Textbox()
            : base()
        {
        }

        protected override void LightUpdate(DeltaTime time)
        {
            base.LightUpdate(time);
            //if (G.Input.Keyboard.IsPressed(Keys.Back) || G.Input.Keyboard.IsDown(Keys.Back, 0.6f));
            //{
            //    if (Text.Length > 0)
            //        Text.Remove(Text.Length - 1, 1);
            //}
            G.Input.Keyboard.GetHeldKeys(AddKey);
        }

        void AddKey(Keys key)
        {
            //if (G.Input.Keyboard.IsPressed(key) || G.Input.Keyboard.IsDown(key, 0.6f))
            //{
            //    char charKey;
            //    switch (key)
            //    {
            //        case Keys.Escape:
            //        case Keys.Back:
            //            return;
            //        default:
            //            if (G.Input.Keyboard.TryGetKeyChar(key, out charKey))
            //            {
            //                Text.Append(charKey);
            //                Invalidate();
            //            }
            //            return;
            //    }
            //}
        }

    }
}
