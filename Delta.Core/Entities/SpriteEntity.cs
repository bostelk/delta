using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Delta.Structures;

namespace Delta.Graphics
{
    public class SpriteEntity : Entity, IRecyclable
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
        [ContentSerializer(ElementName = "FrameOffset")] //for Rob's convience
        public int AnimationFrameOffset { get; set; }
        [ContentSerializer]
        public SpriteEffects SpriteEffects { get; set; }
        [ContentSerializer]
        public bool IsLooped { get; set; }
        [ContentSerializer]
        public bool IsPaused { get; set; }
        [ContentSerializer]
        public bool IsFinished { get; set; }
        [ContentSerializer]
        public bool IsOverlay { get; set; }
        [ContentSerializer]
        public bool StartOnRandomFrame { get; set; }
        [ContentSerializer]
        public bool Outline { get; set; }

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
            IsLooped = true;
        }

        public SpriteEntity(string spriteSheet)
            : this()
        {
            _spriteSheetName= spriteSheet;
        }

#if WINDOWS
        protected internal override bool ImportCustomValues(string name, string value)
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
                    StartOnRandomFrame = bool.Parse(value);
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

        protected override void LightUpdate(DeltaTime time)
        {
            if (_animation != null && !IsFinished && !IsPaused)
                UpdateAnimationFrame(time);
            base.LightUpdate(time);
        }

        protected internal virtual void UpdateAnimationFrame(DeltaTime time)
        {
            _frameDurationTimer -= (float)time.TotalSeconds;
            if (_frameDurationTimer <= 0f)
            {
                _frameDurationTimer = _animation.FrameDuration;
                _animationFrame = (_animationFrame + 1).Wrap(0, _animation.Frames.Count - 1);
                if (!IsLooped && _animationFrame >= _animation.Frames.Count - 1)
                {
                    IsFinished = true;
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

        protected override void Draw(DeltaTime gameTime, SpriteBatch spriteBatch)
        {
            if (Outline)
            {
                spriteBatch.End();
                G.SimpleEffect.SetTechnique(Effects.SimpleEffect.Technique.FillColor);
                G.SimpleEffect.Color = OutlineColor;
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, G.SimpleEffect, G.World.Camera.View);
                spriteBatch.Draw(_spriteSheet.Texture, RenderPosition - Vector2.UnitY, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                spriteBatch.Draw(_spriteSheet.Texture, RenderPosition + Vector2.UnitX, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                spriteBatch.Draw(_spriteSheet.Texture, RenderPosition + Vector2.UnitY, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                spriteBatch.Draw(_spriteSheet.Texture, RenderPosition - Vector2.UnitX, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, G.World.Camera.View);
            }
            spriteBatch.Draw(_spriteSheet.Texture, RenderPosition, _sourceRectangle, Tint, Rotation, RenderOrigin, Scale, SpriteEffects, 0);
        }

        public void Play(string animation)
        {
            _animationName = animation;
            IsPaused = false;
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
            if (StartOnRandomFrame)
                _animationFrame = G.Random.Next(0, _animation.Frames.Count - 1);
            _frameDurationTimer = _animation.FrameDuration;
            IsFinished = false;
            // draw the first frame
            _sourceRectangle = _spriteSheet.GetFrameSourceRectangle(_animation.ImageName, _animation.Frames[_animationFrame] + AnimationFrameOffset);
            Size = new Vector2(_sourceRectangle.Width, _sourceRectangle.Height);
        }

        public override void Recycle()
        {
            base.Recycle();
            IsLooped = true;
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
