using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Delta.Input.States;

namespace Delta.UI.Controls
{
    public abstract class Control : ControlBase
    {
        internal Color _currentColor = Color.White;
        internal Color _currentBorderColor = Color.White;
        internal Control _parentControl = null;

        protected Vector2 RenderBorderSize { get; set; }
        protected Vector2 InnerRenderPosition { get; set; }
        protected Vector2 InnerRenderSize { get; set; }

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

        bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            internal set
            {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
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

        bool _isDragged = false;
        public bool IsDragged
        {
            get { return _isDragged; }
            internal set
            {
                if (_isDragged != value)
                {
                    _isDragged = value;
                    //if (value)
                    //{
                    //    G.UI.DraggedControl = this;
                    //    G.UI._dragStartPosition = G.Input.Mouse.Position;
                    //}
                    //else
                    //    G.UI.DraggedControl = null;
                }
            }
        }

        bool _isDraggable = false;
        public bool IsDraggable
        {
            get { return _isDraggable; }
            internal set
            {
                if (_isDraggable != value)
                {
                    _isDraggable = value;
                }
            }
        }

#endif

        Color _backColor = Color.SeaGreen;
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor != value)
                {
                    _backColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _borderColor = Color.DarkSeaGreen;
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    NeedsHeavyUpdate = true;
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
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _disabledBorderColor = Color.DarkGray;
        public Color DisabledBorderColor
        {
            get { return _disabledBorderColor; }
            set
            {
                if (_disabledBorderColor != value)
                {
                    _disabledBorderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }


        Color _highlightedColor = Color.DarkKhaki;
        public Color HighlightedColor
        {
            get { return _highlightedColor; }
            set
            {
                if (_highlightedColor != value)
                {
                    _highlightedColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _highlightedBorderColor = Color.Khaki;
        public Color HighlightedBorderColor
        {
            get { return _highlightedBorderColor; }
            set
            {
                if (_highlightedBorderColor != value)
                {
                    _highlightedBorderColor = value;
                    NeedsHeavyUpdate = true;
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
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _pressedBorderColor = Color.Violet;
        public Color PressedBorderColor
        {
            get { return _pressedBorderColor; }
            set
            {
                if (_pressedBorderColor != value)
                {
                    _pressedBorderColor = value;
                    NeedsHeavyUpdate = true;
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
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Color _focusedBorderColor = Color.SlateBlue;
        public Color FocusedBorderColor
        {
            get { return _focusedBorderColor; }
            set
            {
                if (_focusedBorderColor != value)
                {
                    _focusedBorderColor = value;
                    NeedsHeavyUpdate = true;
                }
            }
        }

        Point _size = new Point(25, 25);
        public Point Size
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

        Point _borderSize = new Point(1, 1);
        public Point BorderSize
        {
            get { return _borderSize; }
            set
            {
                if (_borderSize != value)
                {
                    _borderSize = value;
                    OnBorderSizeChanged();
                }
            }
        }

        public Control()
            : base()
        {
        }

        protected internal override void HeavyUpdate(DeltaTime time)
        {
            UpdateRenderBorderSize();
            UpdateColors();
            base.HeavyUpdate(time);
            UpdateInnerRenderPosition();
            UpdateInnerRenderSize();
        }

        internal virtual void UpdateRenderBorderSize()
        {
            RenderBorderSize = new Vector2(_borderSize.X, _borderSize.Y);
        }

        internal virtual void UpdateInnerRenderPosition()
        {
            InnerRenderPosition = RenderPosition + RenderBorderSize;
        }

        internal virtual void UpdateInnerRenderSize()
        {
            InnerRenderSize = RenderSize - RenderBorderSize * 2;
        }

        internal virtual void UpdateColors()
        {
            if (IsEnabled)
            {
                if (IsPressed)
                {
                    _currentColor = _pressedColor;
                    _currentBorderColor = _pressedBorderColor;
                }
                else if (IsFocused)
                {
                    _currentColor = _focusedColor;
                    _currentBorderColor = _focusedBorderColor;
                }
                else if (IsHighlighted)
                {
                    _currentColor = _highlightedColor;
                    _currentBorderColor = _highlightedBorderColor;
                }
                else
                {
                    _currentColor = _backColor;
                    _currentBorderColor = _borderColor;
                }
            }
            else
            {
                _currentColor = _disabledColor;
                _currentBorderColor = _disabledBorderColor;
            }
        }

        protected override void BeginDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            G.GraphicsDevice.ScissorRectangle = _cullRectangle;
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            DrawBorder(time, spriteBatch);
            DrawBack(time, spriteBatch);
        }

        protected virtual void DrawBack(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (_currentColor.A <= 0 || InnerRenderSize.X <= 0 || InnerRenderSize.Y <= 0)
                return;
            else
                spriteBatch.DrawRectangle(InnerRenderPosition, InnerRenderSize, _currentColor);
        }

        protected virtual void DrawBorder(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (_currentBorderColor.A <= 0 || RenderSize.X <= 0 || RenderSize.Y <= 0 || RenderBorderSize.X <= 0 || RenderBorderSize.Y <= 0)
                return;
            else
                spriteBatch.DrawRectangle(RenderPosition, RenderSize, _currentBorderColor);
        }

#if WINDOWS
        internal override bool ProcessMouseMove()
        {
            bool handled = false;
            handled = base.ProcessMouseMove();
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(mouse.Position))
                {
                    MouseIsInside = true;
                    //if (mouse.ScrollWheelDelta != 0)
                    //    OnMouseScroll();
                    OnMouseMove();
                    handled = true;
                }
                else
                {
                    MouseIsInside = false;
                    handled = false;
                }
            }
            return handled;
        }

        internal override bool ProcessMouseDown()
        {
            bool handled = false;
            handled = base.ProcessMouseDown();
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(mouse.Position))
                {
                    if (!IsPressed && IsEnabled)
                    {
                        IsPressed = true;
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

        internal override bool ProcessMouseUp()
        {
            bool handled = false;
            handled = base.ProcessMouseUp();
            if (!handled)
            {
                MouseInputState mouse = G.Input.Mouse;
                if (IntersectTest(G.Input.Mouse.Position))
                {
                    IsPressed = false;
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
            if (_isDraggable && !_isDragged)
                IsDragged = true;
        }

        protected virtual void OnMouseUp()
        {
            if (_isDragged)
                IsDragged = false;
        }

        protected virtual void OnPressed()
        {
            //if (G.UI.PressedControl != this)
            //{
            //    if (G.UI.PressedControl != null)
            //        G.UI.PressedControl.OnReleased();
            //    G.UI.PressedControl = this;
            //}
        }

        protected virtual void OnReleased()
        {
            //if (G.UI.PressedControl == this)
            //    G.UI.PressedControl = null;
        }

        protected virtual void OnMouseEnter()
        {
            //if (G.UI.EnteredControl != this)
            //{
            //    if (G.UI.EnteredControl != null)
            //        G.UI.EnteredControl.OnMouseLeave();
            //    G.UI.EnteredControl = this;
            //}
            //IsHighlighted = true;
            //if (G.Input.Mouse.LeftButton.IsDown)
            //{
            //    if (G.UI.PressedControl != this)
            //        return;
            //    IsPressed = true;
            //}
        }

        protected virtual void OnMouseLeave()
        {
            //if (G.UI.EnteredControl == this)
            //    G.UI.EnteredControl = null;
            //IsHighlighted = false;
            //if (G.Input.Mouse.LeftButton.IsDown)
            //{
            //    if (G.UI.PressedControl == this)
            //    {
            //        IsHighlighted = false;
            //        IsPressed = false;
            //    }
            //    else
            //        return;
            //}
        }

        protected virtual void OnMouseScroll()
        {
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

        protected override void OnGotFocus()
        {
            //if (G.UI.FocusedControl != this)
            //{
            //    if (G.UI.FocusedControl != null)
            //        G.UI.FocusedControl.IsFocused = false;
            //    G.UI.FocusedControl = this;
            //}
        }

        protected override void OnLostFocus()
        {
            //IsPressed = false;
            //G.UI.FocusedControl = null;
        }

        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            Control parentControl = Parent as Control;
            if (parentControl != null)
                _parentControl = parentControl;
        }

        protected virtual void OnBorderSizeChanged()
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

        public override string ToString()
        {
            return String.Format("Name: {0}, Position: ({1},{2}), Size: ({3},{4})", Name, RenderPosition.X, RenderPosition.Y, RenderSize.X, RenderSize.Y);
        }

    }
}
