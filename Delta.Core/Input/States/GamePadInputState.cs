using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Delta.Input.States
{
    public sealed class GamePadInputState
    {
        PlayerIndex _playerIndex = PlayerIndex.One;

        public GamePadTriggers Triggers { get; internal set; }
        public GamePadThumbSticks ThumbSticks { get; internal set; }
        public GamePadButtons Buttons { get; internal set; }
        public bool IsVibrating { get; internal set; }

        internal GamePadInputState(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
            Triggers = new GamePadTriggers();
            ThumbSticks = new GamePadThumbSticks();
            Buttons = new GamePadButtons();
        }

        public void Vibrate(float leftMotor, float rightMotor)
        {
            IsVibrating = (leftMotor != 0 && rightMotor != 0);
            GamePad.SetVibration(_playerIndex, leftMotor, rightMotor);
        }

        public void Vibrate(Vector2 emitter, Vector2 listener, float magnitude)
        {
            Vector2 secondVector;
            secondVector = listener - emitter;
            secondVector.Normalize();
            float num = Vector2.Distance(emitter, listener);
            num = 1f - num / (magnitude * 20f);
            float num2 = Angle(Vector2.UnitX, secondVector) / 3.14159274f;
            float num3 = Angle(-Vector2.UnitX, secondVector) / 3.14159274f;
            float iLeft = num * (1f - num3);
            float iRight = num * (1f - num2);
            Vibrate(iLeft, iRight);
        }

        private float Angle(Vector2 vector, Vector2 secondVector)
        {
            float value;
            Vector2.Dot(ref vector, ref secondVector, out value);
            return (float)Math.Acos((double)MathHelper.Clamp(value, -1f, 1f));
        }

    }
}
