//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework;

//namespace Delta.Input
//{
//    public struct ThumbSticks
//    {
//        public Vector2 Left;
//        public Vector2 Right;
//    }

//    public struct Triggers
//    {
//        public float Left;
//        public float Right;
//    }

//    public class GamePadHelper
//    {
//        GamePadState _thisState;
//        GamePadState _previousState;

//        public PlayerIndex Player
//        {
//            get;
//            private set;
//        }

//        public ThumbSticks Thumbsticks
//        {
//            get
//            {
//                return new ThumbSticks()
//                {
//                    Left = _thisState.ThumbSticks.Left,
//                    Right = _thisState.ThumbSticks.Right,
//                };
//            }
//        }

//        public Triggers Triggers
//        {
//            get
//            {
//                return new Triggers()
//                {
//                    Left = _thisState.Triggers.Left,
//                    Right = _thisState.Triggers.Right,
//                };
//            }
//        }

//        public bool IsVibrating
//        {
//            get;
//            private set;
//        }

//        public bool IsConnected
//        {
//            get
//            {
//                return _thisState.IsConnected;
//            }
//        }

//        public GamePadHelper(PlayerIndex player)
//        {
//            Player = player;
//            _thisState = _previousState = GamePad.GetState(Player);
//        }

//        public bool JustPressed(Buttons button)
//        {
//            return _thisState.IsButtonDown(button) && _thisState.IsButtonUp(button);
//        }

//        public bool JustReleased(Buttons button)
//        {
//            return _thisState.IsButtonUp(button) && _thisState.IsButtonDown(button);
//        }

//        public bool Held(Buttons button)
//        {
//            return _thisState.IsButtonDown(button);
//        }

//        public void SetVibration(float left, float right, float duration) {
//            SetVibration(left, right);
//        }

//        public void SetVibration(float left, float right)
//        {
//            IsVibrating = (left != 0 && right != 0);
//            GamePad.SetVibration(Player, left, right);
//        }

//        public void SetVibrationAtFrom(Vector2 emitter, Vector2 listener, float magnitude)
//        {
//            Vector2 secondVector;
//            secondVector = listener - emitter;
//            secondVector.Normalize();
//            float num = Vector2.Distance(emitter, listener);
//            num = 1f - num / (magnitude * 20f);
//            float num2 = Angle(Vector2.UnitX, secondVector) / 3.14159274f;
//            float num3 = Angle(-Vector2.UnitX, secondVector) / 3.14159274f;
//            float iLeft = num * (1f - num3);
//            float iRight = num * (1f - num2);
//            SetVibration(iLeft, iRight);
//        }

//        private float Angle(Vector2 vector, Vector2 secondVector)
//        {
//            float value;
//            Vector2.Dot(ref vector, ref secondVector, out value);
//            return (float)Math.Acos((double)MathHelper.Clamp(value, -1f, 1f));
//        }

//        public void Update(GameTime gameTime)
//        {
//            _previousState = _thisState;
//            _thisState = GamePad.GetState(Player);
//        }
//    }
//}
