using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Delta.Structures;
using System.ComponentModel;

namespace Delta.Graphics
{
    [Flags]
    public enum AnimationOptions
    {
        None = 0x0,
        Looped = 0x1,
        StartOnRandomFrame = 0x2,
        RemoveWhenFinished = 0x4
    }

    public class SpriteEntity : TransformableEntity, IRecyclable
    {
        static Pool<SpriteEntity> _pool = new Pool<SpriteEntity>(100);

        public static SpriteEntity Create(string spriteSheet)
        {
            SpriteEntity se = _pool.Fetch();
            se.SpriteSheetName = spriteSheet;
            return se;
        }

        SpriteSheet _spriteSheet = null;
        Animation _animation = null;
        Rectangle _sourceRectangle = Rectangle.Empty;
        internal int _animationFrame = 0;
        internal float _frameDurationTimer = 0.0f;

        [ContentSerializerIgnore, Browsable(false)]
        public bool IsAnimationFinished { get; private set; }
        [ContentSerializerIgnore, Browsable(false)]
        public bool IsAnimationPlaying { get { return !IsAnimationFinished; } }
        [ContentSerializerIgnore, Browsable(false)]
        public bool IsOutlined { get; set; }
        [ContentSerializerIgnore, Browsable(false)]
        public Color OutlineColor { get; set; }

        string _spriteSheetName = string.Empty;
        [ContentSerializer(ElementName = "SpriteSheet"), DisplayName("SpriteSheet"), Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue("")]
        public string SpriteSheetName
        {
            get { return _spriteSheetName; }
            set
            {
                if (_spriteSheetName != value)
                {
                    _spriteSheetName = value;
                    NeedsHeavyUpdate = true;
                    OnPropertyChanged();
                }
            }
        }

        string _animationName = string.Empty;
        [ContentSerializer(ElementName = "Animation"), DisplayName("Animation"), Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue("")]
        public string AnimationName
        {
            get { return _animationName; }
            set
            {
                if (_animationName != value)
                {
                    _animationName = value;
                    NeedsHeavyUpdate = true;
                    OnPropertyChanged();
                }
            }
        }

