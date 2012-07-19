using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Input
{
    public struct GamePadButtons
    {
        public Button A { get; internal set; }
        public Button B { get; internal set; }
        public Button X { get; internal set; }
        public Button Y { get; internal set; }
        public Button Back { get; internal set; }
        public Button Start { get; internal set; }
        public Button LeftStickClick { get; internal set; }
        public Button RightStickClick { get; internal set; }
        public Button DpadLeft { get; internal set; }
        public Button DpadRight { get; internal set; }
        public Button DpadUp { get; internal set; }
        public Button DpadDown { get; internal set; }
        public Button LeftBumper { get; internal set; }
        public Button RightBumper { get; internal set; }
    }
}
