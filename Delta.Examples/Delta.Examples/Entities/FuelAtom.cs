using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta.Audio;

namespace Delta.Examples.Entities
{
    public class FuelAtom : Entity
    {

        Texture2D _texture;

        public FuelAtom()
        {
            ID = "FuelAtom";
        }

        public override void LoadContent(ContentManager content)
        {
            // note: pixeltexture is null here, wtf
            _texture = content.Load<Texture2D>(@"Graphics\Atom");
            base.LoadContent(content);
        }

        protected override void LateInitialize()
        {
            base.LateInitialize();
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            base.LightUpdate(gameTime);
        }

        protected override void InternalDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            G.SimpleEffect.SetTechnique(Graphics.Effects.SimpleEffect.Technique.Flicker);
            G.SimpleEffect.FlickerRate = 50;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, G.SimpleEffect, World.Instance.Camera.View);
            spriteBatch.Draw(_texture, Position, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, World.Instance.Camera.View);
            base.InternalDraw(gameTime, spriteBatch);
        }

    }
}
