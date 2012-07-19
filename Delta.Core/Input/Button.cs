using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Input
{
    public struct Button
    {
        bool _previousFrameIsDown;
        float _pressStarted;
        float _releaseStarted;

        public bool IsDown { get; private set; }
        public float DownDuration { get; private set; }
        public float ReleaseDuration { get; private set; }
        public bool IsPressed { get { return IsDown && !_previousFrameIsDown; } }
        public bool IsReleased { get { return IsDown && !_previousFrameIsDown; } }

        internal void SetState(bool value, DeltaTime time)
        {
            if (value && !IsDown)
                _pressStarted = time.TotalSeconds;
            if (value)
                DownDuration = time.TotalSeconds - _pressStarted;
            _previousFrameIsDown = IsDown;
            IsDown = value;
            if (!IsDown && _previousFrameIsDown)
                _releaseStarted = time.TotalSeconds;
            if (_releaseStarted != 0)
                ReleaseDuration = time.TotalSeconds - _releaseStarted;
        }

        public static implicit operator bool(Button b)
        {
            return b.IsDown;
        }
    }
}
