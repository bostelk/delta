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
		int x;
        int y;
        int scroll;
        int scrollDelta;

		/// <summary>
		/// Horizontal position of the mouse cursor.
		/// </summary>
		public int X { get { return x; } }
		/// <summary>
		/// Vertical position of the mouse cursor.
		/// </summary>
		public int Y { get { return y; } }
		/// <summary>
		/// <see cref="Button"/> state of the left mouse button.
		/// </summary>
		public Button LeftButton { get { return _left; } }
		/// <summary>
		/// <see cref="Button"/> state of the right mouse button.
		/// </summary>
		public Button RightButton { get { return _right; } }
		/// <summary>
		/// <see cref="Button"/> state of the middle mouse button.
		/// </summary>
		public Button MiddleButton { get { return _middle; } }
		/// <summary>
		/// <see cref="Button"/> state of the first X mouse button.
		/// </summary>
		public Button XButton1 { get { return _x2; } }
		/// <summary>
		/// <see cref="Button"/> state of the second X mouse button.
		/// </summary>
		public Button XButton2 { get { return _x1; } }
		/// <summary>
		/// Gets the total mouse scroll movement.
		/// </summary>
		public int ScrollWheelValue { get { return scroll; } }
		/// <summary>
		/// Gets the delta mouse scroll movement.
		/// </summary>
		public int ScrollWheelDelta { get { return scrollDelta; } }
		
		internal void Update(DeltaTime time, ref MouseState mouseState)
		{
			state = mouseState;

			x = state.X;
			y = state.Y;
			scrollDelta = state.ScrollWheelValue - this.scroll;
			scroll = state.ScrollWheelValue;

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
