﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Delta.Transformations;
using Microsoft.Xna.Framework;
using Delta.Audio;
using Delta.Entities;
using Delta.UI.Controls;
using Delta.Collision;

namespace Delta.Structures
{
    public static class PoolManager
    {
        static Label _infoLbl = new Label();

        static PoolManager()
        {
            _infoLbl.Position = new Vector2(0, 50);
            _infoLbl.InternalLoadContent();
        }

        public static void DebugDraw()
        {
            _infoLbl.Text.Clear();
            _infoLbl.Text.Append(String.Format("SpritePool: {0}\n", Pool<SpriteEntity>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("TransformerPool: {0}\n", Pool<Transformer>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("TranslatePool: {0}\n", Pool<TranslateTransform>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("RotatePool: {0}\n", Pool<RotateTransform>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("ScalePool: {0}\n", Pool<ScaleTransform>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("FadePool: {0}\n", Pool<FadeTransform>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("FlickerPool: {0}\n", Pool<FlickerTransform>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("BlinkPool: {0}\n", Pool<BlinkTransform>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("Sound1DPool: {0}\n", Pool<Sound1D>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("Sound3DPool: {0}\n", Pool<Sound3D>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("TexturePool: {0}\n", Pool<TextureEntity>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("SpriteEmitterPool: {0}\n", Pool<SpriteEmitter>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("SpriteParticlePool: {0}\n", Pool<SpriteEmitter.SpriteParticle>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("PixelEmitterPool: {0}\n", Pool<PixelEmitter>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("PixelParticlePool: {0}\n", Pool<PixelEmitter.PixelParticle>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("ColliderPool: {0}\n", Pool<CollisionBody>.PerformanceInfo));
            _infoLbl.Text.Append(String.Format("BroadphaseProxyPool: {0}\n", Pool<BroadphaseProxy>.PerformanceInfo));
            _infoLbl.Invalidate();
            _infoLbl.InternalUpdate(G._time);
            G.SpriteBatch.Begin();
            _infoLbl.InternalDraw(G._time, G.SpriteBatch);
            G.SpriteBatch.End();
        }
    }
}
