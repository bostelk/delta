using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Input
{
    public class Button
    {
        float _pressedStarted;
        float _releasedStarted;

        public bool WasDown { get; private set; }
        public bool IsDown { get; private set; }
        public float DownDuration { get; private set; }
        public float UpDuration { get; private set; }
        public bool IsPressed { get { return IsDown && !WasDown; } }
        public bool IsReleased { get { return !IsDown && WasDown; } }

        internal void SetState(bool value, DeltaTime time)
        {
            if (value && !IsDown)
                _pressedStarted = time.TotalSeconds;
            WasDown = IsDown;
            IsDown = value;
            if (!value && WasDown)
                _releasedStarted = time.TotalSeconds;
            if (value)
            {
                DownDuration = time.TotalSeconds - _pressedStarted;
                UpDuration = 0;
                _releasedStarted = 0;
            }
            else
            {
                DownDuration = 0;
                UpDuration = time.TotalSeconds - _releasedStarted;
                _pressedStarted = 0;
            }
        }

        public override string ToString()
        {
            return String.Format("Down: {0}, WasDown: {1}", IsDown, WasDown);
        }
    }
}
