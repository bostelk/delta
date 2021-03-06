﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Delta.Input.States;

namespace Delta.UI.Controls
{
    public abstract class Control : EntityParent<Control>
    {
        internal Rectangle _renderArea = Rectangle.Empty;

        protected internal Vector2 RenderPosition { get; internal set; }
        protected internal Vector2 RenderSize { get; internal set; }
        protected internal Color RenderColor { get; internal set; }
        public bool IsScissored { get; set; }

        Control _parent = null;
        public Control Parent
        {
            get { return _parent; }
            internal set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnParentChanged();
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

        Vector2 _size = Vector2.Zero;
        public Vector2 Size
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

        bool _isFocusable = true;
        public bool IsFocusable
        {
            get { return _isFocusable; }
            set
            {
                if (_isFocusable != value)
                {
                    _isFocusable = value;
                    if (!value && _isFocused)
                        IsFocused = false;
                }
            }
        }

        bool _isFocused = false;
        public bool IsFocused
        {
            get { return _isFocused; }
            internal set
            {
                if (_isFocused != value && _isFocusable)
                {
                    _isFocused = value;
                    if (value)
                        OnGotFocus();
                    else
                        OnLostFocus();
                    NeedsHeavyUpdate = true;
                }
            }
        }

        bool _isHighlightable = true;
        public bool IsHighlightable
        {
            get { return _isHighlightable; }
            set
            {
                if (_isHighlightable != value)
                {
                    _isHighlightable = value;
                    if (!value && _isHighlighted)
                        IsHighlighted = false;
                }
            }
        }

        bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            internal set
            {
                if (_isHighlighted != value && _isHighlightable)
                {
                    _isHighlighted = value;
                    if (value)
                        OnGotHighlight();
                    else
                        OnLostHighlight();
                    NeedsHeavyUpdate = true;
                }
            }
        }

        bool _isPressed = false;
        public bool IsPressed
        {
            get { return _isPressed; }
            internal set
            {
                if (_isPressed != value)
                {
                    _isPressed = value;
                    if (value)
                        OnPressed();
                    else
                        OnReleased();
                    NeedsHeavyUpdate = true;
                }
            }
        }

#if WINDOWS
        bool _mouseIsInside = false;
        public bool MouseIsInside
        {
            get { return _mouseIsInside; }
            internal set
            {
                if (_mouseIsInside != value)
                {
                    _mouseIsInside = value;
                    if (value)
                        OnMouseEnter();
                    else
                        OnMouseLeave();
                }
            }
        }
#endif

        Color _backColor = Color.Black;
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    UpdateColors();
                }
            }
        }

        Color _disabledColor = Color.Gray;
        public Color DisabledColor
        {
            get { return _disabledColor; }
            set
            {
                if (_disabledColor != value)
                {
                    _disabledColor = value;
                    UpdateColors();
                }
            }
        }

        Color _highlightedColor = Color.SeaGreen;
        public Color HighlightedColor
        {
            get { return _highlightedColor; }
            set
            {
                if (_highlightedColor != value)
                {
                    _highlightedColor = value;
                    UpdateColors();
                }
            }
        }

        Color _pressedColor = Color.DarkViolet;
        public Color PressedColor
        {
            get { return _pressedColor; }
            set
            {
                if (_pressedColor != value)
                {
                    _pressedColor = value;
                    UpdateColors();
                }
            }
        }

        Color _focusedColor = Color.DarkSlateBlue;
        public Color FocusedColor
        {
            get { return _focusedColor; }
            set
            {
                if (_focusedColor != value)
                {
                    _focusedColor = value;
                    UpdateColors();
                }
            }
        }

        float _alpha = 0.5f;
        public virtual float Alpha
        {
            get { return _alpha; }
            set
            {
                value = value.Clamp(0.0f, 1.0f);
                if (_alpha != value)
                {
                    _alpha = value;
                    OnAlphaChanged();
                }
            }
        }

        public Control()
            : base()
        {
            IsScissored = true;
        }

        public void Focus()
        {
            IsFocused = true;
        }

        public void UnFocus()
        {
            IsFocused = false;
        }

        public void Invalidate()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual bool IntersectTest(Vector2 position)
        {
            if (RenderSize.X > 0 && RenderSize.Y > 0)
                if (position.X >= RenderPosition.X)
                    if (position.X <= RenderPosition.X + RenderSize.X)
                        if (position.Y >= RenderPosition.Y)
                            if (position.Y <= RenderPosition.Y + RenderSize.Y)
                                return true;
            return false;
        }

