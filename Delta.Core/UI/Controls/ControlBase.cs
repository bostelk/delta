using System;
using Microsoft.Xna.Framework;
using Delta.UI.Screens;

namespace Delta.UI.Controls
{
    public class ControlBase : EntityParent<ControlBase>
    {
        internal Rectangle _cullRectangle = Rectangle.Empty;
        internal Rectangle _renderArea = Rectangle.Empty;

        public Screen ParentScreen { get; internal set; }
        protected Vector2 RenderPosition { get; set; }
        protected Vector2 RenderSize { get; set; }

        ControlBase _parent = null;
        public ControlBase Parent
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
                if (_isFocused != value)
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

        public ControlBase()
            : base()
        {
        }

        public override void Add(ControlBase item)
        {
            base.Add(item);
            item.Parent = this;
        }

        public override void Remove(ControlBase item)
        {
            base.Remove(item);
            item.Parent = null;
            item.ParentScreen = null;
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

        protected internal override void HeavyUpdate(DeltaGameTime time)
        {
            base.HeavyUpdate(time);
            if (Children.Count != 0)
                foreach (ControlBase control in Children)
                    control.NeedsHeavyUpdate = true;
            UpdateRenderPosition();
            UpdateRenderSize();
            UpdateCullRectangle();
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

        internal virtual void UpdateCullRectangle()
        {
            _cullRectangle = Rectangle.Intersect(
                new Rectangle(
                    (int)RenderPosition.X, 
                    (int)RenderPosition.Y, 
                    (int)RenderSize.X, 
                    (int)RenderSize.Y), 
                G.ScreenArea);
            if (Parent != null)
                _cullRectangle = Rectangle.Intersect(_cullRectangle, Parent._cullRectangle);
        }

        protected override void OnBeginDraw(DeltaGameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            G.GraphicsDevice.ScissorRectangle = _cullRectangle;
        }

        protected virtual void OnGotFocus()
        {
            //if (G.UI.FocusedControl != this)
            //{
            //    if (G.UI.FocusedControl != null)
            //        G.UI.FocusedControl.IsFocused = false;
            //    G.UI.FocusedControl = this;
            //}
        }

        protected virtual void OnLostFocus()
        {
            //IsPressed = false;
            //G.UI.FocusedControl = null;
        }

        protected virtual void OnParentChanged()
        {
            //Screen parentScreen = Parent as Screen;
            //if (parentScreen != null)
            //{
            //    ParentScreen = parentScreen;
            //    for (int x = 0; x < this.Children.Count; x++)
            //        Children[x].ParentScreen = parentScreen;
            //}
        }

        protected virtual void OnPositionChanged()
        {
            NeedsHeavyUpdate = true;
        }

        protected virtual void OnSizeChanged()
        {
            NeedsHeavyUpdate = true;
        }

    }
}
