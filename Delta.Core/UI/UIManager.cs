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

namespace Delta.UI
{
    public class UIManager : EntityManager<Screen>
    {
        static RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };

        public HUD HUD { get; internal set; }
        public Screen ActiveScreen { get; internal set; }
        public Control FocusedControl { get; set; }
        public Control PressedControl { get; set; }
#if WINDOWS
        public Control EnteredControl { get; set; }
#endif

        internal Action<Keys> _keyDown;
        internal Action<Keys> _keyPress;
        internal Action<Keys> _keyUp;

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

        protected override void LightUpdate(DeltaTime time)
        {
            base.LightUpdate(time);
            HUD.InternalUpdate(time);
        }

        protected override void BeginDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, _rasterizerState, null, Camera.View);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            HUD.InternalDraw(time, spriteBatch);
            base.Draw(time, spriteBatch);
            if (ActiveScreen != null)
                ActiveScreen.InternalDraw(time, spriteBatch);
        }

        public override void Add(Screen item)
        {
            base.Add(item);
            if (ActiveScreen == null)
                ActiveScreen = item;
        }

        public override void Remove(Screen item)
        {
            if (ActiveScreen == item)
                ActiveScreen = null;
            base.Remove(item);
        }

#if WINDOWS
        internal void MouseMove()
        {
            bool handled = false;
            if (EnteredControl != null)
                handled = EnteredControl.ProcessMouseMove();
            if (!handled && ActiveScreen != null)
                handled = ActiveScreen.ProcessMouseMove();
            if (!handled)
                for (int x = 0; x < Children.Count; x++)
                    handled = Children[x].ProcessMouseMove();
            if (!handled)
                handled = HUD.ProcessMouseMove();
            if (!handled && EnteredControl != null)
                EnteredControl.MouseIsInside = false;
        }

        internal void MouseDown()
        {
            bool handled = false;
            if (EnteredControl != null)
                handled = EnteredControl.ProcessMouseDown();
            if (!handled)
                for (int x = 0; x < Children.Count; x++)
                    handled = Children[x].ProcessMouseDown();
            if (!handled)
                handled = HUD.ProcessMouseDown();
            if (!handled && FocusedControl != null)
                FocusedControl.IsFocused = false;
        }

        internal void MouseUp()
        {
            bool handled = false;
            if (EnteredControl != null)
                handled = EnteredControl.ProcessMouseUp();
            if (!handled)
                for (int x = 0; x < Children.Count; x++)
                    handled = Children[x].ProcessMouseUp();
            if (!handled)
                handled = HUD.ProcessMouseUp();
            if (!handled && FocusedControl != null)
                FocusedControl.IsFocused = false;
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
