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
using Delta.Collision;
using Delta.Collision.Geometry;
using Delta.Graphics;
using Delta.Tiled;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZeldaExample : ExampleBase
    {
        const string CONTROLS = "[wads] movement.[rshift] boost.[tab] switch geometry.[~] enable/disable collider.";
        Map _map;
        Entity _player;

        public ZeldaExample() : base("ZeldaExample")
        {
            ClearColor = Color.Black;
            G.Collision.DefineWorld(640, 640, 32);
            G.World.Camera.Offset = G.ScreenCenter;
            G.World.Camera.ZoomImmediate(4);
            //G.World.Camera.BoundsEnabled = true;
            //G.World.Camera.BoundedArea = new Rectangle(0, 0, 640, 640);
        }

        protected override void LoadContent()
        {
            _map = Content.Load<Map>(@"Maps\Plains\3");
            _map.LoadContent();
            _map.AddToWorld(G.World);
            G.World.Ground.Add(_player = new BoxLink());
            G.World.Camera.Follow(_player);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            G.World.Camera.Position += G.Input.WadsDirection;

            //if (G.Input.Keyboard.JustPressed(Keys.Tab))
            //    (_player as BoxLink).SwitchBody();
            //(_player as BoxLink).Input = G.Input.ArrowDirection * ((G.Input.Keyboard.Held(Keys.LeftShift)) ? 2.5f : 1);
            //if (G.Input.Keyboard.JustPressed(Keys.OemTilde))
            //{
            //    (_player as BoxLink).ColliderEnabled = !(_player as BoxLink).ColliderEnabled;
            //}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);

            if (G.Input.Keyboard.Held(Keys.F1))
            {
                Matrix view = G.World.Camera.View;
                Matrix projection = G.World.Camera.Projection;
                G.Collision.DrawDebug(ref view, ref projection);
            }

            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, CONTROLS, new Vector2(G.ScreenCenter.X, 0), Color.Orange, TextAlignment.Center);
            G.SpriteBatch.End();

            G.World.DebugDraw();
        }
    }
}
