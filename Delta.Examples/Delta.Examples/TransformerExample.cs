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
using Delta.Transformations;
using Delta.Structures;
using Delta.Graphics;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TransformerExample : ExampleBase
    {

        //TextureEntity _flower;
        //TextureEntity _crocus;

        float _trailInterval = 0.1f;
        float _trailTime = 0f;
        float _trailInterval1 = 0.3f;
        float _trailTime1 = 0f;

        public TransformerExample()
            : base("TransformerExample")
        {
            G.World.Camera.ZoomImmediate(4f);
            //_flower = new TextureEntity(@"Graphics\Flower");
            //_crocus = new TextureEntity(@"Graphics\Crocus");
        }

        protected override void Initialize()
        {
            //_crocus.Position = G.ScreenCenter / 4f;
            //_flower.Position = _crocus.Position + new Vector2(20, 0);
            //_flower.Scale = new Vector2(0.0f, 0.0f);

            //G.World.Add(_crocus);
            //G.World.Add(_flower);

            //Transformer.ThisEntity(_flower).FlickerFor(0.8f, 1, 0.1f).Loop();
            //Transformer.ThisEntity(_flower).ScaleTo(new Vector2(0.7f, 0.7f), 0.8f);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            //Vector2 newPosition;
            //newPosition.X = _crocus.Position.X + 40f * (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds);
            //newPosition.Y = _crocus.Position.Y + 35f * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 5;
            //_flower.Position = newPosition;

            //if (G.World.SecondsPast(_trailTime1 + _trailInterval1))
            //{
            //    TextureEntity trail = new TextureEntity(@"Graphics\Flower");
            //    trail.Position = _flower.Position;
            //    trail.Scale = new Vector2(0.7f, 0.7f);
            //    trail.Alpha = _flower.Alpha;
            //    Transformer.ThisEntity(trail).FadeTo(0, 0.5f);
            //    //Transformer.ThisEntity(trail).ScaleTo(new Vector2(0.5f, 0.5f), 0.8f);
            //    G.World.Add(trail);
            //    _trailTime1 = G.World.Time.TotalSeconds;
            //}
            //if (G.World.SecondsPast(_trailTime + _trailInterval) && G.Input.Keyboard.IsDown(Keys.D1))
            //{
            //    TextureEntity trail = new TextureEntity(@"Graphics\Flower");
            //    trail.Position = _flower.Position;
            //    trail.Scale = new Vector2(0.7f, 0.7f);
            //    trail.Alpha = _flower.Alpha;
            //    Transformer.ThisEntity(trail).TranslateTo(trail.Position + new Vector2(0, 80), 0.5f);
            //    Transformer.ThisEntity(trail).FadeTo(0, 0.5f);
            //    Transformer.ThisEntity(trail).ScaleTo(new Vector2(1.5f, 1.5f), 0.5f);
            //    G.World.Add(trail);
            //    _trailTime = G.World.Time.TotalSeconds;
            //}
            //else if (G.World.SecondsPast(_trailTime + _trailInterval) && G.Input.Keyboard.IsDown(Keys.D2))
            //{
            //    TextureEntity trail = new TextureEntity(@"Graphics\Flower");
            //    trail.Position = _flower.Position;
            //    trail.Scale = new Vector2(0.7f, 0.7f);
            //    trail.Alpha = _flower.Alpha;
            //    Transformer.ThisEntity(trail).TranslateTo(trail.Position + new Vector2(0, 80), 0.5f);
            //    Transformer.ThisEntity(trail).FadeTo(0, 0.5f);
            //    Transformer.ThisEntity(trail).ScaleTo(new Vector2(1.5f, 1.5f), 0.5f);
            //    Transformer.ThisEntity(trail).RotateTo(360, 0.5f);
            //    G.World.Add(trail);
            //    _trailTime = G.World.Time.TotalSeconds;
            //}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);
            PoolManager.DebugDraw();
        }
    }

}
