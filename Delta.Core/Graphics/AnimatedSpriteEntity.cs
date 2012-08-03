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
        Recycle = 0x4
    }

    public class AnimatedSpriteEntity : BaseSpriteEntity
    {
        static Pool<AnimatedSpriteEntity> _pool = new Pool<AnimatedSpriteEntity>(100);

        public static AnimatedSpriteEntity Create(string spriteSheet)
        {
            AnimatedSpriteEntity se = _pool.Fetch();
            se.SpriteSheetName = spriteSheet;
            return se;
        }

        int _animationFrame = 0;
        float _animationframeDurationTimer = 0.0f;

        [ContentSerializerIgnore, Browsable(false)]
        public bool IsAnimating { get; private set; }
        [ContentSerializerIgnore, Browsable(false)]
        public bool IsAnimationFinished { get; private set; }

        Animation _animation = null;
        public Animation Animation
        {
            get { return _animation; }
            set
            {
                if (_animation != value)
                {
                    _animation = value;
                    OnAnimationChanged();
                }
            }
        }

        string _animationName = string.Empty;
        [ContentSerializer(ElementName = "Animation"), DisplayName("Animation"), Description(""), Category("Animation"), Browsable(true), ReadOnly(false), DefaultValue("")]
        public string AnimationName
        {
            get { return _animationName; }
            set
            {
                if (_animationName != value)
                {
                    _animationName = value;
                    UpdateAnimation();
                    OnPropertyChanged();
                }
            }
        }

        AnimationOptions _animationOptions = AnimationOptions.None;
        [ContentSerializer(ElementName = "PlayOptions"), DisplayName("PlayOptions"), Description(""), Category("Animation"), Browsable(true), ReadOnly(false), DefaultValue(AnimationOptions.None), Editor(typeof(Delta.Editor.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public AnimationOptions AnimationOptions
        {
            get { return _animationOptions; }
            set
            {
                if (_animationOptions != value)
                {
                    _animationOptions = value;
                    OnPropertyChanged();
                }
            }
        }

        int _frameOffset = 0;
        [ContentSerializer(ElementName = "FrameOffset"), Description(""), Category("Animation"), Browsable(true), ReadOnly(false), DefaultValue(0)]
        public int FrameOffset
        {
            get { return _frameOffset; }
            set
            {
                if (_frameOffset != value)
                {
                    _frameOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        bool _isAnimationPaused = false;
        [ContentSerializerIgnore, DisplayName("AnimationPaused"), Description(""), Category("Animation"), Browsable(true), ReadOnly(false), DefaultValue(false)]
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

        public AnimatedSpriteEntity()
            : base()
        {
        }

        public override void Recycle()
        {
            base.Recycle();
            _animation = null;
            _animationFrame = 0;
            _animationframeDurationTimer = 0.0f;
            _animationName = string.Empty;
            _animationOptions = AnimationOptions.None;
            _frameOffset = 0;
            _isAnimationPaused = false;
            IsAnimating = false;
            IsAnimationFinished = false;
            _pool.Release(this);
        }

#if WINDOWS
        protected internal override bool SetValue(string name, string value)
        {
            switch (name)
            {
                case "animation":
                case "animationname":
                    _animationName = value;
                    return true;
                case "startrandom":
                case "random":
                    _animationOptions |= AnimationOptions.StartOnRandomFrame;
                    return true;
                case "ispaused":
                case "paused":
                case "isstopped":
                case "stopped":
                    IsAnimationPaused = bool.Parse(value);
                    return true;
                case "foffset":
                case "frameoffset":
                    _frameOffset = int.Parse(value, CultureInfo.InvariantCulture);
                    return true;
            }
            return base.SetValue(name, value);
        }
#endif

        protected override void LightUpdate(DeltaGameTime time)
        {
            if (_animation != null && !IsAnimationFinished && !IsAnimationPaused)
                UpdateAnimationFrame(time);
            base.LightUpdate(time);
        }

        protected virtual void UpdateAnimation()
        {
            if (SpriteSheet == null)
                return;
            Animation = SpriteSheet.GetAnimation(_animationName);
        }

        protected internal virtual void UpdateAnimationFrame(DeltaGameTime time)
        {
            _animationframeDurationTimer -= time.ElapsedSeconds * TimeScale;
            if (_animationframeDurationTimer <= 0f)
            {
                _animationframeDurationTimer = _animation.FrameDuration;
                _animationFrame = (_animationFrame + 1).Wrap(0, _animation.Frames.Count - 1);
                Frame = _animation.Frames[_animationFrame];
                if (((_animationOptions & AnimationOptions.Looped) == 0) && _animationFrame >= _animation.Frames.Count - 1)
                {
                    IsAnimationFinished = true;
                    IsAnimating = false;
                    _animationframeDurationTimer = 0;
                    if ((_animationOptions & AnimationOptions.Recycle) != 0)
                        RemoveNextFrame();
                }
            }
        }

        protected override void UpdateSourceRectangle()
        {
            if (SpriteSheet == null)
                return;
            SourceRectangle = SpriteSheet.GetFrameSourceRectangle(_animation.ImageName, Frame + _frameOffset);
        }

        protected override bool CanDraw()
        {
            if (_animation == null)
                return false;
            return base.CanDraw();
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
            IsAnimating = true;
            IsAnimationFinished = false;
            FrameOffset = frameOffset;
        }

        public void Pause()
        {
            IsAnimationPaused = true;
        }

        protected override void OnSpriteSheetChanged()
        {
            base.OnSpriteSheetChanged();
            UpdateAnimation();
        }

        protected internal virtual void OnAnimationChanged()
        {
            if (_animation == null)
                return;
            _animationFrame = _frameOffset;
            Frame = _animation.Frames[_animationFrame];
            if ((AnimationOptions & AnimationOptions.StartOnRandomFrame) != 0)
                _animationFrame = G.Random.Next(0, _animation.Frames.Count - 1);
            _animationframeDurationTimer = _animation.FrameDuration;
            IsAnimationFinished = false;
            UpdateSourceRectangle();
        }

        protected internal override void OnRemoved()
        {
            if ((_animationOptions & AnimationOptions.Recycle) != 0)
                Recycle();
            base.OnRemoved();
        }
    }

}
