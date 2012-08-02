using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta;
using Delta.Audio;

namespace Delta.Examples.Entities
{
    public class FuelAtom : TransformableEntity
    {

        Texture2D _texture;

        public FuelAtom() : base("FuelAtmo")
        {
        }

        protected override void LoadContent()
        {
            _texture = G.Content.Load<Texture2D>(@"Graphics\Atom");
            base.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            //spriteBatch.End();

            //G.SimpleEffect.SetTechnique(Graphics.Effects.SimpleEffect.Technique.Flicker);
            //G.SimpleEffect.FlickerRate = 50;
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, G.SimpleEffect, World.Instance.Camera.View);
            spriteBatch.Draw(_texture, Position, new Rectangle(0, 0, _texture.Width, _texture.Height), Color.White.SetAlpha(Alpha), Rotation, new Vector2(_texture.Width / 2, _texture.Height / 2), Scale, SpriteEffects.None, 0f);
            //spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, World.Instance.Camera.View);
            base.Draw(time, spriteBatch);
        }

    }
}
