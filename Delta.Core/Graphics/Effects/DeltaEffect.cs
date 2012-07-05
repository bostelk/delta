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

        public enum Technique
        {
            Tint,
            FillColor,
            Grayscale,
            Negative,
            Overlay
        }

        public DeltaEffect(Effect e) 
            : base(e)
        {
            SetTechnique(Technique.Tint);
        }

        public void SetTechnique(Technique technique)
        {
            CurrentTechnique = Techniques[technique.ToString()];
        }
    }
}