#if WINDOWS
        internal virtual bool ProcessMouseMove()
        {
            if (IntersectTest(G.Input.Mouse.Position))
            {
                MouseIsInside = true;
                IsHighlighted = true;
                OnMouseMove();
                bool handled = false;
                for (int x = Children.Count - 1; x >= 0; x--)
                {
                    handled = Children[x].ProcessMouseMove();
                    if (handled)
                        break;
                }
                return true;
            }
            else
            {
                MouseIsInside = false;
                IsHighlighted = false;
                return false;
            }
        }

        internal virtual bool ProcessMouseDown()
        {
            bool handled = false;
            for (int x = Children.Count - 1; x >= 0; x--)
            {
                handled = Children[x].ProcessMouseDown();
                if (handled)
                    break;
            }
            if (!handled && _mouseIsInside)
            {
                handled = true;
                IsPressed = true;
                IsFocused = true;
                OnMouseDown();
            }
            return handled;
        }

        internal virtual bool ProcessMouseUp()
        {
            bool handled = false;
            for (int x = Children.Count - 1; x >= 0; x--)
            {
                handled = Children[x].ProcessMouseUp();
                if (handled)
                    break;
            }
            if (!handled)
            {
                if (_isPressed || _mouseIsInside)
                {
                    IsPressed = false;
                    handled = true;
                    OnMouseUp();
                }
            }
            return handled;
        }

        protected internal virtual void ProcessKeyDown(Keys key)
        {
            OnKeyDown(key);
        }

        protected internal virtual void ProcessKeyPress(Keys key)
        {
            OnKeyPress(key);
        }

        protected internal virtual void ProcessKeyUp(Keys key)
        {
            OnKeyUp(key);
        }
#endif

        protected internal override void HeavyUpdate(DeltaGameTime time)
        {
            base.HeavyUpdate(time);
            if (Children.Count != 0)
                foreach (Control control in Children)
                    control.NeedsHeavyUpdate = true;
            UpdateRenderPosition();
            UpdateRenderSize();
            UpdateRenderArea();
            UpdateColors();
            UpdateDepth();
        }

        internal virtual void UpdateRenderPosition()
        {
            RenderPosition = Position;
            if (Parent != null)
                RenderPosition += Parent.RenderPosition;
        }

        internal virtual void UpdateRenderSize()
        {
            RenderSize = Size;
        }

        internal virtual void UpdateRenderArea()
        {
            _renderArea = Rectangle.Intersect(
                new Rectangle(
                    (int)RenderPosition.X,
                    (int)RenderPosition.Y,
                    (int)RenderSize.X,
                    (int)RenderSize.Y),
                G.ScreenArea);
            if (Parent != null)
                _renderArea = Rectangle.Intersect(_renderArea, Parent._renderArea);
        }

        internal virtual void UpdateColors()
        {
            if (IsEnabled)
            {
                if (IsPressed)
                    RenderColor = _pressedColor * Alpha;
                else if (IsFocused)
                    RenderColor = _focusedColor * Alpha;
                else if (IsHighlighted)
                    RenderColor = _highlightedColor * Alpha;
                else
                    RenderColor = _backColor * Alpha;
            }
            else
                RenderColor = _disabledColor * Alpha;
        }

        internal virtual void UpdateDepth()
        {
            if (IsFocused)
                Depth = 1.1f;
            else
                Depth = 1.0f;
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(RenderPosition, RenderSize, RenderColor);
        }

        protected internal override void OnAdded()
        {
            base.OnAdded();
            Control parentControl = ParentCollection as Control;
            if (parentControl != null)
                Parent = parentControl;
        }

        protected internal override void OnRemoved()
        {
            base.OnRemoved();
            Parent = null;
        }

        protected override void OnBeginDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            G.GraphicsDevice.ScissorRectangle = IsScissored ? _renderArea : G.ScreenArea;
        }

        protected virtual void OnParentChanged()
        {
        }

        protected virtual void OnPositionChanged()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual void OnSizeChanged()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual void OnAlphaChanged()
        {
            UpdateColors();
        }

        protected virtual void OnGotFocus()
        {
            if (G.UI.FocusedControl != this)
            {
                if (G.UI.FocusedControl != null)
                    G.UI.FocusedControl.IsFocused = false;
                G.UI.FocusedControl = this;
            }
        }

        protected virtual void OnLostFocus()
        {
            IsPressed = false;
            G.UI.FocusedControl = null;
        }

        protected virtual void OnGotHighlight()
        {
            if (G.UI.HighlightedControl != this)
            {
                if (G.UI.HighlightedControl != null)
                    G.UI.HighlightedControl.IsHighlighted = false;
                G.UI.HighlightedControl = this;
            }
        }

        protected virtual void OnLostHighlight()
        {
            if (G.UI.HighlightedControl == this)
                G.UI.HighlightedControl = null;
        }

        protected virtual void OnPressed()
        {
            if (G.UI.PressedControl != this)
            {
                if (G.UI.PressedControl != null)
                    G.UI.PressedControl.IsPressed = false;
                G.UI.PressedControl = this;
            }
        }

        protected virtual void OnReleased()
        {
            if (G.UI.PressedControl == this)
                G.UI.PressedControl = null;
        }

#if WINDOWS
        protected virtual void OnMouseDown()
        {
        }

        protected virtual void OnMouseMove()
        {
        }

        protected virtual void OnMouseUp()
        {
        }

        protected virtual void OnMouseEnter()
        {
        }

        protected virtual void OnMouseLeave()
        {
        }

        protected virtual void OnKeyDown(Keys key)
        {
        }

        protected virtual void OnKeyPress(Keys key)
        {
        }

        protected virtual void OnKeyUp(Keys key)
        {
        }
#endif
    }
}
