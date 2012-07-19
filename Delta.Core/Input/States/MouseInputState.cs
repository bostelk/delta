using System;
using Microsoft.Xna.Framework.Input;

namespace Delta.Input.States
{
#if WINDOWS
	public sealed class MouseInputState
	{
		MouseState state;
        Button _left;
        Button _right; 
        Button _middle; 
        Button _x1; 
        Button _x2;
		int _x;
        int _y;
        int _xDelta;
        int _yDelta;
        int _scroll;
        int _scrollDelta;

		public int X { get { return _x; } }
		public int Y { get { return _y; } }
        public int XDelta { get { return _xDelta; } }
        public int YDelta { get { return _yDelta; } }
		public Button LeftButton { get { return _left; } }
		public Button RightButton { get { return _right; } }
		public Button MiddleButton { get { return _middle; } }
		public Button XButton1 { get { return _x2; } }
		public Button XButton2 { get { return _x1; } }
		public int ScrollWheelValue { get { return _scroll; } }
		public int ScrollWheelDelta { get { return _scrollDelta; } }
		
		internal void Update(DeltaTime time, ref MouseState mouseState)
		{
			state = mouseState;

            _xDelta = state.X - _x;
            _yDelta = state.Y - _y;
			_x = state.X;
			_y = state.Y;
			_scrollDelta = state.ScrollWheelValue - _scroll;
			_scroll = state.ScrollWheelValue;

			_left.SetState(state.LeftButton == ButtonState.Pressed, time);
			_right.SetState(state.RightButton == ButtonState.Pressed, time);
			_middle.SetState(state.MiddleButton== ButtonState.Pressed, time);
			_x1.SetState(state.XButton1 == ButtonState.Pressed, time);
			_x2.SetState(state.XButton2 == ButtonState.Pressed, time);
		}

		public static implicit operator MouseState(MouseInputState input)
		{
			return input.state;
		}
	}

#endif
}
