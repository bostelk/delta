using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Delta.Input.States
{
#if WINDOWS
	public sealed class MouseInputState
	{
        internal MouseState MouseState { get; private set; }
        public Point Position { get; private set; }
        public Point PositionOld { get; private set; }
        public Point PositionDelta { get; private set; }
        public Button LeftButton { get; private set; }
        public Button RightButton { get; private set; }
        public Button MiddleButton { get; private set; }
        public Button XButton1 { get; private set; }
        public Button XButton2 { get; private set; }
        public int ScrollWheelValue { get; private set; }
        public int ScrollWheelDelta { get; private set; }

        internal MouseInputState()
            : base()
        {
            LeftButton = new Button();
            RightButton = new Button();
            MiddleButton = new Button();
            XButton1 = new Button();
            XButton2 = new Button();
        }
		
		internal void Update(DeltaTime time, ref MouseState mouseState)
		{
            MouseState = mouseState;
            PositionOld = Position;
            Position = new Point(mouseState.X, mouseState.Y);
            PositionDelta = new Point(Position.X - PositionOld.X, Position.Y - PositionOld.Y);
			ScrollWheelDelta = mouseState.ScrollWheelValue - ScrollWheelValue;
			ScrollWheelValue = mouseState.ScrollWheelValue;

			LeftButton.SetState(mouseState.LeftButton == ButtonState.Pressed, time);
			RightButton.SetState(mouseState.RightButton == ButtonState.Pressed, time);
			MiddleButton.SetState(mouseState.MiddleButton== ButtonState.Pressed, time);
			XButton1.SetState(mouseState.XButton1 == ButtonState.Pressed, time);
			XButton2.SetState(mouseState.XButton2 == ButtonState.Pressed, time);
		}
	}

#endif
}
