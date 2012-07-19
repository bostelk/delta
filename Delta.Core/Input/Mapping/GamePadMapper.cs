//#if WINDOWS

//using System;
//using Microsoft.Xna.Framework.Input;
//using Delta.Input.States;

//namespace Delta.Input.Mapping
//{
//        public class GamePadMapper
//        {
//            PlayerInputCollection _inputParent = null;
//            public bool IsWindowFocused { get; private set; }

//            internal void SetKeyboardMouseState(ref KeyboardState keyboardState, ref MouseState mouseState, ControlInput inputType)
//            {
//                KeyboardMouseState.KeyboardState = gameState.KeyboardState;
//                KeyboardMouseState.MouseState = keyboardState;
//            }

//            internal void Update(ref GamePadInputState gamePadState, KeyboardMouseControlMapping mapping, ControlInput inputType, bool windowFocused)
//            {
//                GamePadInputState gps;

//                switch (inputType)
//                {
//#if !XBOX360
//                    case ControlInput.KeyboardMouse:

//                        _keyboardMouseState.WindowFocused = windowFocused;
//                        Update(gameState, gamePadState, KeyboardMouseState, mapping);
//                        return;
//#endif
//                    case ControlInput.GamePad1:
//                        gameState.GetGamePadState(0, out gps);
//                        UpdateState(gameState, gamePadState, gps);
//                        return;
//                    case ControlInput.GamePad2:
//                        gameState.GetGamePadState(1, out gps);
//                        UpdateState(gameState, gamePadState, gps);
//                        return;
//                    case ControlInput.GamePad3:
//                        gameState.GetGamePadState(2, out gps);
//                        UpdateState(gameState, gamePadState, gps);
//                        return;
//                    case ControlInput.GamePad4:
//                        gameState.GetGamePadState(3, out gps);
//                        UpdateState(gameState, gamePadState, gps);
//                        return;
//                }
//                throw new ArgumentException();
//            }

//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetFaceButtonStates(UpdateState gameState, InputState state, bool AButtonState, bool BButtonState, bool XButtonState, bool YButtonState)
//            {
//                long tick = gameState.TotalTimeTicks;

//                state.buttons.a.SetState(AButtonState, tick);
//                state.buttons.b.SetState(BButtonState, tick);
//                state.buttons.x.SetState(XButtonState, tick);
//                state.buttons.y.SetState(YButtonState, tick);
//            }
//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetDpadStates(UpdateState gameState, InputState state, bool DPadDownButtonState, bool DPadUpButtonState, bool DPadLeftButtonState, bool DPadRightButtonState)
//            {
//                long tick = gameState.TotalTimeTicks;

//                state.buttons.dpadD.SetState(DPadDownButtonState, tick);
//                state.buttons.dpadU.SetState(DPadUpButtonState, tick);
//                state.buttons.dpadL.SetState(DPadLeftButtonState, tick);
//                state.buttons.dpadR.SetState(DPadRightButtonState, tick);
//            }
//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetShoulderButtonStates(UpdateState gameState, InputState state, bool leftShoulderButtonState, bool rightShoulderButtonState)
//            {
//                long tick = gameState.TotalTimeTicks;

//                state.buttons.shoulderL.SetState(leftShoulderButtonState, tick);
//                state.buttons.shoulderR.SetState(rightShoulderButtonState, tick);
//            }
//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetStickButtonStates(UpdateState gameState, InputState state, bool leftStickClickButtonState, bool rightStickClickButtonState)
//            {
//                long tick = gameState.TotalTimeTicks;

//                state.buttons.leftStickClick.SetState(leftStickClickButtonState, tick);
//                state.buttons.rightStickClick.SetState(rightStickClickButtonState, tick);
//            }
//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetSpecialButtonStates(UpdateState gameState, InputState state, bool backButtonState, bool startButtonState)
//            {
//                long tick = gameState.TotalTimeTicks;

//                state.buttons.back.SetState(backButtonState, tick);
//                state.buttons.start.SetState(startButtonState, tick);
//            }
//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetTriggerStates(UpdateState gameState, InputState state, float leftTriggerState, float rightTriggerState)
//            {
//                state.triggers.leftTrigger = leftTriggerState;
//                state.triggers.rightTrigger = rightTriggerState;
//            }
//            /// <summary>
//            /// Method that may be used when overriding one of the 'UpdateState' methods
//            /// </summary>
//            protected void SetStickStates(UpdateState gameState, InputState state, Vector2 leftStickState, Vector2 rightStickState)
//            {
//                state.sticks.leftStick = leftStickState;
//                state.sticks.rightStick = rightStickState;
//            }

