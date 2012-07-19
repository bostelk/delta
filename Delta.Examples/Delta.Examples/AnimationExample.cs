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
        SpriteEntity sprite = new SpriteEntity();

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
            mainSheet = Content.Load<SpriteSheet>(@"Graphics\SpriteSheets\16x16");
            _supportedAnimations = mainSheet.Animations;
            sprite = new SpriteEntity("testSprite", @"Graphics\SpriteSheets\16x16");
            sprite.Play(_supportedAnimations[_animationIndex].Name);
            sprite.Position = G.ScreenCenter;
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (G.Input.Keyboard.IsPressed(Keys.Left))
            {
                _animationIndex = MathExtensions.Wrap(_animationIndex - 1, 0, _supportedAnimations.Count - 1);
                sprite.Play(_supportedAnimations[_animationIndex].Name);
                sprite.Position = G.ScreenCenter + sprite.Size * 2;
            }
            if (G.Input.Keyboard.IsPressed(Keys.Right))
            {
                _animationIndex = MathExtensions.Wrap(_animationIndex + 1, 0, _supportedAnimations.Count - 1);
                sprite.Play(_supportedAnimations[_animationIndex].Name);
                sprite.Position = G.ScreenCenter + sprite.Size * 2;
            }
            if (G.Input.Keyboard.IsPressed(Keys.Space))
            {
                if (!sprite.IsAnimationPaused)
                    sprite.IsAnimationPaused = true;
                else
                    sprite.IsAnimationPaused = false;
            }
            if (G.Input.Keyboard.IsPressed(Keys.F1))
            {
                mainSheet.Texture.SaveAsPng(new System.IO.FileStream("sheet.png", System.IO.FileMode.OpenOrCreate), mainSheet.Texture.Width, mainSheet.Texture.Height);
            }
            sprite.InternalUpdate(G.World.Time);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            G.SpriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
            sprite.InternalDraw(G.World.Time, G.SpriteBatch);
            G.SpriteBatch.DrawString(G.Font, CONTROLS, new Vector2(G.ScreenCenter.X, 0), Color.Orange, TextAlignment.Center);
            SpriteBatch.DrawString(G.Font, InfoText, new Vector2(0, 50), Color.White);
            G.SpriteBatch.DrawPixel(G.ScreenCenter, Color.White);
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public string InfoText { get { return string.Empty; } }
    }
}
