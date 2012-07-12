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
using Delta.Structures;
using Delta.Graphics;

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
            G.World.Add(new Image(@"Graphics\Background") { Layer = (int)DrawLayers.BackgroundLow });
            G.World.Add(new Image(@"Graphics\ForeGround") { Layer = (int)DrawLayers.Background });
            G.World.Add(new GravitySink() { Layer = (int)DrawLayers.Ground });
            G.World.Add(new Lucas() { Layer = (int)DrawLayers.Ground });
            G.World.Add(new MovingSpeaker() { Position = new Vector2(200, 0), Layer = (int)DrawLayers.Ground });

            FuelAtom atom = new FuelAtom() { Layer = (int)DrawLayers.Ground };
            G.World.Add(atom);
            Transformer.ThisEntity(atom).TranslateTo(atom.Position + new Vector2(200, 0), 10f, Interpolation.EaseInOutCubic).TranslateTo(atom.Position, 10f, Interpolation.EaseInOutCubic).Repeat(4);
            Transformer.ThisEntity(atom).ScaleTo(new Vector2(2, 2), 10f).ScaleTo(new Vector2(1, 1), 10f).Loop();
            Transformer.ThisEntity(atom).RotateTo(180, 10f).RotateTo(0, 10f);
            Transformer.ThisEntity(atom).FlickerFor(0.5f, 1, 0.2f).Loop();
            //Transformer.ThisEntity(atom).BlinkFor(0.5f, 20);
            //Transformer.ThisEntity(atom).FadeTo(0.0f, 0.2f).FadeTo(1, 0.2f).Repeat(80 * 1000);

            UI.Add(new GameHud());
            UI.Camera.ZoomImmediate(4.0f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            G.Audio.PlaySound("SFX_Ambiance_1");
            Entity lucas = Entity.Get("Lucas") as Entity;
            MovingSpeaker speaker = Entity.Get("Speaker") as MovingSpeaker;
            speaker.Orbit(Vector2.Zero);
            speaker.OrbitLength = 100;

            G.Audio.Listener = lucas;
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);

            PoolManager.DebugDraw();
        }
    }
}
