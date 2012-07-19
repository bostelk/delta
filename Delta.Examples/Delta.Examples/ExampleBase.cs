using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Delta;
using Delta.Input;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ExampleBase : G
    {
        public Color ClearColor { get; set; }

        public ExampleBase(string exampleName) 
            : base(1280, 720, false, true)
        {
            Window.Title = exampleName;
            ClearColor = Color.Black;
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (G.Input.Keyboard.IsPressed(Keys.Escape))
                Exit();
            if ((G.Input.Keyboard.IsDown(Keys.LeftAlt) || G.Input.Keyboard.IsDown(Keys.RightAlt)) && G.Input.Keyboard.IsPressed(Keys.Enter))
                G.ToggleFullScreen();
            if (G.Input.Keyboard.IsPressed(Keys.Pause))
                G.World.TogglePause();
            if (G.Input.Keyboard.IsPressed(Keys.PageUp))
                G.World.TimeScale += 0.1f;
            if (G.Input.Keyboard.IsPressed(Keys.PageDown))
                G.World.TimeScale -= 0.1f;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
