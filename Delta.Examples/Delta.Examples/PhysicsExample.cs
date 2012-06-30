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
using Delta.Examples.Entities;
using Delta.Physics;
using Delta.Physics.Geometry;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PhysicsExample : ExampleBase
    {
        const string CONTROLS = "[wasd] movement 1. [tab] change body 1. [arrows] movement 2.[right ctrl] change body 2.";
        Entity player1, player2;

        public PhysicsExample() : base("PhysicsExample")
        {
            ClearColor = Color.Black;
            G.World.Add(player1 = new PhysicsCar());
            G.World.Add(player2 = new PhysicsCar());
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
            if (G.Input.Keyboard.JustPressed(Keys.Tab))
                (player1 as PhysicsCar).SwitchBody();
            if (G.Input.Keyboard.JustPressed(Keys.RightControl))
                (player2 as PhysicsCar).SwitchBody();
                
            (player1 as PhysicsCar).Input = G.Input.WadsDirection;
            (player2 as PhysicsCar).Input = G.Input.ArrowDirection;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);
            Matrix view = G.World.Camera.View;
            Matrix projection = G.World.Camera.Projection;
            G.Physics.DrawDebug(ref view, ref projection);
            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, CONTROLS, new Vector2(G.ScreenCenter.X, 0), Color.Orange, TextAlignment.Center);
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
