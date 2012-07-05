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

namespace Delta.Graphics.Effects
{
    public class DeltaEffect : Effect
    {

        public EffectTechnique None { get; private set; }
        public EffectTechnique Grayscale { get; private set; }
        public EffectTechnique Negative { get; private set; }
        public EffectTechnique Overlay { get; private set; }

        internal DeltaEffect(Effect e) 
            : base(e)
        {
            None = Techniques["Tint"];
            Grayscale = Techniques["Grayscale"];
            Negative = Techniques["Negative"];
            Overlay = Techniques["Overlay"];
            CurrentTechnique = None;
        }

    }
}
