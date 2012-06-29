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
    public class ExampleBase : DeltaGame
    {

        public Color ClearColor { get; set; }

        PerformanceMetrics _performanceMetrics;

        public ExampleBase(string exampleName) : base()
        {
            Window.Title = exampleName;
            ClearColor = Color.Black;
            _performanceMetrics = new PerformanceMetrics();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (G.Input.Keyboard.JustPressed(Keys.Escape))
                Exit();
            if ((G.Input.Keyboard.Held(Keys.LeftAlt) || G.Input.Keyboard.Held(Keys.RightAlt)) && G.Input.Keyboard.JustPressed(Keys.Enter))
                G.GraphicsDeviceManager.ToggleFullScreen();

            _performanceMetrics.InternalUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            _performanceMetrics.LoadContent(Content);
            base.LoadContent();
        }

        protected override void LateInitialize()
        {
            base.LateInitialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            G.SpriteBatch.Begin();
            _performanceMetrics.InternalDraw(gameTime, G.SpriteBatch);
            G.SpriteBatch.End();
        }
    }
}
