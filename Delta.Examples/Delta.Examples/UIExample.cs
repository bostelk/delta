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
            Panel pnl = new Panel();
            pnl.BackColor = Color.White;
            pnl.Alpha = 1;
            pnl.Position = new Vector2(50, 50);
            pnl.Size = new Vector2(50, 50);
            Panel pnl2 = new Panel();
            pnl2.BackColor = Color.Yellow;
            pnl2.Alpha = 1;
            pnl2.Position = new Vector2(75, 80);
            pnl2.Size = new Vector2(50, 50);
            G.UI.HUD.Add(pnl);
            G.UI.HUD.Add(pnl2);
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
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
