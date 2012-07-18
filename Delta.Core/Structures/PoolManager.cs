using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Delta.Movement;
using Microsoft.Xna.Framework;
using Delta.Audio;
using Delta.Entities;

namespace Delta.Structures
{
    public static class PoolManager
    {
        public static void DebugDraw()
        {
            string info = String.Format("SpritePool: {0}\n", Pool<SpriteEntity>.PerformanceInfo)
                + String.Format("TransformerPool: {0}\n", Pool<Transformer>.PerformanceInfo)
                + String.Format("TranslatePool: {0}\n", Pool<TranslateTransform>.PerformanceInfo)
                + String.Format("RotatePool: {0}\n", Pool<RotateTransform>.PerformanceInfo)
                + String.Format("ScalePool: {0}\n", Pool<ScaleTransform>.PerformanceInfo)
                + String.Format("FadePool: {0}\n", Pool<FadeTransform>.PerformanceInfo)
                + String.Format("FlickerPool: {0}\n", Pool<FlickerTransform>.PerformanceInfo)
                + String.Format("BlinkPool: {0}\n", Pool<BlinkTransform>.PerformanceInfo)
                + String.Format("Sound1DPool: {0}\n", Pool<Sound1D>.PerformanceInfo)
                + String.Format("Sound3DPool: {0}\n", Pool<Sound3D>.PerformanceInfo)
                + String.Format("TexturePool: {0}\n", Pool<TextureEntity>.PerformanceInfo)
                + String.Format("SpriteEmitterPool: {0}\n", Pool<SpriteEmitter>.PerformanceInfo)
                + String.Format("SpriteParticlePool: {0}\n", Pool<SpriteEmitter.SpriteParticle>.PerformanceInfo)
                + String.Format("PixelEmitterPool: {0}\n", Pool<PixelEmitter>.PerformanceInfo)
                + String.Format("PixelParticlePool: {0}\n", Pool<PixelEmitter.PixelParticle>.PerformanceInfo);
            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, info, new Vector2(0, 100), Color.White);
            G.SpriteBatch.End();
        }
    }
}
