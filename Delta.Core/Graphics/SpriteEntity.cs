using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Delta.Structures;

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
        Overlay = 0x10,
        OutlineTop = 0x20,
        OutlineRight = 0x40,
        OutlineBottom = 0x80,
        OutlineLeft = 0x100,
    }

    public class SpriteEntity : TransformableEntity, IRecyclable
    {
        static Pool<SpriteEntity> _pool;

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
        [ContentSerializer(ElementName = "FrameOffset")] //for Rob's convience
        public int AnimationFrameOffset { get; set; }
        [ContentSerializer]
        public SpriteEffects SpriteEffects { get; set; }

        [ContentSerializerIgnore]
        public bool IsPaused
        {
            get { return _state.HasFlag(SpriteState.Paused); }
            set
            {
                if (value)
                    _state |= SpriteState.Paused;
                else
                    _state &= ~SpriteState.Paused;
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
                    _state &= ~SpriteState.Looped;
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
                    _state &= ~SpriteState.Finished;
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
                    _state &= ~SpriteState.Overlay;
            }
        }

        [ContentSerializerIgnore]
        public Color OutlineColor
        {
            get;
            set;
        }

        static SpriteEntity()
        {
            _pool = new Pool<SpriteEntity>(100);
        }
        
        public static SpriteEntity Create(string spriteSheet) 
        {
            SpriteEntity spriteEntity = _pool.Fetch();
            spriteEntity._spriteSheetName = spriteSheet;
            return spriteEntity;
        }

        public SpriteEntity()
            : base()
        {
            AnimationFrameOffset = 0;
        }

        public SpriteEntity(string spriteSheet)
            : this()
        {
            _spriteSheetName= spriteSheet;
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
                    string[] split = value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                    switch (split[0])
                    {
                        case "left":
                        case "right":
                        case "horizontal":
                        case "h":
                            SpriteEffects |= SpriteEffects.FlipHorizontally;
                            return true;
                        case "up":
                        case "down":
                        case "vertical":
                        case "v":
                            SpriteEffects |= SpriteEffects.FlipVertically;
                            return true;
                    }
                    if (split.Length > 1)
                    {
                        switch (split[1])
                        {
                            case "left":
                            case "right":
                            case "horizontal":
                            case "h":
                                SpriteEffects |= SpriteEffects.FlipHorizontally;
                                return true;
                            case "up":
                            case "down":
                            case "vertical":
                            case "v":
                                SpriteEffects |= SpriteEffects.FlipVertically;
                                return true;
                        }
                    }
                    break;
                case "startrandom":
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
                case "foffset":
                case "frameoffset":
                    AnimationFrameOffset = int.Parse(value, CultureInfo.InvariantCulture);
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
            if (_animation != null && !IsFinished)
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
            if (_animation == null || _spriteSheet == null || _spriteSheet.Texture == null || !OnCamera()) return false;
            return base.CanDraw();
        }

        protected virtual bool OnCamera()
        {
            Rectangle spriteArea = new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Scale.X), (int)(Size.Y * Size.Y));
            Rectangle viewingArea = G.World.Camera.ViewingArea;
            viewingArea.Inflate(16, 16); // pad the viewing area with a border of off-screen tiles. for smooth scrolling, otherwise tiles seem to 'pop' in.
            return (viewingArea.Contains(spriteArea) || viewingArea.Intersects(spriteArea));
        }

        protected internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_state.HasFlag(SpriteState.OutlineTop) || _state.HasFlag(SpriteState.OutlineRight) || _state.HasFlag(SpriteState.OutlineBottom) || _state.HasFlag(SpriteState.OutlineLeft))
            {
                spriteBatch.End();
                G.SimpleEffect.SetTechnique(Effects.SimpleEffect.Technique.FillColor);
                G.SimpleEffect.Color = OutlineColor;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, G.SimpleEffect, G.World.Camera.View);
                if (_state.HasFlag(SpriteState.OutlineTop))
                    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition - Vector2.UnitY, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                if (_state.HasFlag(SpriteState.OutlineRight))
                    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition + Vector2.UnitX, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                if (_state.HasFlag(SpriteState.OutlineBottom))
                    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition + Vector2.UnitY, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                if (_state.HasFlag(SpriteState.OutlineLeft))
                    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition - Vector2.UnitX, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, G.World.Camera.View);
            }

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

        public void Outline(bool top, bool right, bool bottom, bool left)
        {
            if (top)
                _state |= SpriteState.OutlineTop;
            else
                _state &= ~SpriteState.OutlineTop;
            if (right)
                _state |= SpriteState.OutlineRight;
            else
                _state &= ~SpriteState.OutlineRight;
            if (bottom)
                _state |= SpriteState.OutlineBottom;
            else
                _state &= ~SpriteState.OutlineBottom;
            if (left)
                _state |= SpriteState.OutlineLeft;
            else
                _state &= ~SpriteState.OutlineLeft;
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
            _state &= ~SpriteState.Finished;
        }

        public override void Recycle()
        {
            base.Recycle();
            _state = SpriteState.Looped;
            _spriteSheet = null;
            _spriteSheetName = string.Empty;
            _animation = null;
            _animationName = string.Empty;
            _animationFrame = 0;
            _sourceRectangle = Rectangle.Empty;
            _frameDurationTimer = 0f;
            AnimationFrameOffset = 0;
            SpriteEffects = SpriteEffects.None;
            OutlineColor = Color.White;

            RemoveNextUpdate = true;
            _pool.Release(this);
        }
    }

}
