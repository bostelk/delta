using System;
using Microsoft.Xna.Framework;
using Delta.UI.Controls;

namespace Delta.UI.Screens
{
    public enum ScreenState
    {
        None,
        TransitionOn,
        TransitionOff,
    }


    public abstract class Screen : ControlBase
    {
        bool _isExiting = false;

        public PlayerIndex? ControllingPlayer { get; set; }
        public ScreenState State { get; internal set; }
        public float TransitionOnTime { get; set; }
        public float TransitionOffTime { get; set; }
        public float CurrentTransitionProgress { get; set; }

        public Screen()
            : base()
        {
        }

        protected override void LateInitialize()
        {
            base.LateInitialize();
            Size = new Vector2(G.ScreenArea.Width, G.ScreenArea.Height);
        }

        protected override void LightUpdate(DeltaTime time)
        {
            base.LightUpdate(time);
            if (_isExiting)
            {

                //        // If the screen is going away to die, it should transition off.
                //        screenState = ScreenState.TransitionOff;

                //        if (!UpdateTransition(gameTime, transitionOffTime, 1))
                //        {
                //            // When the transition finishes, remove the screen.
                //            ScreenManager.RemoveScreen(this);
                //        }
            }
            if (State == ScreenState.TransitionOn || State == ScreenState.TransitionOff)
            {
                float transitionTime = 0;
                if (State == ScreenState.TransitionOn)
                    transitionTime = TransitionOnTime;
                else if (State == ScreenState.TransitionOff)
                    transitionTime = TransitionOffTime;
                if (CurrentTransitionProgress <= 0)
                    OnTransitionStarted();
                CurrentTransitionProgress = (time.ElapsedSeconds / transitionTime * 100).Clamp(0, 100);
                if (CurrentTransitionProgress >= 100)
                {
                    OnTransitionFinished();
                    State = ScreenState.None;
                }
            }
            //    if (isExiting)
            //    {
            //        // If the screen is going away to die, it should transition off.
            //        screenState = ScreenState.TransitionOff;

            //        if (!UpdateTransition(gameTime, transitionOffTime, 1))
            //        {
            //            // When the transition finishes, remove the screen.
            //            ScreenManager.RemoveScreen(this);
            //        }
            //    }
            //    else if (coveredByOtherScreen)
            //    {
            //        // If the screen is covered by another, it should transition off.
            //        if (UpdateTransition(gameTime, transitionOffTime, 1))
            //        {
            //            // Still busy transitioning.
            //            screenState = ScreenState.TransitionOff;
            //        }
            //        else
            //        {
            //            // Transition finished!
            //            screenState = ScreenState.Hidden;
            //        }
            //    }
            //    else
            //    {
            //        // Otherwise the screen should transition on and become active.
            //        if (UpdateTransition(gameTime, transitionOnTime, -1))
            //        {
            //            // Still busy transitioning.
            //            screenState = ScreenState.TransitionOn;
            //        }
            //        else
            //        {
            //            // Transition finished!
            //            screenState = ScreenState.Active;
            //        }
            //    }
        }

        //public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        //{
        //    this.otherScreenHasFocus = otherScreenHasFocus;

        //    if (isExiting)
        //    {
        //        // If the screen is going away to die, it should transition off.
        //        screenState = ScreenState.TransitionOff;

        //        if (!UpdateTransition(gameTime, transitionOffTime, 1))
        //        {
        //            // When the transition finishes, remove the screen.
        //            ScreenManager.RemoveScreen(this);
        //        }
        //    }
        //    else if (coveredByOtherScreen)
        //    {
        //        // If the screen is covered by another, it should transition off.
        //        if (UpdateTransition(gameTime, transitionOffTime, 1))
        //        {
        //            // Still busy transitioning.
        //            screenState = ScreenState.TransitionOff;
        //        }
        //        else
        //        {
        //            // Transition finished!
        //            screenState = ScreenState.Hidden;
        //        }
        //    }
        //    else
        //    {
        //        // Otherwise the screen should transition on and become active.
        //        if (UpdateTransition(gameTime, transitionOnTime, -1))
        //        {
        //            // Still busy transitioning.
        //            screenState = ScreenState.TransitionOn;
        //        }
        //        else
        //        {
        //            // Transition finished!
        //            screenState = ScreenState.Active;
        //        }
        //    }
        //}

        public void Exit()
        {
            if (TransitionOffTime <= 0)
            {
                if (Parent != null)
                    Parent.Remove(this);
                else
                    G.UI.Remove(this);
            }
            else
                _isExiting = true;
        }

        protected virtual void OnTransitionStarted()
        {
        }

        protected virtual void OnTransitionFinished()
        {
        }

    }
}