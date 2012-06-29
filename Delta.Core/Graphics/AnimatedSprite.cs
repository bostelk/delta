using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Graphics
{
    public class AnimatedSprite : SpriteEntity
    {
        int _frameIndex;
        float _frameDuration = 0f;

        [ContentSerializerIgnore]
        public bool IsPlaying { get; private set; }
        public bool IsPaused { get; private set; }

        public float SecondsLeft
        {
            get
            {
                return TotalFrames * Animation.FrameSeconds - Frame * Animation.FrameSeconds;
            }
        }

        public AnimatedSprite()
            : base()
        {
        }

        public void Pause()
        {
            IsPaused = true;
            IsPlaying = false;
        }

        public void Resume()
        {
            IsPaused = false;
            IsPlaying = false;
        }

        public void Stop()
        {
        }

        protected internal override void UpdateAnimation(GameTime gameTime)
        {
            if (IsPaused)
                return;
            base.UpdateAnimation(gameTime);
            if (Animation != null)
            {
                // if there are frames to play, then autostart playing them
                if (Animation.Frames.Count > 1)
                    IsPlaying = true;
                else
                    IsPlaying = false;
            }
            else
            {
                _frameIndex = -1;
                _frameDuration = 0f;
                IsPlaying = false;
            }
        }

        protected internal override void UpdateFrame(GameTime gameTime)
        {
            base.UpdateFrame(gameTime);
            if (IsPaused)
                return;
            UpdateAnimationFrame(gameTime);
        }

        protected internal virtual void UpdateAnimationFrame(GameTime gameTime)
        {
            if (IsPlaying && !IsPaused)
            {
                if (_frameIndex < 0)
                {
                    _frameIndex = 0;
                    _frameDuration = Animation.FrameSeconds;
                }
                _frameDuration -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (_frameDuration <= 0f)
                {
                    _frameIndex = DeltaMath.Wrap(_frameIndex + 1, 0, TotalFrames - 1);
                    _frameDuration = Animation.FrameSeconds;
                }
                _frameIndex = DeltaMath.Wrap(_frameIndex, 0, TotalFrames - 1); // HACK: _frameIndex, out of bounds otherwise
                Frame = Animation.Frames[_frameIndex];
            }
        }
    }
}
