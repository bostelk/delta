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
        TransformableEntity _player;

        public ZeldaExample() : base("ZeldaExample")
        {
            ClearColor = Color.Black;
            G.Collision.DefineWorld(1024, 1024, 32);
            G.World.Camera.Offset = G.ScreenCenter;
            G.World.Camera.ZoomImmediate(4);

            G.World.Add(_player = new BoxLink());
            //_player.Position = new Vector2(-1000, -800);
            //G.World.Add(new WorldBounds());
            G.World.Camera.Follow(_player);
        }

        protected override void LoadContent()
        {
            Content.Load<SpriteSheet>(@"Graphics\SpriteSheets\16x16");
            _map = Content.Load<Map>(@"Maps\Plains\2");
            G.World.Add(_map);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (G.Input.Keyboard.JustPressed(Keys.Tab))
                (_player as BoxLink).SwitchBody();
            (_player as BoxLink).Input = G.Input.WadsDirection * ((G.Input.Keyboard.Held(Keys.RightShift)) ? 2.5f : 1);
            if (G.Input.Keyboard.JustPressed(Keys.OemTilde))
            {
                (_player as BoxLink).ColliderEnabled = !(_player as BoxLink).ColliderEnabled;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);
            Matrix view = G.World.Camera.View;
            Matrix projection = G.World.Camera.Projection;
            G.Collision.DrawDebug(ref view, ref projection);
            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, CONTROLS, new Vector2(G.ScreenCenter.X, 0), Color.Orange, TextAlignment.Center);
            G.SpriteBatch.End();
        }
    }
}
