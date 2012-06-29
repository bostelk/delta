using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Graphics
{

    public class AnimatedSpriteEntity : SpriteEntity
    {
        float _frameDurationTimer = 0f;

        [ContentSerializerIgnore]
        public int AnimationFrame { get; private set; }
        [ContentSerializerIgnore]
        public bool IsPaused { get; private set; }
        [ContentSerializerIgnore]
        public bool IsFinished { get; private set; }
        [ContentSerializerIgnore]
        public bool IsLooped { get; private set; }

        [ContentSerializerIgnore]
        public float SecondsLeft
        {
            get
            {
                if (Animation == null)
                    return 0;
                return TotalFrames * Animation.FrameDuration - ImageFrame * Animation.FrameDuration;
            }
        }

        public AnimatedSpriteEntity()
            : base()
        {
        }

        public void PlayAnimation(string animationName, bool isLooped, int startingFrame)
        {
            AnimationName = animationName;
            AnimationFrame = DeltaMath.Clamp(startingFrame, 0, TotalFrames);
            _frameDurationTimer = Animation.FrameDuration;
        }

        public void PlayAnimation(string animation, bool isLooped)
        {
            PlayAnimation(animation, isLooped);
        }

        public void PlayAnimation(string animation)
        {
            PlayAnimation(animation, true);
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        protected internal override void UpdateImageFrame(GameTime gameTime)
        {
            if (!IsPaused || !IsFinished)
                UpdateAnimationFrame(gameTime);
            base.UpdateImageFrame(gameTime);
        }

        protected internal virtual void UpdateAnimationFrame(GameTime gameTime)
        {
            _frameDurationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameDurationTimer <= 0f)
            {
                _frameDurationTimer = Animation.FrameDuration;
                AnimationFrame = DeltaMath.Wrap(AnimationFrame + 1, 0, TotalFrames);
                if (AnimationFrame == TotalFrames && !IsLooped)
                    IsFinished = true;
            }
            ImageFrame = Animation.Frames[AnimationFrame];
        }

        protected override void OnAnimationChanged()
        {
 	        base.OnAnimationChanged();
            AnimationFrame = 0;
            _frameDurationTimer = Animation.FrameDuration;
            IsFinished = false;
        }
    }
}
