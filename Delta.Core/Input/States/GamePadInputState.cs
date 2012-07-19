using System;

namespace Delta.Input.States
{
    public sealed class GamePadInputState
    {
        public GamePadTriggers Triggers { get; internal set; }
        public GamePadThumbSticks ThumbSticks { get; internal set; }
        public GamePadButtons Buttons { get; set; }

        internal GamePadInputState()
        {
            Triggers = new GamePadTriggers();
            ThumbSticks = new GamePadThumbSticks();
            Buttons = new GamePadButtons();
        }

    }
}
