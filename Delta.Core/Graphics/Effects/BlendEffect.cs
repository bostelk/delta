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
    public class BlendEffect : Effect
    {

        public Texture2D Src
        {
            set
            {
                GraphicsDevice.Textures[1] = value;
            }
        }

        public Texture2D Dst
        {
            set
            {
                GraphicsDevice.Textures[2] = value;
            }
        }

        public enum Technique
        {
            Overlay,
        }

       public BlendEffect(Effect e) : base(e)
        {
        }

        public void SetTechnique(Technique technique)
        {
            CurrentTechnique = Techniques[(int)technique];
        }
    }
}
