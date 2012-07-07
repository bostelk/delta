using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{

    [Flags]
    internal enum SpriteState
    {
        None = 0x0,
        Paused = 0x1,
        Looped = 0x2,
        RandomFrameStart = 0x4,
        Finished = 0x8,
        //temporary?
        Overlay = 0x10
    }

    public class SpriteEntity : TransformableEntity
    {
        internal SpriteSheet _spriteSheet = null;
        internal Rectangle _sourceRectangle = Rectangle.Empty;
        internal float _frameDurationTimer = 0f;
        internal int _animationFrame = 0;
        internal Animation _animation = null;
        internal SpriteState _state = SpriteState.Looped;

        [ContentSerializer(ElementName = "SpriteSheet")] //for Rob's convience
        public string SpriteSheetName { get; private set; }
        [ContentSerializer(ElementName = "Animation")] //for Rob's convience
        public string AnimationName { get; set; }
        [ContentSerializer(ElementName = "FrameOffset")] //for Rob's convience
        public int AnimationFrameOffset { get; set; }

        [ContentSerializer]
        protected virtual SpriteEffects SpriteEffects { get; set; }

        [ContentSerializerIgnore]
        public bool IsPaused
        {
            get { return _state.HasFlag(SpriteState.Paused); }
            set
            {
                if (value)
                    _state |= SpriteState.Paused;
                else
                    _state ^= SpriteState.Paused;
            }
        }

        [ContentSerializerIgnore]
        public bool IsLooped
        {
            get { return _state.HasFlag(SpriteState.Looped); }
            set
            {
                if (value)
                    _state |= SpriteState.Looped;
                else
                    _state ^= SpriteState.Looped;
            }
        }

        public SpriteEntity()
            : base()
        {
            AnimationFrameOffset = 0;
        }

        public SpriteEntity(string spriteSheet)
            : this()
        {
            SpriteSheetName = spriteSheet;
        }

#if WINDOWS
        protected override bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "animation":
                case "animationname":
                    AnimationName = value;
                    return true;
                //temporary?
                case "islight":
                case "light":
                case "overlay":
                case "isoverlay":
                    if (bool.Parse(value))
                        _state |= SpriteState.Overlay;
                    else
                        _state ^= SpriteState.Overlay;
                    break;
                case "mirror":
                case "flip":
                    switch (value)
                    {
                        case "left":
                        case "right":
                        case "horizontal":
                        case "h":
                            SpriteEffects = SpriteEffects.FlipHorizontally;
                            return true;
                        case "up":
                        case "down":
                        case "vertical":
                        case "v":
                            SpriteEffects = SpriteEffects.FlipVertically;
                            return true;
                    }
                    break;
                case "randomframestart":
                case "randomstart":
                case "random":
                    if (bool.Parse(value))
                        _state |= SpriteState.RandomFrameStart;
                    else
                        _state ^= SpriteState.RandomFrameStart;
                    return true;
                case "ispaused":
                case "paused":
                case "isstopped":
                case "stopped":
                    if (bool.Parse(value))
                        _state |= SpriteState.Paused;
                    else
                        _state ^= SpriteState.Paused;
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }
#endif

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
            if (_state.HasFlag(SpriteState.Finished))
                return;
            _frameDurationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameDurationTimer <= 0f)
            {
                _frameDurationTimer = _animation.FrameDuration;
                _animationFrame = (_animationFrame + 1).Wrap(0, _animation.Frames.Count - 1);
                if (!_state.HasFlag(SpriteState.Looped) && _animationFrame >= _animation.Frames.Count - 1)
                {
                    _state |= SpriteState.Finished;
                    _frameDurationTimer = 0;
                }
                Rectangle previousSourceRectangle = _sourceRectangle;
                _sourceRectangle = _spriteSheet.GetFrameSourceRectangle(_animation.ImageName, _animation.Frames[_animationFrame] + AnimationFrameOffset);
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
            _animationFrame = AnimationFrameOffset;
            if (_state.HasFlag(SpriteState.RandomFrameStart))
                _animationFrame = G.Random.Next(0, _animation.Frames.Count - 1);
            _frameDurationTimer = _animation.FrameDuration;
            _state ^= SpriteState.Finished;
        }

    }

}