//            /// <summary>
//            /// Override this method to change how raw keyboard and mouse input values are translated to a <see cref="InputState"/> object
//            /// </summary>
//            /// <param name="gameState"></param>
//            /// <param name="state">structure to write input state values</param>
//            /// <param name="inputState">stores raw keyboard state</param>
//            /// <param name="mapping">stores the current mapped input values</param>
//            protected virtual void UpdateState(UpdateState gameState, InputState state, KeyboardMouseState inputState, KeyboardMouseControlMapping mapping)
//            {
//                long tick = gameState.TotalTimeTicks;

//                state.buttons.a.SetState(mapping.A.GetValue(inputState), tick);
//                state.buttons.b.SetState(mapping.B.GetValue(inputState), tick);
//                state.buttons.x.SetState(mapping.X.GetValue(inputState), tick);
//                state.buttons.y.SetState(mapping.Y.GetValue(inputState), tick);

//                state.buttons.dpadD.SetState(mapping.DpadDown.GetValue(inputState), tick);
//                state.buttons.dpadU.SetState(mapping.DpadUp.GetValue(inputState), tick);
//                state.buttons.dpadL.SetState(mapping.DpadLeft.GetValue(inputState), tick);
//                state.buttons.dpadR.SetState(mapping.DpadRight.GetValue(inputState), tick);

//                state.buttons.shoulderL.SetState(mapping.LeftShoulder.GetValue(inputState), tick);
//                state.buttons.shoulderR.SetState(mapping.RightShoulder.GetValue(inputState), tick);

//                state.buttons.back.SetState(mapping.Back.GetValue(inputState), tick);
//                state.buttons.start.SetState(mapping.Start.GetValue(inputState), tick);
//                state.buttons.leftStickClick.SetState(mapping.LeftStickClick.GetValue(inputState), tick);
//                state.buttons.rightStickClick.SetState(mapping.RightStickClick.GetValue(inputState), tick);

//                state.triggers.leftTrigger = mapping.LeftTrigger.GetValue(inputState, false);
//                state.triggers.rightTrigger = mapping.RightTrigger.GetValue(inputState, false);

//                Vector2 v = new Vector2();

//                v.Y = mapping.LeftStickForward.GetValue(inputState, false) + mapping.LeftStickBackward.GetValue(inputState, true);
//                v.X = mapping.LeftStickLeft.GetValue(inputState, true) + mapping.LeftStickRight.GetValue(inputState, false);

//                state.sticks.leftStick = v;

//                v.Y = mapping.RightStickForward.GetValue(inputState, false) + mapping.RightStickBackward.GetValue(inputState, true);
//                v.X = mapping.RightStickLeft.GetValue(inputState, true) + mapping.RightStickRight.GetValue(inputState, false);

//                state.sticks.rightStick = v;
//            }
//            /// <summary>
//            /// Override this method to change how raw <see cref="GamePadInputState"/> values are translated to a <see cref="InputState"/> object
//            /// </summary>
//            /// <param name="gameState"></param>
//            /// <param name="input"></param>
//            /// <param name="state"></param>
//            protected virtual void UpdateState(UpdateState gameState, GamePadInputState input, GamePadInputState state)
//            {
//                long tick = gameState.TotalTimeTicks;

//                input.buttons.a.SetState(state.Buttons.A == ButtonState.Pressed, tick);
//                input.buttons.b.SetState(state.Buttons.B == ButtonState.Pressed, tick);
//                input.buttons.x.SetState(state.Buttons.X == ButtonState.Pressed, tick);
//                input.buttons.y.SetState(state.Buttons.Y == ButtonState.Pressed, tick);

//                input.buttons.shoulderL.SetState(state.Buttons.LeftShoulder == ButtonState.Pressed, tick);
//                input.buttons.shoulderR.SetState(state.Buttons.RightShoulder == ButtonState.Pressed, tick);

//                input.buttons.dpadD.SetState(state.DPad.Down == ButtonState.Pressed, tick);
//                input.buttons.dpadL.SetState(state.DPad.Left == ButtonState.Pressed, tick);
//                input.buttons.dpadR.SetState(state.DPad.Right == ButtonState.Pressed, tick);
//                input.buttons.dpadU.SetState(state.DPad.Up == ButtonState.Pressed, tick);

//                input.buttons.back.SetState(state.Buttons.Back == ButtonState.Pressed, tick);
//                input.buttons.start.SetState(state.Buttons.Start == ButtonState.Pressed, tick);
//                input.buttons.leftStickClick.SetState(state.Buttons.LeftStick == ButtonState.Pressed, tick);
//                input.buttons.rightStickClick.SetState(state.Buttons.RightStick == ButtonState.Pressed, tick);

//                input.triggers.leftTrigger = state.Triggers.Left;
//                input.triggers.rightTrigger = state.Triggers.Right;

//                input.sticks.leftStick = state.ThumbSticks.Left;
//                input.sticks.rightStick = state.ThumbSticks.Right;
//            }
//        }
