using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Delta.Transformations;
using Delta.Graphics;

namespace Delta.Graphics
{
    public static class Visuals
    {

        /// <summary>
        /// Disposable effects like explosions.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animation"></param>
        /// <param name="position"></param>
        public static void Create(string spriteSheet, string animation, Vector2 position)
        {
            AnimatedSpriteEntity se = AnimatedSpriteEntity.Create(spriteSheet);
            se.InternalLoadContent();
            se.Position = position;
            se.Origin = new Vector2(0.5f, 0.5f);
            se.Play(animation, AnimationOptions.Recycle);
            se.Depth = position.Y;
            G.World.Ground.UnsafeAdd(se);
        }
 
        /// <summary>
        /// For creating motion trails.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animation"></param>
        /// <param name="position"></param>
        public static void CreateTrail(string spriteSheet, string animation, Vector2 position) 
        {
            AnimatedSpriteEntity se = AnimatedSpriteEntity.Create(spriteSheet);
            se.InternalLoadContent();
            se.Position = position;
            se.Origin = new Vector2(0.5f, 0.5f);
            se.Alpha = 0.5f;
            se.Play(animation);
            se.Depth = position.Y;
            G.World.BelowGround.UnsafeAdd(se);

            Transformer.ThisEntity(se).FadeTo(0, 0.5f).OnSequenceFinished(() => { se.Recycle(); });
        }

        /// <summary>
        /// For creating motion trails.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animation"></param>
        /// <param name="position"></param>
        public static void CreateTrail(AnimatedSpriteEntity sprite, Vector2 position)
        {
            SpriteEntity sfe = SpriteEntity.Create(sprite);
            sfe.InternalLoadContent();
            sfe.Position = position;
            sfe.Origin = new Vector2(0.5f, 0.5f);
            sfe.Alpha = 0.5f;
            sfe.Depth = position.Y;
            G.World.BelowGround.UnsafeAdd(sfe);

            Transformer.ThisEntity(sfe).FadeTo(0, 0.3f).OnSequenceFinished(() => { sfe.Recycle(); });
        }


        /// <summary>
        /// Visually looks like a randomly rotated X pattern.
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animation"></param>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public static void CreateShatter(string spriteSheet, string animation, Vector2 position, float radius) 
        {
            // pick a random frame from teh animation.
            AnimatedSpriteEntity[] se = new AnimatedSpriteEntity[4];

            for(int i = 0; i < 4; i++)
            {
                AnimatedSpriteEntity see = se[i] = AnimatedSpriteEntity.Create(spriteSheet);
                see.InternalLoadContent();
                see.Position = position;
                see.Origin = new Vector2(0.5f, 0.5f);
                see.Play(animation, AnimationOptions.StartOnRandomFrame);
                see.Pause();
                see.Depth = position.Y;
                G.World.AboveGround.UnsafeAdd(see);
            }

            // rotate the cross randomly
            double angleOffset = G.Random.NextDouble() * MathExtensions.RADIANS_45;

            Vector2 x1 = new Vector2((float)Math.Cos(MathExtensions.RADIANS_45 + angleOffset), (float)Math.Sin(MathExtensions.RADIANS_45 + angleOffset)) * radius;
            Vector2 x2 = new Vector2((float)Math.Cos(MathExtensions.RADIANS_135 + angleOffset), (float)Math.Sin(MathExtensions.RADIANS_135 + angleOffset)) * radius;
            Vector2 x3 = new Vector2((float)Math.Cos(MathExtensions.RADIANS_225 + angleOffset), (float)Math.Sin(MathExtensions.RADIANS_225 + angleOffset)) * radius;
            Vector2 x4 = new Vector2((float)Math.Cos(MathExtensions.RADIANS_315 + angleOffset), (float)Math.Sin(MathExtensions.RADIANS_315 + angleOffset)) * radius;

            // explode 4 pieces off in directions 45 degrees seperated from each other. then, flash the pieces briefly.
            Transformer.ThisEntity(se[0]).TranslateTo(position + x1, 0.5f).BlinkFor(0.08f, 0.5f).OnSequenceFinished(() => { se[0].Recycle(); });
            Transformer.ThisEntity(se[1]).TranslateTo(position + x2, 0.5f).BlinkFor(0.08f, 0.5f).OnSequenceFinished(() => { se[1].Recycle(); });
            Transformer.ThisEntity(se[2]).TranslateTo(position + x3, 0.5f).BlinkFor(0.08f, 0.5f).OnSequenceFinished(() => { se[2].Recycle(); });
            Transformer.ThisEntity(se[3]).TranslateTo(position + x4, 0.5f).BlinkFor(0.08f, 0.5f).OnSequenceFinished(() => { se[3].Recycle(); });
        }

    }
}
