using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Delta;

namespace Delta.Input
{
    public class InputManager
    {
        public const int MAX_GAMEPADS = 4;

        MouseHelper _mouse;
        GamePadHelper[] _gamepad;
        KeyboardHelper _keyboard;

        public MouseHelper Mouse
        {
            get
            {
                return _mouse;
            }
        }

        public KeyboardHelper Keyboard
        {
            get
            {
                return _keyboard;
            }
        }

        public GamePadHelper[] Gamepad
        {
            get
            {
                return _gamepad;
            }
        }

        private InputManager _instance;

        public Vector2 WadsDirection
        {
            get
            {
                Vector2 direction = Vector2.Zero;
                if (Keyboard.Held(Keys.W))
                    direction.Y = -1;
                if (Keyboard.Held(Keys.A))
                    direction.X = -1;
                if (Keyboard.Held(Keys.S))
                    direction.Y = 1;
                if (Keyboard.Held(Keys.D))
                    direction.X= 1;
                Vector2Extensions.SafeNormalize(ref direction);
                return direction;
            }
        }

        public Vector2 ArrowDirection
        {
            get
            {
                Vector2 direction = Vector2.Zero;
                if (Keyboard.Held(Keys.Up))
                    direction.Y = -1;
                if (Keyboard.Held(Keys.Left))
                    direction.X = -1;
                if (Keyboard.Held(Keys.Down))
                    direction.Y = 1;
                if (Keyboard.Held(Keys.Right))
                    direction.X = 1;
                Vector2Extensions.SafeNormalize(ref direction);
                return direction;
            }
        }

        public Vector2 DpadDirection
        {
            get
            {
                Vector2 direction = Vector2.Zero;
                if (Gamepad[1].Held(Buttons.DPadUp))
                    direction.Y = -1;
                if (Gamepad[1].Held(Buttons.DPadRight))
                    direction.X = -1;
                if (Gamepad[1].Held(Buttons.DPadDown))
                    direction.Y = 1;
                if (Gamepad[1].Held(Buttons.DPadRight))
                    direction.X = 1;
                Vector2Extensions.SafeNormalize(ref direction);
                return direction;
            }
        }

        public InputManager()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("InputManager already exists!");
            }

            _mouse = new MouseHelper();
            _keyboard = new KeyboardHelper();

            _gamepad = new GamePadHelper[MAX_GAMEPADS];
            for(int i = 0; i < MAX_GAMEPADS; i++) 
            {
                _gamepad[i] = new GamePadHelper((PlayerIndex) i);
            }

            _instance = this;
        }

        public void Update(GameTime gameTime)
        {
            Keyboard.Update(gameTime);
            Mouse.Update(gameTime);
            for(int i = 0; i < 4; i++) 
            {
                Gamepad[i].Update(gameTime);
            }
        }
    }
}
