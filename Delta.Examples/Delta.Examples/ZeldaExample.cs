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
    public class ZeldaExample : ExampleBase
    {
        const string CONTROLS = "[wads] movement.[tab] switch geometry.[f1] obstacle rotate on/off.[f2] switch obstacle geometry.";
        TransformableEntity _player;
        List<Obstacle> _obstacles;

        public ZeldaExample() : base("ZeldaExample")
        {
            ClearColor = Color.Black;
            G.Physics.DefineWorld(1024, 1024, 32);
            G.World.Camera.Offset = G.ScreenCenter;
            G.World.Camera.ZoomImmediate(4);

            G.World.Add(_player = new BoxLink());
            //G.World.Add(new WorldBounds());
            _obstacles = EntitySpawner.OnAGrid<Obstacle>(Vector2.Zero, 10, 20, 32, 32, () => { return new Obstacle(); });
            G.World.AddRange(_obstacles.ToList<IEntity>(), 0);

            G.World.Camera.Follow(_player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (G.Input.Keyboard.JustPressed(Keys.Tab))
                (_player as BoxLink).SwitchBody();
            (_player as BoxLink).Input = G.Input.WadsDirection * ((G.Input.Keyboard.Held(Keys.RightShift)) ? 2.5f : 1);
            if (G.Input.Keyboard.JustPressed(Keys.F1))
            {
                foreach (Obstacle obstacle in _obstacles)
                    obstacle.AutoRotate = !obstacle.AutoRotate;
            }
            if (G.Input.Keyboard.JustPressed(Keys.F2))
            {
                foreach (Obstacle obstacle in _obstacles)
                    obstacle.SwitchBody();
            }
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
