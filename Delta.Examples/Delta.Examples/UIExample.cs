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
using Delta.UI;
using Delta.UI.Controls;

namespace Delta.Examples
{

    public class UIExample : ExampleBase
    {
        public UIExample() : base("UIExample")
        {
            G.UI.HUD.Add(new Textbox()
            {
                Position = new Vector2(50, 50),
                Size = new Vector2(40, 40),
            });
            G.UI.HUD.Add(new Button()
            {
                AutoSize = false,
                Position = new Vector2(100, 100),
                Size = new Vector2(40, 40),
            });
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
            GraphicsDevice.Clear(Color.Gray);
            base.Draw(gameTime);
        }
    }
}
