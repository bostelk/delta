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

using Delta.Graphics;
using Delta.Tiled;
using Delta.Input;
using System.Text;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TiledExample : ExampleBase
    {
        const string CONTROLS = "[up] up layer.[down] down layer.[left] invisible.[right] visible.[lctrl] all visible.[rctrl] all invisible.";

        Map _map;
        int _selectedLayer = 0;

        public void RunScript()
        {
            this.Run();
        }

        public TiledExample() : base("TiledExample")
        {
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Content.Load<SpriteSheet>(@"Graphics\SpriteSheets\Tilesets");
            _map = Content.Load<Map>(@"Maps\Plains\1");
            G.World.Add(_map);
            G.World.Camera.Offset = G.ScreenCenter;
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            G.World.Camera.MoveToImmediate(G.World.Camera.Position + G.Input.WadsDirection * (G.Input.Keyboard.Held(Keys.RightShift) ? 8: 3));
            if (G.Input.Keyboard.Held(Keys.OemPlus))
                G.World.Camera.ZoomByAmount(0.2f);
            if (G.Input.Keyboard.Held(Keys.OemMinus))
                G.World.Camera.ZoomByAmount(-0.2f);

            if (G.Input.Keyboard.JustPressed(Keys.Up))
                _selectedLayer = DeltaMath.Wrap(_selectedLayer - 1, 0, _map.Children.Count - 1);
            else if (G.Input.Keyboard.JustPressed(Keys.Down))
                _selectedLayer = DeltaMath.Wrap(_selectedLayer + 1, 0, _map.Children.Count - 1);
            if (G.Input.Keyboard.JustPressed(Keys.Left))
                _map.Children[_selectedLayer].IsVisible = false;
            if (G.Input.Keyboard.JustPressed(Keys.Right))
                _map.Children[_selectedLayer].IsVisible = true;
            if (G.Input.Keyboard.JustPressed(Keys.LeftControl))
            {
                foreach (TileLayer layer in _map.Children)
                    layer.IsVisible = true;
            }
            if (G.Input.Keyboard.JustPressed(Keys.RightControl))
            {
                foreach (TileLayer layer in _map.Children)
                    layer.IsVisible = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);
            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, InfoText, new Vector2(0, 40), Color.White);
            G.SpriteBatch.DrawString(G.Font, CONTROLS, new Vector2(G.ScreenCenter.X, 0), Color.Orange, TextAlignment.Center);
            G.SpriteBatch.End();
        }

        public string InfoText
        {
            get
            {
#if DEBUG
                int totalTiles = 0; int totalDrawn = 0;
                StringBuilder text = new StringBuilder();
                text.Append(String.Format("Map: {0}x{1} tiles.\nTiles: {2}x{3} pixels.\n\n", _map.Width, _map.Height, _map.TileWidth, _map.TileHeight));
                text.Append(String.Format("Selected Layer: {0}\n", _selectedLayer));
                text.Append(String.Format("{0,-8}{1,-50}{2,20}{3,20}{4,20}\n", "Index", "Name", "Visible", "Tiles", "Drawn"));
                for (int i = 0; i < _map.Children.Count; i++)
                {
                    TileLayer layer = _map.Children[i]  as TileLayer;
                    if (i == _selectedLayer)
                        text.Append("   ");
                    text.Append(String.Format("{0,-8}{1,-50}{2,20}{3,20}{4,20}\n", i + ".", layer.ID, layer.IsVisible, layer.TileCount, layer.TilesDrawn));
                    totalTiles += layer.TileCount;
                    totalDrawn += layer.TilesDrawn;
                }
                text.Append(String.Format("Total:{0,-8}{1,-50}{2,20}{3,20}{4,20}\n", "", "", "", totalTiles, totalDrawn));
                return text.ToString();
#else
                return String.Empty;
#endif
            }
        }
    }
}
