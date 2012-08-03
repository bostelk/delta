using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Delta.Input;
using Delta.Input.States;
using Delta.UI.Screens;
using Delta.UI.Controls;

namespace Delta.UI
{
    public class UIManager : EntityParent<Screen>
    {
        internal Point _dragStartPosition = Point.Zero;

        public HUD HUD { get; internal set; }
        public Control FocusedControl { get; set; }
        public Control PressedControl { get; set; }
        public Control HighlightedControl { get; set; }

        internal Action<Keys> _keyDown = null;
        internal Action<Keys> _keyPress = null;
        internal Action<Keys> _keyUp = null;

        int _keyboardDelay = 0;
        public int KeyboardDelay
        {
            get
            {
                if (_keyboardDelay == 0)
                    _keyboardDelay = System.Windows.Forms.SystemInformation.KeyboardDelay;
                return _keyboardDelay;
            }
        }

        public UIManager()
            : base()
        {
            HUD = new HUD();
            _keyDown = KeyDown;
            _keyPress = KeyPress;
            _keyUp = KeyUp;
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            base.LightUpdate(time);
            HUD.InternalUpdate(time);
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            HUD.InternalDraw(time, spriteBatch);
            base.Draw(time, spriteBatch);
        }

#if WINDOWS
        internal void MouseMove()
        {
            //if (DraggedControl != null)
            //{
            //    Point newDragPosition = G.Input.Mouse.Position;
            //    DraggedControl.Position = new Point(DraggedControl.Position.X + newDragPosition.X - _dragStartPosition.X, DraggedControl.Position.Y + newDragPosition.Y - _dragStartPosition.Y);
            //    _dragStartPosition = newDragPosition;
            //    DraggedControl.Invalidate();
            //}
            bool handled = false;
            if (FocusedControl != null)
                handled = FocusedControl.ProcessMouseMove();
            if (!handled)
                handled = HUD.ProcessMouseMove();
            if (!handled)
                for (int x = Children.Count - 1; x >= 0; x--)
                    handled = Children[x].ProcessMouseMove();
        }

        internal void MouseDown()
        {
            bool handled = false;
            if (FocusedControl != null)
                handled = FocusedControl.ProcessMouseDown();
            handled = HUD.ProcessMouseDown();
            if (!handled)
                for (int x = Children.Count - 1; x >= 0; x--)
                    handled = Children[x].ProcessMouseDown();;
        }

        internal void MouseUp()
        {
            bool handled = false;
            handled = HUD.ProcessMouseUp();
            if (!handled && PressedControl != null)
                handled = PressedControl.ProcessMouseUp();
            if (!handled)
                for (int x = Children.Count - 1; x >= 0; x--)
                    handled = Children[x].ProcessMouseUp();
        }

        internal void KeyDown(Keys key)
        {
            if (FocusedControl != null)
                FocusedControl.ProcessKeyDown(key);
        }

        internal void KeyPress(Keys key)
        {
            if (FocusedControl != null)
                FocusedControl.ProcessKeyPress(key);
        }

        internal void KeyUp(Keys key)
        {
            if (FocusedControl != null)
                FocusedControl.ProcessKeyUp(key);
        }
#endif

    }
}
