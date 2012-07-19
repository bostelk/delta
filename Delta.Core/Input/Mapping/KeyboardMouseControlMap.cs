//#if WINDOWS

//using System;
//using Microsoft.Xna.Framework.Input;
//using Delta.Input.States;

//namespace Delta.Input.Mapping
//{

//    public struct KeyboardMouseControlMap : IComparable<KeyboardMouseControlMap>
//    {
//        public static implicit operator KeyboardMouseControlMap(MouseInput mouse)
//        {
//            return new KeyboardMouseControlMap(mouse);
//        }

//        public static implicit operator KeyboardMouseControlMap(Keys key)
//        {
//            return new KeyboardMouseControlMap(key);
//        }

//        Keys _key;
//        MouseInput _mouse;
//        bool _usingMouse = false;

//        public KeyboardMouseControlMap(Keys key)
//        {
//            _key = key;
//            _usingMouse = false;
//            _mouse = MouseInput.LeftButton;
//        }

//        public KeyboardMouseControlMap(MouseInput mouse)
//        {
//            _mouse = mouse;
//            _usingMouse = true;
//            _key = (Keys)0;
//        }

//        public bool UsingMouse { get { return _usingMouse; } }

//        public MouseInput MouseInputMapping
//        {
//            get { return _mouse; }
//            set 
//            { 
//                _mouse = value; 
//                _usingMouse = true; 
//            }
//        }

//        public bool IsAnalog
//        {
//            get
//            {
//                if (_usingMouse)
//                    return _mouse == MouseInput.Y || _mouse == MouseInput.X || _mouse == MouseInput.ScrollWheel;
//                return false;
//            }
//        }

//        public Keys KeyMapping
//        {
//            get { return _key; }
//            set
//            {
//                _key = value;
//                _usingMouse = false;
//            }
//        }

//        public override string ToString()
//        {
//            if (_usingMouse)
//                return _mouse.ToString();
//            else
//                return _key.ToString();
//        }

//        public float GetValue(KeyboardState keyboardState, MouseState mouseState, MouseState previousMouseState, bool invert)
//        {
//            if (_usingMouse)
//            {
//                float value = 0;
//                switch (_mouse)
//                {
//                    case MouseInput.LeftButton:
//                        value = mouseState.LeftButton == ButtonState.Pressed ? 1 : 0;
//                        break;
//                    case MouseInput.MiddleButton:
//                        value = mouseState.MiddleButton == ButtonState.Pressed ? 1 : 0;
//                        break;
//                    case MouseInput.RightButton:
//                        value = mouseState.RightButton == ButtonState.Pressed ? 1 : 0;
//                        break;
//                    case MouseInput.ScrollWheel:
//                        if (invert)
//                            return 0;
//                        value = mouseState.ScrollWheelValue / 640.0f;
//                        break;
//                    case MouseInput.XButton1:
//                        value = mouseState.XButton1 == ButtonState.Pressed ? 1 : 0;
//                        break;
//                    case MouseInput.XButton2:
//                        value = mouseState.XButton2 == ButtonState.Pressed ? 1 : 0;
//                        break;
//                    case MouseInput.X:
//                        if (invert)
//                            return 0;
//                        value = (mouseState.X - previousMouseState.x) / 8.0f;
//                        break;
//                    case MouseInput.Y:
//                        if (invert)
//                            return 0;
//                        value = (mouseState.Y - previousMouseState.Y) / -8.0f;
//                        break;
//                }

//                if (invert)
//                    return -value;
//                return value;
//            }
//            else
//            {
//                if (keyboardState.IsKeyDown(_key))
//                {
//                    if (invert)
//                        return -1;
//                    return 1;
//                }
//                return 0;
//            }
//        }

//        public bool GetValue(ref KeyboardState keyboardState, ref MouseState mouseState)
//        {
//            if (_usingMouse)
//            {
//                switch (_mouse)
//                {
//                    case MouseInput.MiddleButton:
//                        return mouseState.MiddleButton == ButtonState.Pressed;
//                    case MouseInput.RightButton:
//                        return mouseState.RightButton == ButtonState.Pressed;
//                    case MouseInput.XButton1:
//                        return mouseState.XButton1 == ButtonState.Pressed;
//                    case MouseInput.XButton2:
//                        return mouseState.XButton2 == ButtonState.Pressed;
//                    default:
//                        return mouseState.LeftButton == ButtonState.Pressed;
//                }
//            }
//            else
//                return keyboardState.IsKeyDown(_key);
//        }
//#else
//        public float GetValue(KeyboardMouseState inputState, bool invert)
//        {
//            KeyboardState _keyboardState = inputState.KeyboardState;

//            if (_keyboardState.IsKeyDown(key))
//            {
//                if (invert)
//                    return -1;
//                return 1;
//            }
//            return 0;
//        }

//        public bool GetValue(KeyboardMouseState inputState)
//        {
//            return inputState.KeyboardState.IsKeyDown(key);
//        }
//#endif

//        public int CompareTo(KeyboardMouseControlMap map)
//        {
//#if !XBOX360
//            if (map._usingMouse != _usingMouse)
//            {
//                if (_usingMouse)
//                    return 1;
//                else
//                    return -1;
//            }

//            if (_key != map._key)
//                return (int)map._key - (int)_key;
//            return (int)map._mouse - (int)_mouse;
//#else
//        return (int)map.key - (int)key;
//#endif
//        }
//    }
//}
