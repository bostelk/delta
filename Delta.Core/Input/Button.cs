using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Input
{
    public struct Button
    {
        bool _previousFrameIsDown;
        float _pressedStarted;
        float _releasedStarted;

        public bool IsDown { get; private set; }
        public float DownDuration { get; private set; }
        public float UpDuration { get; private set; }
        public bool IsPressed { get { return IsDown && !_previousFrameIsDown; } }
        public bool IsReleased { get { return IsDown && !_previousFrameIsDown; } }

        internal void SetState(bool value, DeltaTime time)
        {
            if (value && !IsDown)
                _pressedStarted = time.TotalSeconds;
            _previousFrameIsDown = IsDown;
            IsDown = value;
            if (!value && _previousFrameIsDown)
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

        public static implicit operator bool(Button b)
        {
            return b.IsDown;
        }
    }
}
