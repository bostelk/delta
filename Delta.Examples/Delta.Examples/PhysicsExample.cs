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
//using Delta.Scripting;
using Delta.Examples.Entities;
using Delta.Physics;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PhysicsExample : ExampleBase
    {
        BoxCar player1, player2;

        public PhysicsExample() : base("PhysicsExample")
        {
            ClearColor = Color.Black;

            G.World.Add(player1 = new BoxCar());
            G.World.Add(player2 = new BoxCar());
            player2.Position = player1.Position + new Vector2(100, 0);
            G.World.Camera.Offset = G.ScreenCenter;
        }

        protected override void LoadContent()
        {
            G.World.LoadContent(Content);
            G.UI.LoadContent(Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            player1.Input = G.Input.WadsDirection;
            player2.Input = G.Input.ArrowDirection;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);
            Matrix view = G.World.Camera.View;
            Matrix projection = G.World.Camera.Projection;
            G.Physics.DrawDebug(ref view, ref projection);
            base.Draw(gameTime);
        }
    }
}
