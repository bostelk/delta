using System;
using Microsoft.Xna.Framework;
using Delta.Input.States;

namespace Delta.Input
{
    public sealed class PlayerInput
    {
        KeyboardInputState _chatPadState = new KeyboardInputState();
        PlayerInputCollection _parent = null;
        ControlInput _controlInput = ControlInput.KeyboardMouse;
        //private KeyboardMouseControlMapping kmb = new KeyboardMouseControlMapping();
        //internal GamePadMapper _gamePadMapper = new GamePadMapper();
        GamePadInputState _gamePadState = new GamePadInputState();
        internal int _index = 0;
        PlayerIndex _playerIndex = 0;

        internal PlayerInput(int index, PlayerInputCollection parent)
        {
            _index = index;
            _playerIndex = (PlayerIndex)_index;
            _parent = parent;
            //kmb.inputParent = parent;
            //_gamePadMapper.inputParent = parent;
        }

        public PlayerIndex PlayerIndex{ get { return _playerIndex; } }
        public KeyboardInputState ChatPad { get { return _chatPadState; } }
        public GamePadInputState GamePad { get { return _gamePadState; } }
        public ControlInput ControlInput { get; set; }

        //public InputMapper InputMapper
        //{
        //    get { return _gamePadMapper; }
        //    set
        //    {
        //        if (ReadOnly)
        //            throw new ArgumentException("readonly");

        //        if (value == null)
        //            throw new ArgumentNullException();

        //        _gamePadMapper = value;
        //        _gamePadMapper.inputParent = _parent;
        //    }
        //}


        ///// <summary>
        ///// Gets/Sets the class used for mapping keyboard/mouse controls to gamepad equivalent controls 
        ///// </summary>
        //public KeyboardMouseControlMapping KeyboardMouseControlMapping
        //{
        //    get { return kmb; }
        //    set
        //    {
        //        if (ReadOnly)
        //            throw new ArgumentException("readonly");

        //        if (value == null)
        //            throw new ArgumentNullException();

        //        kmb = value;
        //        kmb.inputParent = _parent;
        //    }
        //}

//#if XBOX360
//        internal void UpdatePadState(long tick, ref KeyboardState keyboardState)
//        {
//            this.chatPadState.Update(tick,ref keyboardState);
//        }
//#endif
    }
}
