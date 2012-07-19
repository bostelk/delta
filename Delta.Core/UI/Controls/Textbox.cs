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
            if (G.Input.Keyboard.IsDown(Keys.Back))
            {
                if (Text.Length > 0)
                    Text.Remove(Text.Length - 1, 1);
            }
            else
                G.Input.Keyboard.GetPressedKeys(AddKey);
        }

        void AddKey(Keys key)
        {
            char charKey;
            if (G.Input.Keyboard.TryGetKeyChar(key, out charKey))
            {
                Text.Append(charKey);
                UpdateTextSize();
                UpdateRenderSize();
                UpdateTextPosition();
            }
        }

    }
}
