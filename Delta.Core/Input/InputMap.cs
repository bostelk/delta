//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

//namespace Delta.Input
//{
//    public enum Action
//    {
//        JustPressed,
//        JustReleased,
//        Held
//    }

//    public struct KeyboardActions
//    {
//        public Queue<KeyboardAction> _actions;

//        public KeyboardActions Chain(KeyboardAction action)
//        {
//            _actions.Enqueue(action);
//            return this;
//        }
//    }

//    public interface IAction
//    {
//        Action Action { get; set; }
//    }

//    public struct KeyboardAction : IAction
//    {
//        public Action Action { get; set; }
//        public Keys Key;
//    }

//    public struct GamePadAction : IAction
//    {
//        public Action Action { get; set; }
//        public PlayerIndex Player;
//        public Buttons Button;
//    }

//    public struct MouseAction : IAction
//    {
//        public Action Action { get; set; }
//        public MouseButton Button;
//    }

//    public class InputMap<T>
//    {
//        private Dictionary<T, IAction> _actions;

//        public InputMap()
//        {
//            _actions = new Dictionary<T, IAction>();
//        }

//        public void MapAction(T index, KeyboardAction action)
//        {
//            _actions[index] = action;
//        }

//        public void MapAction(T index, MouseAction action)
//        {
//            _actions[index] = action;
//        }

//        public void MapAction(T index, GamePadAction action)
//        {
//            _actions[index] = action;
//        }

//        public bool CheckAction(T index)
//        {
//            if (_actions.ContainsKey(index))
//            {
//                IAction action = _actions[index];
//                if (action is KeyboardAction)
//                {
//                    KeyboardAction ka = (KeyboardAction)action;
//                    switch (ka.Action)
//                    {
//                        case Action.Held:
//                            return G.Input.Keyboard.Held(ka.Key);
//                        case Action.JustPressed:
//                            return G.Input.Keyboard.JustPressed(ka.Key);
//                        case Action.JustReleased:
//                            return G.Input.Keyboard.JustReleased(ka.Key);
//                        default:
//                            throw new InvalidOperationException("Which action?");
//                    }
//                }
//                else if (action is GamePadAction)
//                {
//                    GamePadAction ka = (GamePadAction)action;
//                    switch (ka.Action)
//                    {
//                        case Action.Held:
//                            return G.Input.Gamepad[(int)ka.Player].Held(ka.Button);
//                        case Action.JustPressed:
//                            return G.Input.Gamepad[(int)ka.Player].JustPressed(ka.Button);
//                        case Action.JustReleased:
//                            return G.Input.Gamepad[(int)ka.Player].JustReleased(ka.Button);
//                        default:
//                            throw new InvalidOperationException("Which action?");
//                    }
//                }
//                else if (action is MouseAction)
//                {
//                    MouseAction ka = (MouseAction)action;
//                    switch (ka.Action)
//                    {
//                        case Action.Held:
//                            return G.Input.Mouse.Held(ka.Button);
//                        case Action.JustPressed:
//                            return G.Input.Mouse.JustPressed(ka.Button);
//                        case Action.JustReleased:
//                            return G.Input.Mouse.JustReleased(ka.Button);
//                        default:
//                            throw new InvalidOperationException("Which action?");
//                    }
//                }
//                else
//                {
//                    throw new InvalidOperationException("Not a recognized action");
//                }
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
//}
