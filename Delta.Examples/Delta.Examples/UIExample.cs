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
            G.UI.HUD.Add(new Delta.UI.Controls.Textbox()
            {
                AutoSize = false,
                Position = new Vector2(50, 50),
                Size = new Vector2(40, 40),
                BackColor = Color.White,
                HighlightedColor = Color.DarkRed,
                FocusedColor = Color.Green,
                ClickedColor = Color.Orange,
            });
            G.UI.HUD.Add(new Delta.UI.Controls.Label()
            {
                AutoSize = false,
                Position = new Vector2(100, 100),
                Size = new Vector2(40, 40),
                BackColor = Color.White,
                HighlightedColor = Color.Yellow,
                FocusedColor = Color.Green,
                ClickedColor = Color.Orange,
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
