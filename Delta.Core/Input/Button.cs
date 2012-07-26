using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Input
{
    public class Button
    {
        public bool WasDown { get; private set; }
        public bool IsDown { get; private set; }
        public float DownDuration { get; private set; }
        public float UpDuration { get; private set; }
        public bool IsPressed { get { return IsDown && !WasDown; } }
        public bool IsReleased { get { return !IsDown && WasDown; } }

        internal void SetState(bool value, DeltaGameTime time)
        {
            WasDown = IsDown;
            IsDown = value;
            if (IsDown && WasDown)
                DownDuration += time.ElapsedSeconds;
            else
                DownDuration = 0;
            if (!IsDown && !WasDown)
                UpDuration += time.ElapsedSeconds;
            else
                UpDuration = 0;
        }

        public override string ToString()
        {
            return String.Format("Down: {0}, WasDown: {1}", IsDown, WasDown);
        }
    }
}
