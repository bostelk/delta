//using System;
//using Microsoft.Xna.Framework.Input;

//namespace Delta.Input.Mapping
//{
//    public sealed class KeyboardMouseControlMapping
//    {
//        internal PlayerInputCollection _inputParent;

//        public KeyboardMouseControlMap LeftTrigger { get; set; }
//        public KeyboardMouseControlMap RightTrigger { get; set; }
//        public KeyboardMouseControlMap LeftBumper { get; set; }
//        public KeyboardMouseControlMap RightBumper { get; set; }
//        public KeyboardMouseControlMap LeftStickClick { get; set; }
//        public KeyboardMouseControlMap RightStickClick { get; set; }
//        public KeyboardMouseControlMap Back { get; set; }
//        public KeyboardMouseControlMap Start { get; set; }
//        public KeyboardMouseControlMap LeftStickUp { get; set; }
//        public KeyboardMouseControlMap LeftStickDown { get; set; }
//        public KeyboardMouseControlMap LeftStickLeft { get; set; }
//        public KeyboardMouseControlMap LeftStickRight { get; set; }
//        public KeyboardMouseControlMap DPadUp { get; set; }
//        public KeyboardMouseControlMap DPadDown { get; set; }
//        public KeyboardMouseControlMap DPadLeft { get; set; }
//        public KeyboardMouseControlMap DPadRight { get; set; }
//        public KeyboardMouseControlMap X { get; set; }
//        public KeyboardMouseControlMap Y { get; set; }
//        public KeyboardMouseControlMap A { get; set; }
//        public KeyboardMouseControlMap B { get; set; }
//        public KeyboardMouseControlMap RightStickUp { get; set; }
//        public KeyboardMouseControlMap RightStickDown { get; set; }
//        public KeyboardMouseControlMap RightStickLeft { get; set; }
//        public KeyboardMouseControlMap RightStickRight { get; set; }

//        public KeyboardMouseControlMapping()
//        {
//            LeftStickUp = Keys.W;
//            LeftStickDown = Keys.S;
//            LeftStickLeft = Keys.A;
//            LeftStickRight = Keys.D;
//#if WINDOWS
//            RightStickUp = MouseInput.Y;
//            RightStickDown = MouseInput.Y;
//            RightStickLeft = MouseInput.X;
//            RightStickRight = MouseInput.X;
//            LeftTrigger = MouseInput.LeftButton;
//            RightTrigger = MouseInput.RightButton;
//#else
//            RightStickUp = Keys.I;
//            RightStickDown = Keys.K;
//            RightStickLeft = Keys.J;
//            RightStickRight = Keys.L;
//            LeftTrigger = Keys.LeftShift;
//            RightTrigger = Keys.RightShift;
//#endif
//            DPadUp = Keys.Up;
//            DPadDown = Keys.Down;
//            DPadLeft = Keys.Left;
//            DPadRight = Keys.Right;
//            LeftBumper = Keys.U;
//            RightBumper = Keys.O;
//            A = Keys.Space;
//            B = Keys.E;
//            X = Keys.X;
//            Y = Keys.C;
//            Back = Keys.Back;
//            Start = Keys.Enter;
//            LeftStickClick = Keys.Q;
//            RightStickClick = Keys.O;
//            LeftBumper = Keys.F;
//            RightBumper = Keys.H;
//        }

//        public bool TestForAnalogDigitalConflicts()
//        {
//            KeyboardMouseControlMap[] maps = new KeyboardMouseControlMap[] { LeftStickUp, RightStickUp, LeftStickDown, RightStickDown, LeftStickLeft, RightStickLeft, LeftStickRight, RightStickRight, LeftTrigger, RightTrigger, LeftBumper, RightBumper, A, B, X, Y, Back, Start, DPadUp, DPadDown, DPadLeft, DPadRight, LeftStickClick, RightStickClick };

//            for (int i = 1; i < maps.Length; i++)
//            {
//                if (maps[i].IsAnalog)
//                    continue;
//            }
//            //make sure buttons are digital
//            maps = new KeyboardMouseControlMap[] { LeftBumper, RightBumper, A, B, X, Y, Back, Start, DPadUp, DPadDown, DPadLeft, DPadRight, LeftStickClick, RightStickClick };
//            foreach (KeyboardMouseControlMap map in maps)
//                if (map.IsAnalog)
//                    return true;

//            //directions must be analog or digital, not both
//            if (LeftStickUp.IsAnalog != LeftStickDown.IsAnalog ||
//                LeftStickLeft.IsAnalog != LeftStickRight.IsAnalog ||
//                RightStickUp.IsAnalog != RightStickDown.IsAnalog ||
//                RightStickLeft.IsAnalog != RightStickRight.IsAnalog)
//                return true;

//            return false;
//        }

//        public bool TestForConflicts()
//        {
//            KeyboardMouseControlMap[] maps = new KeyboardMouseControlMap[] { LeftStickUp, RightStickUp, LeftStickDown, RightStickDown, LeftStickLeft, RightStickLeft, LeftStickRight, RightStickRight, LeftTrigger, RightTrigger, LeftBumper, RightBumper, A, B, X, Y, Back, Start, DPadUp, DPadDown, DPadLeft, DPadRight, LeftStickClick, RightStickClick };
//            Array.Sort<KeyboardMouseControlMap>(maps);
//            for (int i = 1; i < maps.Length; i++)
//            {
//                if (maps[i - 1].CompareTo(maps[i]) == 0)
//                    return true;
//            }
//            return false;
//        }
//    }        
//}
