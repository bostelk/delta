using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.UI.Controls;
using Delta.UI.Screens.Transitions;

namespace Delta.UI.Screens
{
    public enum ScreenState
    {
        None,
        TransitionOn,
        TransitionOff,
    }

    public abstract class Screen : EntityParent<Control>
    {
        Transition _currentTransition = null;

        public ScreenState State { get; internal set; }
        public Transition OnTransition { get; set; }
        public Transition OffTransition { get; set; }

        public Screen()
            : base()
        {
        }

#if WINDOWS
        internal virtual bool ProcessMouseMove()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseMove();
                if (handled)
                    break;
            }
            return handled;
        }

        internal virtual bool ProcessMouseDown()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseDown();
                if (handled)
                    break;
            }
            return handled;
        }

        internal virtual bool ProcessMouseUp()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
            {
                handled = Children[x].ProcessMouseUp();
                if (handled)
                    break;
            }
            return handled;
        }
#endif

        protected override void LightUpdate(DeltaGameTime time)
        {
            base.LightUpdate(time);
            if (_currentTransition == null)
            {
                if (State == ScreenState.TransitionOn)
                    _currentTransition = OnTransition;
                else if (State == ScreenState.TransitionOff)
                    _currentTransition = OffTransition;
                else
                    return;
            }
            if (_currentTransition == null)
            {
                State = ScreenState.None;
                return;
            }
            if (_currentTransition.Progress <= 0)
                OnTransitionStarted();
            _currentTransition.Progress = (time.ElapsedSeconds / _currentTransition.Time * 100).Clamp(0, 100);
            if (_currentTransition.Progress == 100)
            {
                OnTransitionFinished();
                State = ScreenState.None;
                _currentTransition = null;
                return;
            }
            _currentTransition.InternalUpdate(time);
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
            if (_currentTransition != null)
                _currentTransition.InternalDraw(time, spriteBatch);
        }

        public void Exit()
        {
            if (OffTransition == null || OffTransition.Time <= 0)
                ExitImmediately();
            else
                State = ScreenState.TransitionOff;
        }

        public void ExitImmediately()
        {
            if (ParentCollection != null)
                ParentCollection.UnsafeRemove(this);
            else
                G.UI.Remove(this);
        }

        protected override void OnBeginDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnBeginDraw(time, spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, SpriteBatchExtensions._cullRasterizerState, null, _currentTransition == null ? Matrix.Identity : _currentTransition.View);
        }

        protected override void OnEndDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            base.OnEndDraw(time, spriteBatch);
        }

        protected virtual void OnTransitionStarted()
        {
            To DO //MAKE ON/OFF TRANSITIONS ENCAPSULATE ONE CLASS
                // use basiceffect? We want to alpha things/out
        }

        protected virtual void OnTransitionFinished()
        {
        }

    }
}