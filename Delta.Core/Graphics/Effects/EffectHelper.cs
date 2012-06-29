using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Graphics.Effects
{
    public static class EffectHelper
    {

        public static Entity SpriteEffect(string sheet, string animation, float duration)
        {
            SpriteEntity s = new SpriteEntity();
            //s.SpriteSheet = G.Content.Load<SpriteSheet>("mainSpriteSheet");
            s.AnimationName = animation;
            return s;
        }
    }
}
