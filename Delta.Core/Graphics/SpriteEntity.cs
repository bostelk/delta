using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{ 

    public class SpriteEntity : TransformableEntity
    {
        internal SpriteSheet _spriteSheet = null;
        internal Rectangle _sourceRectangle = Rectangle.Empty;
        float _frameDurationTimer = 0f;
        Animation _animation = null;

        [ContentSerializer(ElementName = "Frame")] //for Rob's convience
        public int AnimationFrame { get; private set; }
        [ContentSerializer(ElementName = "Paused")] //for Rob's convience
        public bool AnimationIsPaused { get; set; }
        [ContentSerializer(ElementName = "Looped")] //for Rob's convience
        public bool AnimationIsLooped { get; set; }
        [ContentSerializerIgnore]
        public bool AnimationIsFinished { get; private set; }
        [ContentSerializer(ElementName = "SpriteSheet")] //for Rob's convience
        public string SpriteSheetName { get; set; }
        [ContentSerializer(ElementName = "Animation")] //for Rob's convience
        public string AnimationName { get; set; }
        [ContentSerializer]
        public SpriteEffects SpriteEffects { get; set; }
        [ContentSerializer(ElementName = "FrameOffset")] //for Rob's convience
        public int AnimationFrameOffset { get; set; }
        [ContentSerializer(ElementName = "StartRandom")]
        public bool RandomFrameStart { get; set; }

        //temporary?
        [ContentSerializer]
        public bool IsOverlay { get; set; }

        public SpriteEntity()
            : base()
        {
            AnimationIsLooped = true;
            AnimationFrameOffset = 0;
        }

        public override void LoadContent()
        {
            _spriteSheet = G.Content.Load<SpriteSheet>(SpriteSheetName);
            base.LoadContent();
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            UpdateAnimation();
            if (_animation != null)
                UpdateAnimationFrame(gameTime);
            base.LightUpdate(gameTime);
        }

        protected internal void UpdateAnimation()
        {
            if (_spriteSheet == null)
            {
                _animation = null;
                return;
            }
            if (_animation == null || _animation.Name != AnimationName)
            {
                _animation = _spriteSheet.GetAnimation(AnimationName);
                if (_animation != null)
                    OnAnimationChanged();
            }
        }

        protected internal virtual void UpdateAnimationFrame(GameTime gameTime)
        {
            if (AnimationIsFinished)
                return;
            _frameDurationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameDurationTimer <= 0f)
            {
                _frameDurationTimer = _animation.FrameDuration;
                AnimationFrame = DeltaMath.Wrap(AnimationFrame + 1, 0, _animation.Frames.Count - 1);
                if (!AnimationIsLooped && AnimationFrame >= _animation.Frames.Count - 1)
                {
                    AnimationIsFinished = true;
                    _frameDurationTimer = 0;
                }
                Rectangle previousSourceRectangle = _sourceRectangle;
                _sourceRectangle = _spriteSheet.GetFrameSourceRectangle(_animation.ImageName, _animation.Frames[AnimationFrame] + AnimationFrameOffset);
                if (_sourceRectangle != Rectangle.Empty)
                {
                    if (previousSourceRectangle.Width != _sourceRectangle.Width || previousSourceRectangle.Height != _sourceRectangle.Height)
                        Size = new Vector2(_sourceRectangle.Width, _sourceRectangle.Height);
                }
            }
        }

        protected override bool CanDraw()
        {
            if (_animation == null || _spriteSheet == null || _spriteSheet.Texture == null) return false;
            return base.CanDraw();
        }

        protected internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteSheet.Texture, RenderPosition, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
        }

        protected internal override void OnPositionChanged()
        {
            base.OnPositionChanged();
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected internal virtual void OnAnimationChanged()
        {
            AnimationFrame = AnimationFrameOffset;
            if (RandomFrameStart)
                AnimationFrame = G.Random.Next(0, _animation.Frames.Count - 1);
            _frameDurationTimer = _animation.FrameDuration;
            AnimationIsFinished = false;
        }

    }

}
