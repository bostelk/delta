using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Delta.Input.States;

namespace Delta.Input
{
    public sealed class InputManager
    {
        PlayerInput[] _playerInputs = new PlayerInput[4];
        GamePadState[] _gamePads = new GamePadState[4];
        KeyboardState[] _chatPads = new KeyboardState[4];
#if WINDOWS
        MouseState _mouseState = new MouseState();
        public KeyboardInputState Keyboard { get; private set; }
        public MouseInputState Mouse { get; private set; } 
#endif
        public PlayerInputCollection PlayerInput { get; private set; }

        public Vector2 WasdDirection
        {
            get
            {
                Vector2 direction = Vector2.Zero;
                if (Keyboard.IsDown(Keys.W))
                    direction.Y = -1;
                if (Keyboard.IsDown(Keys.A))
                    direction.X = -1;
                if (Keyboard.IsDown(Keys.S))
                    direction.Y = 1;
                if (Keyboard.IsDown(Keys.D))
                    direction.X = 1;
                Vector2Extensions.SafeNormalize(ref direction);
                return direction;
            }
        }

        public Vector2 ArrowDirection
        {
            get
            {
                Vector2 direction = Vector2.Zero;
                if (Keyboard.IsDown(Keys.Up))
                    direction.Y = -1;
                if (Keyboard.IsDown(Keys.Left))
                    direction.X = -1;
                if (Keyboard.IsDown(Keys.Down))
                    direction.Y = 1;
                if (Keyboard.IsDown(Keys.Right))
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
                if (PlayerInput[0].GamePad.Buttons.DpadUp.IsDown)
                    direction.Y = -1;
                if (PlayerInput[0].GamePad.Buttons.DpadRight.IsDown)
                    direction.X = -1;
                if (PlayerInput[0].GamePad.Buttons.DpadDown.IsDown)
                    direction.Y = 1;
                if (PlayerInput[0].GamePad.Buttons.DpadRight.IsDown)
                    direction.X = 1;
                Vector2Extensions.SafeNormalize(ref direction);
                return direction;
            }
        }

		internal InputManager()
		{
            for (int i = 0; i < 4; i++)
                _playerInputs[i] = new PlayerInput(i);

            PlayerInput = new PlayerInputCollection(_playerInputs);
#if WINDOWS
            Keyboard = new KeyboardInputState();
            Mouse = new MouseInputState();
#endif
		}

        internal void Update(DeltaTime time)
        {
            for (int i = 0; i < 4; i++)
			{
				_gamePads[i] = Microsoft.Xna.Framework.Input.GamePad.GetState((PlayerIndex)i);
				_chatPads[i] = Microsoft.Xna.Framework.Input.Keyboard.GetState((PlayerIndex)i);
			}
#if WINDOWS
            _mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            Keyboard.Update(time, ref _chatPads[0]);
            Mouse.Update(time, ref _mouseState);
#endif
#if XBOX
            for (int i = 0; i < 4; i++)
            {
                if (PlayerInput[i].ControlInput != ControlInput.KeyboardMouse)
                    PlayerInput[i].UpdatePadState(totalSeconds, ref keyboards[(int)this.PlayerInput[i].ControlInput]);
            }
#endif
            if (Mouse.PositionDelta != Vector2.Zero)
                G.UI.MouseMove();
            if (Mouse.LeftButton.IsPressed || Mouse.RightButton.IsPressed || Mouse.MiddleButton.IsPressed || Mouse.XButton1.IsPressed || Mouse.XButton2.IsPressed)
                G.UI.MouseDown();
            if (Mouse.LeftButton.IsReleased || Mouse.RightButton.IsReleased || Mouse.MiddleButton.IsReleased || Mouse.XButton1.IsReleased || Mouse.XButton2.IsReleased)
                G.UI.MouseUp();
        }
	}
 }
