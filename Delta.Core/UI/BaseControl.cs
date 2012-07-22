using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Delta.Input;
using Delta.Input.States;

namespace Delta.UI
{
    public abstract class BaseControl : EntityCollection<BaseControl>
    {
        bool _needsToInvalidate = true;

        public bool IsClicked { get; internal set; }
        public bool IsFocusable { get; set; }
        public Vector2 AbsolutePosition { get; internal set; }
        protected Vector2 RenderSize { get; set; }

#if WINDOWS
        public bool MouseIsInside { get; private set; }
#endif

        BaseScreen _parentScreen = null;
        public BaseScreen ParentScreen
        {
            get { return _parentScreen; }
            set
            {
                if (_parentScreen != value)
                {
                    _parentScreen = value;
                    OnPositionChanged();
                }
            }
        }

        BaseControl _parent = null;
        public BaseControl Parent 
        {
            get { return _parent; }
            set 
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnPositionChanged();
                }
            }
        }

        Vector2 _position = Vector2.Zero;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPositionChanged();
                }
            }
        }

        Vector2 _size;
        public virtual Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged();
                }
            }
        }

        bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnSelectedChanged();
                }
            }
        }

        bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    OnHighlightedChanged();
                }
            }
        }

        bool _isFocused = false;
        public bool IsFocused
        {
            get { return _isFocused; }
            set
            {
                if (_isFocused != value)
                {
                    _isFocused = value;
                    if (value)
                        OnGotFocus();
                    else
                        OnLostFocus();
                }
            }
        }

        public BaseControl()
            : base()
        {
            IsFocusable = true;
        }
       
        public BaseControl(string name)
            : this()
        {
            Name = name;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void InternalUpdate(DeltaTime time)
        {
            base.InternalUpdate(time);
            if (_needsToInvalidate)
            {
                _needsToInvalidate = false;
                OnInvalidate();
            }
        }

        public void Invalidate()
        {
            _needsToInvalidate = true;
        }

        protected internal virtual void OnInvalidate()
        {
            UpdateAbsolutePosition();
            UpdateRenderSize();
        }

        protected virtual void UpdateAbsolutePosition()
        {
            AbsolutePosition = _position;
            if (_parent != null)
                AbsolutePosition += _parent.AbsolutePosition;
            if (_parentScreen != null)
                AbsolutePosition += _parentScreen.Position;
            if (Children.Count != 0)
            {
                foreach (BaseControl control in Children)
                {
                    control.UpdateAbsolutePosition();
                    control.Invalidate();
                }
            }
        }

        protected virtual void UpdateRenderSize()
        {
            RenderSize = Size;
        }

#if WINDOWS
        protected internal virtual bool ProcessMouseMove()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
                handled = Children[x].ProcessMouseMove();
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(mouse.Position))
                {
                    if (!MouseIsInside)
                        OnMouseEnter();
                    //if (mouse.ScrollWheelDelta != 0)
                    //    OnMouseScroll();
                    OnMouseMove();
                    handled = true;
                }
                else
                {
                    if (MouseIsInside)
                        OnMouseLeave();
                    handled = false;
                }
            }
            return handled;
        }

        internal virtual bool ProcessMouseDown()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
                handled = Children[x].ProcessMouseDown();
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(mouse.Position))
                {
                    if (!IsClicked && IsEnabled)
                    {
                        OnMouseDown();
                        if (IsFocusable && !IsFocused)
                            IsFocused = true;
                    }
                    handled = true;
                }
                else
                    handled = false;
            }
            return handled;
        }

        internal virtual bool ProcessMouseUp()
        {
            bool handled = false;
            for (int x = 0; x < Children.Count; x++)
                handled = Children[x].ProcessMouseUp();
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(G.Input.Mouse.Position))
                {
                    if (IsClicked)
                        OnMouseClick();
                    OnMouseUp();
                    handled = true;
                }
                else
                    handled = false;
            }
            return handled;
        }

        protected virtual void OnMouseMove()
        {
        }

        protected virtual void OnMouseDown()
        {
            IsClicked = true;
            G.UI.ClickedControl = this;
        }

        protected virtual void OnMouseClick()
        {
        }

        protected virtual void OnMouseUp()
        {
            IsClicked = false;
            G.UI.ClickedControl = null;
        }

        //protected virtual void OnMouseCaptureChanged()
        //{
        //    G.UI.CaptureControl = null;
        //    IsClicked = false;
        //}

        protected virtual void OnMouseEnter()
        {
            MouseIsInside = true;
            BaseControl enteredControl = G.UI.EnteredControl;
            if (enteredControl != this)
            {
                if (enteredControl != null)
                    enteredControl.OnMouseLeave();
                enteredControl = this;
            }
        }

        protected virtual void OnMouseLeave()
        {
            MouseIsInside = false;
            if (G.UI.EnteredControl == this)
                G.UI.EnteredControl = null;
        }

        protected virtual void OnMouseScroll()
        {
        }
#endif

        public virtual void ProcessKeyDown()
        {
            OnKeyDown();
        }

        public virtual void ProcessKeyPress()
        {
            OnKeyPress();
        }

        protected virtual void OnKeyDown()
        {
        }

        protected virtual void OnKeyPress()
        {
        }

        protected virtual void OnKeyUp()
        {
        }

        protected virtual void OnGotFocus()
        {
            if (G.UI.FocusedControl != this)
            {
                if (G.UI.FocusedControl != null)
                    G.UI.FocusedControl.IsFocused = false;
                G.UI.FocusedControl = this;
            }
            Invalidate();
        }

        protected virtual void OnLostFocus()
        {
            IsClicked = false;
            G.UI.FocusedControl = null;
            Invalidate();
        }

        protected virtual void OnSelectedChanged()
        {
        }

        protected virtual void OnHighlightedChanged()
        {
        }

        protected virtual void OnSizeChanged()
        {
            UpdateRenderSize();
        }

        protected virtual void OnPositionChanged()
        {
            UpdateAbsolutePosition();
        }

        protected virtual bool IntersectTest(Vector2 point)
        {
            bool returnValue = false;
            if (RenderSize.X > 0 && RenderSize.Y > 0)
                if (point.X >= AbsolutePosition.X)
                    if (point.X <= AbsolutePosition.X + RenderSize.X)
                        if (point.Y >= AbsolutePosition.Y)
                            if (point.Y <= AbsolutePosition.Y + RenderSize.Y)
                                returnValue = true;
            return returnValue;
        }

        public void Focus()
        {
            IsFocused = true;
        }

        public override string ToString()
        {
            return String.Format("Name: {0}, Position: {1}, Size: ({2},{3})", Name, AbsolutePosition, RenderSize.X, RenderSize.Y);
        }
    }
}