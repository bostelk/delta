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
        [ContentSerializer(ElementName = "SpriteSheet")]
        internal string _spriteSheetName = string.Empty;
        internal SpriteSheet _spriteSheet = null;
        [ContentSerializer(ElementName = "Animation")]
        internal string _animationName = string.Empty;
        internal Animation _animation = null;
        internal Rectangle _sourceRectangle = Rectangle.Empty;
        internal float _frameDurationTimer = 0f;
        [ContentSerializer(ElementName = "Frame")]
        internal int _animationFrame = 0;
        [ContentSerializer(ElementName = "State")]
        internal SpriteState _state = SpriteState.Looped;

        [ContentSerializer(ElementName = "SpriteSheet")] //for Rob's convience
        public string SpriteSheetName { get; private set; }
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

        [ContentSerializerIgnore]
        public bool IsFinished
        {
            get { return _state.HasFlag(SpriteState.Finished); }
            private set
            {
                if (value)
                    _state |= SpriteState.Finished;
                else
                    _state ^= SpriteState.Finished;
            }
        }

        [ContentSerializerIgnore]
        public bool IsOverlay
        {
            get { return _state.HasFlag(SpriteState.Overlay); }
            set
            {
                if (value)
                    _state |= SpriteState.Overlay;
                else
                    _state ^= SpriteState.Overlay;
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
                case "spritesheet":
                case "spritesheetname":
                    _spriteSheetName = value;
                    return true;
                case "animation":
                case "animationname":
                    _animationName = value;
                    return true;
                //temporary?
                case "isshadow":
                case "shadow":
                case "islight":
                case "light":
                case "overlay":
                case "isoverlay":
                    IsOverlay = bool.Parse(value);
                    return true;
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
                    IsPaused = bool.Parse(value);
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }
#endif

        public override void LoadContent()
        {
            if (!string.IsNullOrEmpty(_spriteSheetName))
                _spriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
            Play(_animationName);
            base.LoadContent();
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            if (_animation != null || !IsFinished)
                UpdateAnimationFrame(gameTime);
            base.LightUpdate(gameTime);
        }

        protected internal virtual void UpdateAnimationFrame(GameTime gameTime)
        {
            _frameDurationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameDurationTimer <= 0f)
            {
                _frameDurationTimer = _animation.FrameDuration;
                _animationFrame = (_animationFrame + 1).Wrap(0, _animation.Frames.Count - 1);
                if (!IsLooped && _animationFrame >= _animation.Frames.Count - 1)
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

        public void Play(string animation)
        {
            _animationName = animation;
            if (_spriteSheet == null)
            {
                _animation = null;
                return;
            }
            _animation = _spriteSheet.GetAnimation(_animationName);
            if (_animation != null)
                OnAnimationChanged();
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
