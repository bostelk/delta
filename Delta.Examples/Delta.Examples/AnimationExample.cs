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

using Delta;
using Delta.Graphics;
using Delta.Examples.Entities;
using Delta.Input;
using System.Collections.ObjectModel;

namespace Delta.Examples
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AnimationExample : ExampleBase
    {
        const string CONTROLS = "[f1] export sheet.[up] next sprite.[down] previous sprite.[left] previous animation.[right] next animation.\n[space] play/pause.";

        SpriteSheet mainSheet;
        AnimatedSpriteEntity sprite = new AnimatedSpriteEntity();

        ReadOnlyCollection<Animation> _supportedAnimations;
        int _animationIndex = 0;

        public AnimationExample() : base("AnimationExample")
        {
            ClearColor = Color.Gray;
        }

        protected override void Initialize()
        {
            base.Initialize();
            sprite.Scale = new Vector2(4);
        }

        protected override void LoadContent()
        {
            mainSheet = Content.Load<SpriteSheet>(@"Graphics\SpriteSheets\Tilesets");
            _supportedAnimations = mainSheet.Animations;
            sprite.SpriteSheet = mainSheet;
            sprite.AnimationName = _supportedAnimations[_animationIndex].Name;
            sprite.Position = G.ScreenCenter;
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (G.Input.Keyboard.JustPressed(Keys.Up))
            {
            }
            if (G.Input.Keyboard.JustPressed(Keys.Down))
            {
            }
            if (G.Input.Keyboard.JustPressed(Keys.Left))
            {
                _animationIndex = DeltaMath.Wrap(_animationIndex - 1, 0, _supportedAnimations.Count - 1);
                sprite.AnimationName = _supportedAnimations[_animationIndex].Name;
            }
            if (G.Input.Keyboard.JustPressed(Keys.Right))
            {
                _animationIndex = DeltaMath.Wrap(_animationIndex + 1, 0, _supportedAnimations.Count - 1);
                sprite.AnimationName = _supportedAnimations[_animationIndex].Name;
            }
            if (G.Input.Keyboard.JustPressed(Keys.Space))
            {
                if (!sprite.IsPaused)
                    sprite.Pause();
                else
                    sprite.Resume();
            }
            if (G.Input.Keyboard.JustPressed(Keys.F1))
            {
                mainSheet.Texture.SaveAsPng(new System.IO.FileStream("sheet.png", System.IO.FileMode.OpenOrCreate), mainSheet.Texture.Width, mainSheet.Texture.Height);
            }
            sprite.InternalUpdate(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            G.SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
            sprite.InternalDraw(gameTime, G.SpriteBatch);
            G.SpriteBatch.DrawString(G.Font, CONTROLS, new Vector2(G.ScreenCenter.X, 0), Color.Orange, TextAlignment.Center);
            //SpriteBatch.DrawString(G.Font, InfoText, new Vector2(0, 50), Color.White);
            G.SpriteBatch.DrawPixel(G.ScreenCenter, Color.White);
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }

        //public string InfoText
        //{
        //    get
        //    {
        //        return string.Format("Animation: {0}\nFrame: {1} of {2}\nFrame Duration: {3}", sprite.AnimationName, sprite.AnimationFrame, sprite.TotalFrames, sprite.SecondsLeft);
        //    }
        //}
    }
}