        AnimationOptions _animationOptions = AnimationOptions.None;
        [ContentSerializer(ElementName = "PlayOptions"), DisplayName("PlayOptions"), Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue(AnimationOptions.None), Editor(typeof(Delta.Editor.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public AnimationOptions AnimationOptions
        {
            get { return _animationOptions; }
            set
            {
                if (_animationOptions != value)
                {
                    _animationOptions = value;
                    Reset();
                    OnPropertyChanged();
                }
            }
        }

        int _animationFrameOffset = 0;
        [ContentSerializer(ElementName = "FrameOffset"), Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue(0)]
        public int AnimationFrameOffset
        {
            get { return _animationFrameOffset; }
            set
            {
                if (_animationFrameOffset != value)
                {
                    _animationFrameOffset = value;
                    Reset();
                    OnPropertyChanged();
                }
            }
        }

        float _timeScale = 1.0f;
        [ContentSerializer, Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue(1.0f)]
        public float TimeScale
        {
            get { return _timeScale; }
            set
            {
                if (_timeScale != value)
                {
                    _timeScale = value;
                    OnPropertyChanged();
                }
            }
        }

        SpriteEffects _spriteEffects = SpriteEffects.None;
        [ContentSerializer, Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue(SpriteEffects.None), Editor(typeof(Delta.Editor.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public SpriteEffects SpriteEffects
        {
            get { return _spriteEffects; }
            set
            {
                if (_spriteEffects != value)
                {
                    _spriteEffects = value;
                    OnPropertyChanged();
                }
            }
        }

        bool _isAnimationPaused = false;
        [ContentSerializerIgnore, DisplayName("AnimationPaused"), Description(""), Category("Sprite"), Browsable(true), ReadOnly(false), DefaultValue(false)]
        public bool IsAnimationPaused
        {
            get { return _isAnimationPaused; }
            set
            {
                if (_isAnimationPaused != value)
                {
                    _isAnimationPaused = value;
                    OnPropertyChanged();
                }
            }
        }

        public SpriteEntity()
            : base()
        {
            IsAnimationFinished = true;
        }

        public SpriteEntity(string id, string spriteSheet)
            : base(id)
        {
            SpriteSheetName= spriteSheet;
            IsAnimationFinished = true;
        }

        public override void Recycle()
        {
            base.Recycle();
            _spriteSheet = null;
            _animation = null;
            _sourceRectangle = Rectangle.Empty;
            _animationFrame = 0;
            _frameDurationTimer = 0.0f;
            _spriteSheetName = string.Empty;
            _animationName = string.Empty;
            _animationOptions = AnimationOptions.None;
            _animationFrameOffset = 0;
            _timeScale = 1.0f;
            _spriteEffects = SpriteEffects.None;
            _isAnimationPaused = false;
            IsAnimationFinished = true;
            SpriteSheetName = string.Empty;
            SpriteEffects = SpriteEffects.None;
            IsOutlined = false;
            OutlineColor = Color.White;
            _pool.Release(this);
        }

#if WINDOWS
        protected internal override bool SetValue(string name, string value)
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
                    _animationOptions |= AnimationOptions.StartOnRandomFrame;
                    return true;
                case "islooped":
                case "looped":
                case "loop":
                    _animationOptions |= AnimationOptions.Looped;
                    return true;
                case "ispaused":
                case "paused":
                case "isstopped":
                case "stopped":
                    IsAnimationPaused = bool.Parse(value);
                    return true;
                case "foffset":
                case "frameoffset":
                    _animationFrameOffset = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
            }
            return base.SetValue(name, value);
        }
#endif

        protected override void LoadContent()
        {
            if (!string.IsNullOrEmpty(_spriteSheetName))
                _spriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
            Play(_animationName, _animationOptions | Graphics.AnimationOptions.Looped);
            base.LoadContent();
        }

        protected override void LightUpdate(DeltaGameTime time)
        {
            if (_animation != null && !IsAnimationFinished && !IsAnimationPaused)
                UpdateAnimationFrame(time);
            base.LightUpdate(time);
        }

        protected internal virtual void UpdateAnimationFrame(DeltaGameTime time)
        {
            _frameDurationTimer -= time.ElapsedSeconds * TimeScale;
            if (_frameDurationTimer <= 0f)
            {
                _frameDurationTimer = _animation.FrameDuration;
                _animationFrame = (_animationFrame + 1).Wrap(0, _animation.Frames.Count - 1);
                if (((_animationOptions & AnimationOptions.Looped) == 0) && _animationFrame >= _animation.Frames.Count - 1)
                {
                    IsAnimationFinished = true;
                    _frameDurationTimer = 0;
                    if ((_animationOptions & AnimationOptions.RemoveWhenFinished) != 0)
                        RemoveNextFrame();
                }
                UpdateSourceRectangle();
            }
        }

        protected virtual void UpdateSourceRectangle()
        {
            Rectangle previousSourceRectangle = _sourceRectangle;
            _sourceRectangle = _spriteSheet.GetFrameSourceRectangle(_animation.ImageName, _animation.Frames[_animationFrame] + _animationFrameOffset);
            if (_sourceRectangle != Rectangle.Empty)
            {
                if (previousSourceRectangle.Width != _sourceRectangle.Width || previousSourceRectangle.Height != _sourceRectangle.Height)
                    Size = new Vector2(_sourceRectangle.Width, _sourceRectangle.Height);
            }
        }

        protected override bool CanDraw()
        {
            if (_animation == null || _spriteSheet == null || _spriteSheet.Texture == null)
                return false;
            return base.CanDraw();
        }

        //protected virtual bool OnCamera()
        //{
        //    Rectangle spriteArea = new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X * Scale.X), (int)(Size.Y * Size.Y));
        //    Rectangle viewingArea = G.World.Camera.ViewingArea;
        //    viewingArea.Inflate(16, 16); // pad the viewing area with a border of off-screen tiles. for smooth scrolling, otherwise tiles seem to 'pop' in.
        //    return (viewingArea.Contains(spriteArea) || viewingArea.Intersects(spriteArea));
        //}

        protected override void Draw(DeltaGameTime gameTime, SpriteBatch spriteBatch)
        {
            //if (IsOutlined)
            //{
            //    spriteBatch.End();
            //    G.SimpleEffect.SetTechnique(Effects.SimpleEffect.Technique.FillColor);
            //    G.SimpleEffect.Color = OutlineColor;
            //    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, G.SimpleEffect, G.World.Camera.View);
            //    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition - Vector2.UnitY, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
            //    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition + Vector2.UnitX, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
            //    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition + Vector2.UnitY, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
            //    spriteBatch.Draw(_spriteSheet.Texture, RenderPosition - Vector2.UnitX, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
            //    spriteBatch.End();
            //    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, G.World.Camera.View);
            //}
            spriteBatch.Draw(_spriteSheet.Texture, RenderPosition, _sourceRectangle, RenderColor, RenderRotation, (Size * Pivot), Scale, SpriteEffects, 0);
        }

        public void Reset()
        {
            Play(_animationName, _animationOptions);
        }

        public void Play(string animation)
        {
            Play(animation, AnimationOptions.None, 0);
        }
 
        public void Play(string animation, AnimationOptions options)
        {
            Play(animation, options, 0);
        }

        public void Play(string animation, AnimationOptions options, int frameOffset)
        {
            AnimationName = animation;
            AnimationOptions = options;
            IsAnimationPaused = false;
            IsAnimationFinished = false;
            AnimationFrameOffset = frameOffset;
            if (_spriteSheet == null)
            {
                _animation = null;
                return;
            }
            _animation = _spriteSheet.GetAnimation(_animationName);
            if (_animation != null)
                OnAnimationChanged();
        }

        public void Pause()
        {
            IsAnimationPaused = true;
        }
        
        protected internal override void OnPositionChanged()
        {
            base.OnPositionChanged();
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected internal virtual void OnAnimationChanged()
        {
            _animationFrame = _animationFrameOffset;
            if ((AnimationOptions & AnimationOptions.StartOnRandomFrame) != 0)
                _animationFrame = G.Random.Next(0, _animation.Frames.Count - 1);
            _frameDurationTimer = _animation.FrameDuration;
            IsAnimationFinished = false;
            UpdateSourceRectangle();
        }

        protected internal override void OnRemoved()
        {
            if ((_animationOptions & AnimationOptions.RemoveWhenFinished) != 0)
                Recycle();
            base.OnRemoved();
        }

        public override string ToString()
        {
            return string.Format("Name:{0}, Position:({1},{2}), Animation:{3}, Frame:{4} of {5}", Name, Position.X, Position.Y, _animation == null ? string.Empty : _animation.Name, _animationFrame, _animation == null ? 0 : _animation.Frames.Count - 1);
        }
    }

}
