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

namespace Delta.Examples
{

    public class UIExample : ExampleBase
    {
        public UIExample() : base("UIExample")
        {

        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);
            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawPixel(G.ScreenCenter, Color.White);
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
