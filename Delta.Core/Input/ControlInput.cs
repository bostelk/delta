using System;

namespace Delta.Input
{
    public enum ControlInput
    {
        GamePad1,
        GamePad2,
        GamePad3,
        GamePad4
#if WINDOWS
        ,
        KeyboardMouse
#endif
    }
}
