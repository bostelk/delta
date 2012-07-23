using System;
using Microsoft.Xna.Framework.Input;

namespace Delta.UI.Controls
{
    public class Textbox : Label
    {
        public Textbox()
            : base()
        {
            AutoSize = false;
            IsWordWrapped = true;
        }

        protected override void OnKeyDown(Keys key)
        {
            Delta.Input.Button button = G.Input.Keyboard.KeyState[key];
            if (button.IsPressed || button.DownDuration >= G.UI.KeyboardDelay)
            {
                switch (key)
                {
                    case Keys.Escape:
                    case Keys.Back:
                        if (Text.Length > 0)
                        {
                            Text.Remove(Text.Length - 1, 1);
                            Invalidate();
                        }
                        break;
                    default:
                        Char charKey;
                        if (G.Input.Keyboard.TryGetKeyChar(key, out charKey))
                        {
                            Text.Append(charKey);
                            Invalidate();
                        }
                        break;
                }
            }
        }
    }
}
