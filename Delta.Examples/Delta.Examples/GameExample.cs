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
using Delta.Examples.Entities;
using System.Collections.ObjectModel;
using Delta.Audio;
using Delta.Movement;

namespace Delta.Examples
{

    public class GameExample : ExampleBase
    {
        public enum DrawLayers
        {
            BackgroundLow,
            Background,
            Ground,
            Flyers,
            Overlay,
        }

        public GameExample() : base("EntityExample")
        {
            World.Add(new Image(@"Graphics\Background"), (int)DrawLayers.BackgroundLow);
            World.Add(new Image(@"Graphics\ForeGround"), (int)DrawLayers.Background);
            World.Add(new GravitySink(), (int)DrawLayers.Ground);
            World.Add(new Lucas(), (int)DrawLayers.Ground);
            World.Add(new MovingSpeaker() { Position = new Vector2(200, 0) }, (int)DrawLayers.Ground);

            FuelAtom fa = new FuelAtom();
            World.Add(fa, (int)DrawLayers.Ground);
            Transformer.InWorldThisEntity(fa).TranslateTo(fa.Position + new Vector2(200, 0), 10f).TranslateTo(fa.Position, 10f).Repeat(4);

            UI.Add(new GameHud(), 0);
            UI.Camera.ZoomImmediate(4.0f);
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            World.LoadContent(Content);
            UI.LoadContent(Content);
            base.LoadContent();
        }

        /// <summary>
        /// Everything is safe; Audio is up, Input is up, all Entities have been added.
        /// </summary>
        protected override void LateInitialize()
        {
            World.Camera.Offset = new Vector2(1280/2, 720/2); // ScreenCenter isn't calculated until LoadContent.

            G.Audio.PlaySound("SFX_Ambiance_1");
            Entity lucas = Entity.Get("Lucas") as Entity;
            MovingSpeaker speaker = Entity.Get("Speaker") as MovingSpeaker;
            speaker.Orbit(Vector2.Zero);
            speaker.OrbitLength = 100;

            G.Audio.Listener = lucas;
            base.LateInitialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDeviceManager.GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);
        }
    }
}