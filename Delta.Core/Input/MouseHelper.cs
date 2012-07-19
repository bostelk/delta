//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

//namespace Delta.Input
//{
//    public enum MouseButton
//    {
//        Left,
//        Middle,
//        Right
//    }

//    public class MouseHelper
//    {
//        MouseState _thisState;
//        MouseState _previousState;

//        /// <summary>
//        /// Location in the Window
//        /// </summary>
//        public Vector2 Position
//        {
//            get
//            {
//                return new Vector2(_thisState.X, _thisState.Y);
//            }
//        }

//        public float ScrollWheel {
//            get
//            {
//                return _thisState.ScrollWheelValue;
//            }
//        }

//        public MouseHelper()
//        {
//            _thisState = _previousState = Mouse.GetState();
//        }

//        public bool ScrollWheelUp()
//        {
//            return (_thisState.ScrollWheelValue - _previousState.ScrollWheelValue) > 0;
//        }

//        public bool ScrollWheelDown()
//        {
//            return (_thisState.ScrollWheelValue - _previousState.ScrollWheelValue) < 0;
//        }

//        public bool JustPressed(MouseButton mouseButton)
//        {
//            switch (mouseButton)
//            {
//                case MouseButton.Left:
//                    return _thisState.LeftButton == ButtonState.Pressed && _previousState.LeftButton == ButtonState.Released;
//                case MouseButton.Right:
//                    return _thisState.RightButton == ButtonState.Pressed && _previousState.RightButton == ButtonState.Released;
//                case MouseButton.Middle:
//                    return _thisState.MiddleButton == ButtonState.Pressed && _previousState.MiddleButton == ButtonState.Released;
//                default:
//                    throw new InvalidOperationException("Which button did you want?");
//            }
//        }

//        public bool JustReleased(MouseButton mouseButton)
//        {
//            switch (mouseButton)
//            {
//                case MouseButton.Left:
//                    return _thisState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed;
//                case MouseButton.Right:
//                    return _thisState.RightButton == ButtonState.Released && _previousState.RightButton == ButtonState.Pressed;
//                case MouseButton.Middle:
//                    return _thisState.MiddleButton == ButtonState.Released && _previousState.MiddleButton == ButtonState.Pressed;
//                default:
//                    throw new InvalidOperationException("Which button did you want?");
//            }
//        }

//        public bool Held(MouseButton mouseButton)
//        {
//            switch(mouseButton) {
//                case MouseButton.Left:
//                    return _thisState.LeftButton == ButtonState.Pressed;
//                case MouseButton.Right:
//                    return _thisState.RightButton == ButtonState.Pressed;
//                case MouseButton.Middle:
//                    return _thisState.MiddleButton == ButtonState.Pressed;
//                default:
//                    throw new InvalidOperationException("Which button did you want?");
//            }
//        }

//        public void Update(GameTime gameTime)
//        {
//            _previousState = _thisState;
//            _thisState = Mouse.GetState();
//        }
//    }
//}
