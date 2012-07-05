﻿using System;
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
        const string CONTROLS = "";
        TransformableEntity player1;

        public PhysicsExample() : base("PhysicsExample")
        {
            ClearColor = Color.Black;
            G.Physics.DefineWorld(1024, 1024, 32);
            G.World.Camera.Offset = G.ScreenCenter;

            G.World.Add(player1 = new BoxLink());
            G.World.Add(new WorldBounds());
            List<Obstacle> obstacles = EntitySpawner.OnAGrid<Obstacle>(Vector2.Zero, 10, 20, 32, 32, () => { return new Obstacle() { MoveAndRotate = true }; });
            G.World.AddRange(obstacles.ToList<IEntity>(), 0);
        }

        protected override void Update(GameTime gameTime)
        {
            if (G.Input.Keyboard.JustPressed(Keys.Tab))
                (player1 as BoxLink).SwitchBody();
            if (IsMouseVisible && G.Input.Keyboard.Held(Keys.Space))
                player1.Position = G.World.Camera.ToWorldPosition(G.Input.Mouse.Position);
            if (G.Input.Keyboard.JustPressed(Keys.F1))
                G.World.Camera.Follow(player1);
                
            (player1 as BoxLink).Input = G.Input.WadsDirection;
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
