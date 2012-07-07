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
            G.World.Add(new Image(@"Graphics\Background"), (int)DrawLayers.BackgroundLow);
            G.World.Add(new Image(@"Graphics\ForeGround"), (int)DrawLayers.Background);
            G.World.Add(new GravitySink(), (int)DrawLayers.Ground);
            G.World.Add(new Lucas(), (int)DrawLayers.Ground);
            G.World.Add(new MovingSpeaker() { Position = new Vector2(200, 0) }, (int)DrawLayers.Ground);

            FuelAtom atom = new FuelAtom();
            G.World.Add(atom, (int)DrawLayers.Ground);
            Transformer.ThisEntity(atom).TranslateTo(atom.Position + new Vector2(200, 0), 10f).TranslateTo(atom.Position, 10f).Repeat(4);
            Transformer.ThisEntity(atom).ScaleTo(new Vector2(2, 2), 10f).ScaleTo(new Vector2(1, 1), 10f).Loop();
            Transformer.ThisEntity(atom).RotateTo(180, 10f).RotateTo(0, 10f);
            Transformer.ThisEntity(atom).FadeTo(0.0f, 0.2f).FadeTo(1, 0.2f).Repeat(80 * 1000);


            G.UI.Add(new GameHud(), 0);
            G.UI.Camera.ZoomImmediate(4.0f);
        }

        //protected override void Initialize()
        //{

        //    base.Initialize();
        //}

        //protected override void LoadContent()
        //{
        //    G.World.LoadContent();
        //    G.UI.LoadContent();
        //    base.LoadContent();
        //}

        /// <summary>
        /// Everything is safe; Audio is up, Input is up, all Entities have been added.
        /// </summary>
        protected override void LateInitialize()
        {
            G.World.Camera.Offset = new Vector2(1280/2, 720/2); // ScreenCenter isn't calculated until LoadContent.

            G.Audio.PlaySound("SFX_Ambiance_1");
            TransformableEntity lucas = Entity.Get("Lucas") as TransformableEntity;
            MovingSpeaker speaker = Entity.Get("Speaker") as MovingSpeaker;
            speaker.Orbit(Vector2.Zero);
            speaker.OrbitLength = 100;

            G.Audio.Listener = lucas;
            base.LateInitialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);
        }
    }
}
