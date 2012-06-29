using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Delta.Input
{
    public class KeyboardHelper
    {
        KeyboardState _thisState;
        KeyboardState _previousState;

        public KeyboardHelper()
        {
            _thisState = _previousState = Keyboard.GetState();
        }

        public bool JustPressed(Keys key)
        {
            return _thisState.IsKeyDown(key) && _previousState.IsKeyUp(key);
        }

        public bool JustReleased(Keys key)
        {
            return _thisState.IsKeyUp(key) && _previousState.IsKeyDown(key);
        }

        public bool Held(Keys key)
        {
            return _thisState.IsKeyDown(key);
        }

        public void Update(GameTime gameTime)
        {
            _previousState = _thisState;
            _thisState = Keyboard.GetState();
        }
    }
}
