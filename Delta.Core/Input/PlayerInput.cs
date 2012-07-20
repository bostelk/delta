using System;
using Microsoft.Xna.Framework;
using Delta.Input.States;

namespace Delta.Input
{
    public sealed class PlayerInput
    {
        KeyboardInputState _chatPadState = new KeyboardInputState();
        GamePadInputState _gamePadState = null;
        internal int _index = 0;
        PlayerIndex _playerIndex = 0;

        internal PlayerInput(int index)
        {
            _index = index;
            _playerIndex = (PlayerIndex)_index;
            _gamePadState = new GamePadInputState(_playerIndex);
        }

        public PlayerIndex PlayerIndex{ get { return _playerIndex; } }
        public KeyboardInputState ChatPad { get { return _chatPadState; } }
        public GamePadInputState GamePad { get { return _gamePadState; } }
    }
}
