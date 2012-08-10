using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

#if WINDOWS
using System.Drawing.Design;
using Delta.Editor;
#endif

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
        int _animationFrame = 0;
        float _animationframeDurationTimer = 0.0f;

        /// <summary>
        /// Gets or sets the frame index of the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Description("The frame index of the game object."), Category("Sprite"), ReadOnly(true)]
        public new int Frame { get { return base.Frame; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="AnimatedSpriteEntity"/> is currently animating.
        /// </summary>
        [ContentSerializerIgnore, Browsable(false)]
        public bool IsAnimating { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the <see cref="AnimatedSpriteEntity"/> is finished animating.
        /// </summary>
        [ContentSerializerIgnore, Browsable(false)]
        public bool IsAnimationFinished { get; private set; }

        Animation _animation = null;
        /// <summary>
        /// Gets the <see cref="Animation"/> used by the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Browsable(false)]
        public Animation Animation
        {
            get { return _animation; }
            internal set
            {
                if (_animation != value)
                {
                    _animation = value;
                    OnAnimationChanged();
                }
            }
        }

        string _animationName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the <see cref="Animation"/> used by the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        [ContentSerializer(ElementName = "Animation"), DisplayName("Animation"), Description("The name of the animation used by the game object."), Category("Animation"), DefaultValue("")]
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
        /// <summary>
        /// Gets or sets the <see cref="AnimationOptions"/> used when playing the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        [ContentSerializer(ElementName = "AnimationOptions"), DisplayName("AnimationOptions"), Description("The options used by the game object when playing the animation."), Category("Animation"), DefaultValue(AnimationOptions.None), 
#if WINDOWS
        Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))
#endif
        ]
        public AnimationOptions AnimationOptions
        {
            get { return _animationOptions; }
            set
            {
                if (_animationOptions != value)
                {
                    _animationOptions = value;
                    Play(_animationName, _animationOptions);
                    OnPropertyChanged();
                }
            }
        }

        int _frameOffset = 0;
        /// <summary>
        /// Gets or sets the <see cref="Frame"/> offset used when playing the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        [ContentSerializer(ElementName = "FrameOffset"), Description("The frame offset of the game object."), Category("Animation"), DefaultValue(0)]
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
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="AnimatedSpriteEntity"/> is currently paused.
        /// </summary>
        [ContentSerializerIgnore, DisplayName("AnimationPaused"), Description("Indicates whether the game object is paused."), Category("Animation"), DefaultValue(false)]
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

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected AnimatedSpriteEntity()
            : base()
        {
        }

        public static AnimatedSpriteEntity Create(string spriteSheet)
        {
            AnimatedSpriteEntity se = Pool.Acquire<AnimatedSpriteEntity>();
            se.SpriteSheetName = spriteSheet;
            return se;
        }

        protected override void Recycle(bool isReleasing)
        {
            _animation = null;
            _animationFrame = 0;
            _animationframeDurationTimer = 0.0f;
            _animationName = string.Empty;
            _animationOptions = AnimationOptions.None;
            _frameOffset = 0;
            _isAnimationPaused = false;
            IsAnimating = false;
            IsAnimationFinished = false;
            base.Recycle(isReleasing);
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

        /// <summary>
        /// Updates the <see cref="AnimatedSpriteEntity"/>. Override this method to add custom update logic which is executed every frame.
        /// </summary>
        /// <param name="time">time</param>
        protected override void LightUpdate(DeltaGameTime time)
        {
            if (_animation != null && !IsAnimationFinished && !IsAnimationPaused)
                UpdateAnimationFrame(time);
            base.LightUpdate(time);
        }

        /// <summary>
        /// Updates the <see cref="Animation"/> used by the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        protected virtual void UpdateAnimation()
        {
            if (SpriteSheet == null)
                return;
            Animation = SpriteSheet.GetAnimation(_animationName);
        }

        /// <summary>
        /// Updates the current animation frame of the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        /// <param name="time"></param>
        protected internal virtual void UpdateAnimationFrame(DeltaGameTime time)
        {
            _animationframeDurationTimer -= time.ElapsedSeconds * TimeScale;
            if (_animationframeDurationTimer <= 0f)
            {
                _animationframeDurationTimer = _animation.FrameDuration;
                _animationFrame = (_animationFrame + 1).Wrap(0, _animation.Frames.Count - 1);
                base.Frame = _animation.Frames[_animationFrame];
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

        /// <summary>
        /// Updates the source <see cref="Rectangle"/> used when drawing the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        protected override void UpdateSourceRectangle()
        {
            if (SpriteSheet == null || _animation == null)
                SourceRectangle = Rectangle.Empty;
            else
                SourceRectangle = SpriteSheet.GetFrameSourceRectangle(_animation.ImageName, Frame + _frameOffset);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="AnimatedSpriteEntity"/> is allowed to draw.
        /// </summary>
        /// <returns>A value indicating whether the <see cref="AnimatedSpriteEntity"/> is allowed to draw.</returns>
        protected override bool CanDraw()
        {
            if (_animation == null)
                return false;
            return base.CanDraw();
        }

        /// <summary>
        /// Plays the <see cref="AnimationSpriteEntity"/>. 
        /// </summary>
        /// <param name="animation">The name of the <see cref="Animation"/> to play.</param>
        public void Play(string animation)
        {
            Play(animation, AnimationOptions.None, 0);
        }

        /// <summary>
        /// Plays the <see cref="AnimationSpriteEntity"/>. 
        /// </summary>
        /// <param name="animation">The name of the <see cref="Animation"/> to play.</param>
        /// <param name="options">The <see cref="AnimationOptions"/> to use when playing the <see cref="AnimatedSpriteEntity"/>.</param>
        public void Play(string animation, AnimationOptions options)
        {
            Play(animation, options, 0);
        }

        /// <summary>
        /// Plays the <see cref="AnimationSpriteEntity"/>. 
        /// </summary>
        /// <param name="animation">The name of the <see cref="Animation"/> to play.</param>
        /// <param name="options">The <see cref="AnimationOptions"/> to use when playing the <see cref="AnimatedSpriteEntity"/>.</param>
        /// <param name="frameOffset">The frame offset used when playing the <see cref="AnimatedSpriteEntity"/>.</param>
        public void Play(string animation, AnimationOptions options, int frameOffset)
        {
            AnimationName = animation;
            AnimationOptions = options;
            IsAnimationPaused = false;
            IsAnimating = true;
            IsAnimationFinished = false;
            FrameOffset = frameOffset;
        }

        /// <summary>
        /// Pauses the <see cref="AnimatedSpriteEntity"/>.
        /// </summary>
        public void Pause()
        {
            IsAnimationPaused = true;
        }

        /// <summary>
        /// Called when the <see cref="AnimatedSpriteEntity"/>'s <see cref="SpriteSheet"/> has changed.
        /// </summary>
        protected override void OnSpriteSheetChanged()
        {
            UpdateAnimation();
            base.OnSpriteSheetChanged();
        }

        /// <summary>
        /// Called when the <see cref="AnimatedSpriteEntity"/>'s <see cref="Animation"/> has changed.
        /// </summary>
        protected internal virtual void OnAnimationChanged()
        {
            if (_animation == null)
                return;
            _animationFrame = _frameOffset;
            base.Frame = _animation.Frames[_animationFrame];
            if ((AnimationOptions & AnimationOptions.StartOnRandomFrame) != 0)
                _animationFrame = G.Random.Next(0, _animation.Frames.Count - 1);
            _animationframeDurationTimer = _animation.FrameDuration;
            IsAnimationFinished = false;
        }

        /// <summary>
        /// Called when the <see cref="AnimatedSpriteEntity"/> has been removed from an <see cref="IEntityCollection"/>.
        /// </summary>
        protected internal override void OnRemoved()
        {
            if ((_animationOptions & AnimationOptions.Recycle) != 0)
                Recycle();
            base.OnRemoved();
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class AnimatedSpriteReader : ContentTypeReader<AnimatedSpriteEntity>
    {
        protected override AnimatedSpriteEntity Read(ContentReader input, AnimatedSpriteEntity existingInstance)
        {
            if (existingInstance == null)
                existingInstance = Pool.Acquire<AnimatedSpriteEntity>();
            input.ReadRawObject<BaseSpriteEntity>(existingInstance as BaseSpriteEntity);
            existingInstance.AnimationName = input.ReadString();
            existingInstance.AnimationOptions = input.ReadObject<AnimationOptions>();
            existingInstance.FrameOffset = input.ReadInt32();
            return existingInstance;
        }
    }

}
