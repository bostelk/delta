using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Delta.Input.States
{
    public sealed class KeyboardInputState
    {
        static readonly Keys[] _keyIndices = new Keys[256];
#if WINDOWS
        static readonly uint[] _scanCodes = new uint[256];
        static readonly byte[] _keyStateBytes = new byte[256];

        static IntPtr _layout;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, out char pwszBuff, int cchBuff, uint wFlags, IntPtr layout);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr layout);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint thread);
#else
		private static char[] _chatpadGreenMapping = new char[256];
		private static char[] _chatpadOrangeMapping = new char[256];
#endif

        Button[] _buttons = new Button[256];
        float _initialTick = -1;
        KeyState _state;
        KeyMap _currentFrame;
        KeyMap _previousFrame;

        static KeyboardInputState()
        {
            System.Reflection.FieldInfo[] enums = typeof(Keys).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            List<Keys> keys = new List<Keys>();
            List<Keys> controlKeys = new List<Keys>();

            foreach (System.Reflection.FieldInfo field in enums)
            {
                Keys key = (Keys)field.GetValue(null);
                keys.Add(key);
                if (char.IsControl((char)key))
                    controlKeys.Add(key);
            }

#if WINDOWS
            _layout = GetKeyboardLayout(0);

            for (int scan = 0; scan < 255; scan++)
            {
                uint key = MapVirtualKeyEx((uint)scan, 1, _layout);
                if (key != 0)
                    _scanCodes[key] = (uint)scan;
            }
#else
			char[] map = _chatpadGreenMapping;

			map['q'] = '!';
			map['w'] = '@';
			map['e'] = (char)8364;//'euro';
			map['r'] = '#';
			map['t'] = '%';
			map['y'] = '^';
			map['U'] = '&';
			map['I'] = '*';
			map['o'] = '(';
			map['p'] = ')';
			map['a'] = '~';
			map['s'] = (char)353;
			map['d'] = '{';
			map['f'] = '}';
			map['g'] = (char)168;
			map['h'] = '/';
			map['j'] = (char)903;
			map['k'] = '[';
			map['l'] = ']';
			map[','] = ':';
			map['z'] = (char)729;
			map['x'] = (char)0x00AB;
			map['c'] = (char)0x00BB;
			map['v'] = '-';
			map['b'] = '|';
			map['n'] = '<';
			map['m'] = '>';
			map['.'] = '?';

			map = _chatpadOrangeMapping;
			map['q'] = (char)0x00EC;
			map['w'] = (char)0x00E3;
			map['e'] = (char)0x00E9;
			map['r'] = '$';
			map['t'] = (char)0x00DE;
			map['y'] = (char)0x00FD;
			map['U'] = (char)0x00FA;
			map['I'] = (char)0x00ED;
			map['o'] = (char)0x00F3;
			map['p'] = '=';
			map['a'] = (char)0x00E1;
			map['s'] = (char)0x00DF;
			map['d'] = (char)0x00F4;
			map['f'] = (char)0x00A3;
			map['g'] = (char)0x00A5;
			map['h'] = '\\';
			map['j'] = '\"';
			map['k'] = '$';
			map['l'] = (char)0x00F8;
			map[','] = ';';
			map['z'] = (char)0x00E6;
			map['x'] = (char)0x0153;
			map['c'] = (char)0x00E7;
			map['v'] = '_';
			map['b'] = '+';
			map['n'] = (char)0x00F1;
			map['m'] = (char)0x00B5;
			map['.'] = (char)0x00BF;
#endif
            _keyIndices = keys.ToArray();
        }

        internal KeyboardInputState()
        {
            _state = new KeyState(_buttons);
            for (int x = 0; x < 256; x++)
                _buttons[x] = new Button();
        }

        public static Keys[] GetKeyArray()
        {
            return (Keys[])_keyIndices.Clone();
        }

        public bool TryGetKeyChar(Keys key, out char keyChar)
        {
#if XBOX360
			keyChar = (char)key;
			if (this.KeyState.ChatPadGreen && _chatpadGreenMapping[(int)key] != '\0')
				keyChar = char.ToUpper(_chatpadGreenMapping[(int)key]);
			if (this.KeyState.ChatPadOrange && _chatpadOrangeMapping[(int)key] != '\0')
				keyChar = char.ToUpper(_chatpadOrangeMapping[(int)key]);
			if (!(this.KeyState.LeftShift | this.KeyState.RightShift))
				keyChar = char.ToLower(keyChar);
			return char.IsLetterOrDigit(keyChar) || char.IsPunctuation(keyChar) || char.IsWhiteSpace(keyChar);
#else
            keyChar = '\0';
            _keyStateBytes[0x10] = (byte)((KeyState.LeftShift.IsDown | KeyState.RightShift.IsDown) ? 0x80 : 0);
            _keyStateBytes[0x11] = (byte)((KeyState.LeftControl.IsDown | KeyState.RightControl.IsDown) ? 0x80 : 0);
            return ToUnicodeEx((uint)key, _scanCodes[(uint)key], _keyStateBytes, out keyChar, 1, 0, _layout) == 1;
#endif
        }

        public KeyState KeyState { get { return _state; } }

        public static implicit operator KeyboardState(KeyboardInputState input)
        {
            return input._currentFrame.state;
        }

        public Button this[Keys key] { get { return _buttons[(int)key]; } }

        public bool IsDown(Keys key)
        {
            return _buttons[(int)key].IsDown;
        }

        //public bool IsDown(Keys key, float duration)
        //{
        //    return _buttons[(int)key].IsDown && _buttons[(int)key].DownDuration >= duration;
        //}

        public bool IsUp(Keys key)
        {
            return !_buttons[(int)key].IsDown;
        }

        //public bool IsUp(Keys key, float duration)
        //{
        //    return !_buttons[(int)key] && _buttons[(int)key].UpDuration >= duration;
        //}

        public bool IsPressed(Keys key)
        {
            return _buttons[(int)key].IsPressed;
        }

        public bool IsReleased(Keys key)
        {
            return _buttons[(int)key].IsReleased;
        }

        internal void Update(DeltaGameTime time, ref KeyboardState keyboardState)
        {
            if (_initialTick <= -1)
            {
                _initialTick = time.TotalSeconds;
                _currentFrame.state = keyboardState;
            }

            time.TotalSeconds -= _initialTick;

            _previousFrame.state = _currentFrame.state;
            _currentFrame.state = keyboardState;

            for (int i = 0; i < 32; i++)
            {
                _buttons[32 * 0 + i].SetState((_currentFrame.currentState0 & ((uint)1) << i) != 0, time);
                _buttons[32 * 1 + i].SetState((_currentFrame.currentState1 & ((uint)1) << i) != 0, time);
                _buttons[32 * 2 + i].SetState((_currentFrame.currentState2 & ((uint)1) << i) != 0, time);
                _buttons[32 * 3 + i].SetState((_currentFrame.currentState3 & ((uint)1) << i) != 0, time);
                _buttons[32 * 4 + i].SetState((_currentFrame.currentState4 & ((uint)1) << i) != 0, time);
                _buttons[32 * 5 + i].SetState((_currentFrame.currentState5 & ((uint)1) << i) != 0, time);
                _buttons[32 * 6 + i].SetState((_currentFrame.currentState6 & ((uint)1) << i) != 0, time);
                _buttons[32 * 7 + i].SetState((_currentFrame.currentState7 & ((uint)1) << i) != 0, time);
            }
        }

        public void KeyDown(Action<Keys> callback)
        {
            if (_currentFrame.currentState0 != 0) HeldCallback(callback, 0);
            if (_currentFrame.currentState1 != 0) HeldCallback(callback, 1);
            if (_currentFrame.currentState2 != 0) HeldCallback(callback, 2);
            if (_currentFrame.currentState3 != 0) HeldCallback(callback, 3);
            if (_currentFrame.currentState4 != 0) HeldCallback(callback, 4);
            if (_currentFrame.currentState5 != 0) HeldCallback(callback, 5);
            if (_currentFrame.currentState6 != 0) HeldCallback(callback, 6);
            if (_currentFrame.currentState7 != 0) HeldCallback(callback, 7);
        }

        public void KeyPressed(Action<Keys> callback)
        {
            if (_currentFrame.currentState0 != _previousFrame.currentState0) PressCallback(callback, 0);
            if (_currentFrame.currentState1 != _previousFrame.currentState1) PressCallback(callback, 1);
            if (_currentFrame.currentState2 != _previousFrame.currentState2) PressCallback(callback, 2);
            if (_currentFrame.currentState3 != _previousFrame.currentState3) PressCallback(callback, 3);
            if (_currentFrame.currentState4 != _previousFrame.currentState4) PressCallback(callback, 4);
            if (_currentFrame.currentState5 != _previousFrame.currentState5) PressCallback(callback, 5);
            if (_currentFrame.currentState6 != _previousFrame.currentState6) PressCallback(callback, 6);
            if (_currentFrame.currentState7 != _previousFrame.currentState7) PressCallback(callback, 7);
        }

        public void KeyUp(Action<Keys> callback)
        {
            if (_currentFrame.currentState0 != _previousFrame.currentState0) ReleaseCallback(callback, 0);
            if (_currentFrame.currentState1 != _previousFrame.currentState1) ReleaseCallback(callback, 1);
            if (_currentFrame.currentState2 != _previousFrame.currentState2) ReleaseCallback(callback, 2);
            if (_currentFrame.currentState3 != _previousFrame.currentState3) ReleaseCallback(callback, 3);
            if (_currentFrame.currentState4 != _previousFrame.currentState4) ReleaseCallback(callback, 4);
            if (_currentFrame.currentState5 != _previousFrame.currentState5) ReleaseCallback(callback, 5);
            if (_currentFrame.currentState6 != _previousFrame.currentState6) ReleaseCallback(callback, 6);
            if (_currentFrame.currentState7 != _previousFrame.currentState7) ReleaseCallback(callback, 7);
        }

        //public void GetPressedKeys(List<Keys> pressedList)
        //{
        //    pressedList.Clear();
        //    if (_currentFrame.currentState0 != _previousFrame.currentState0) PressList(pressedList, 0);
        //    if (_currentFrame.currentState1 != _previousFrame.currentState1) PressList(pressedList, 1);
        //    if (_currentFrame.currentState2 != _previousFrame.currentState2) PressList(pressedList, 2);
        //    if (_currentFrame.currentState3 != _previousFrame.currentState3) PressList(pressedList, 3);
        //    if (_currentFrame.currentState4 != _previousFrame.currentState4) PressList(pressedList, 4);
        //    if (_currentFrame.currentState5 != _previousFrame.currentState5) PressList(pressedList, 5);
        //    if (_currentFrame.currentState6 != _previousFrame.currentState6) PressList(pressedList, 6);
        //    if (_currentFrame.currentState7 != _previousFrame.currentState7) PressList(pressedList, 7);
        //}

        //public void GetHeldKeys(List<Keys> heldKeyList)
        //{
        //    heldKeyList.Clear();
        //    if (_currentFrame.currentState0 != 0) HeldList(heldKeyList, 0);
        //    if (_currentFrame.currentState1 != 0) HeldList(heldKeyList, 1);
        //    if (_currentFrame.currentState2 != 0) HeldList(heldKeyList, 2);
        //    if (_currentFrame.currentState3 != 0) HeldList(heldKeyList, 3);
        //    if (_currentFrame.currentState4 != 0) HeldList(heldKeyList, 4);
        //    if (_currentFrame.currentState5 != 0) HeldList(heldKeyList, 5);
        //    if (_currentFrame.currentState6 != 0) HeldList(heldKeyList, 6);
        //    if (_currentFrame.currentState7 != 0) HeldList(heldKeyList, 7);
        //}

        void PressCallback(Action<Keys> callback, int group)
        {
            for (int i = 0; i < 32; i++)
                if (_buttons[32 * group + i].IsPressed)
                    callback((Keys)(32 * group + i));
        }

        void ReleaseCallback(Action<Keys> callback, int group)
        {
            for (int i = 0; i < 32; i++)
                if (_buttons[32 * group + i].IsReleased)
                    callback((Keys)(32 * group + i));
        }

        void HeldCallback(Action<Keys> callback, int group)
        {
            for (int i = 0; i < 32; i++)
                if (_buttons[32 * group + i].IsDown)
                    callback((Keys)(32 * group + i));
        }

        //void PressList(List<Keys> list, int group)
        //{
        //    for (int i = 0; i < 32; i++)
        //        if (_buttons[32 * group + i].IsPressed)
        //            list.Add((Keys)(32 * group + i));
        //}
        //void HeldList(List<Keys> list, int group)
        //{
        //    for (int i = 0; i < 32; i++)
        //        if (_buttons[32 * group + i].IsDown)
        //            list.Add((Keys)(32 * group + i));
        //}
    }
}
