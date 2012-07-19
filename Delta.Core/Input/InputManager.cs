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
        }
	}
 }
