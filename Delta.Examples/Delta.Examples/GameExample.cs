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

        public GameExample() : base("GameExample")
        {
            G.World.Camera.Tint = (new Color(0, 0, 0, 50));
            G.World.Camera.TintEnabled = true;

            G.World.Add(new Image(@"Graphics\Background") { Depth = (int)DrawLayers.BackgroundLow });
            G.World.Add(new Image(@"Graphics\ForeGround") { Depth = (int)DrawLayers.Background });
            G.World.Add(new GravitySink() { Depth = (int)DrawLayers.Ground });
            G.World.Add(new MovingSpeaker() { Position = new Vector2(200, 0), Depth = (int)DrawLayers.Ground });
            G.World.Add(new Lucas() { Depth = (int)DrawLayers.Flyers });

            FuelAtom atom = new FuelAtom() { Depth = (int)DrawLayers.Ground };
            G.World.Add(atom);
            Transformer.ThisEntity(atom).TranslateTo(atom.Position + new Vector2(200, 0), 10f, Interpolation.EaseInOutCubic).TranslateTo(atom.Position, 10f, Interpolation.EaseInOutCubic).Repeat(4);
            Transformer.ThisEntity(atom).ScaleTo(new Vector2(2, 2), 10f).ScaleTo(new Vector2(1, 1), 10f).Loop();
            Transformer.ThisEntity(atom).RotateTo(180, 10f).RotateTo(0, 10f);
            Transformer.ThisEntity(atom).FlickerFor(0.5f, 1, 0.2f).Loop();

            //UI.Add(new GameHud());
            //UI.Camera.ZoomImmediate(4.0f);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void LateInitialize()
        {
            G.Audio.PlaySound("SFX_Ambiance_1");
            TransformableEntity lucas = TransformableEntity.Get("Lucas") as TransformableEntity;
            MovingSpeaker speaker = TransformableEntity.Get("Speaker") as MovingSpeaker;
            speaker.Orbit(Vector2.Zero);
            speaker.OrbitLength = 100;

            G.Audio.Listener = lucas;
            base.LateInitialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            G.GraphicsDevice.Clear(ClearColor);
            base.Draw(gameTime);

            PoolManager.DebugDraw();
        }
    }
}
