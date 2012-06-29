using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Graphics
{ 

    public class SpriteEntity : Entity
    {
        Vector2 _renderPosition = Vector2.Zero;
        Vector2 _renderOrigin = Vector2.Zero;
        int _currentImageFrame = -1;

        public float Rotation { get; set; }
        public Vector2 Size { get; set; }
        [ContentSerializer(ElementName = "SpriteSheet")]
        public string SpriteSheetName { get; set; }
        [ContentSerializer(ElementName="Animation")]
        public string AnimationName { get; set; }
        [ContentSerializer]
        public int ImageFrame { get; set; } 
        [ContentSerializer]
        public SpriteEffects SpriteEffects { get; set; }

        [ContentSerializerIgnore]
        protected Animation Animation { get; private set; }
        [ContentSerializerIgnore]
        protected internal Rectangle SourceRectangle { get; set; }
        [ContentSerializerIgnore]
        public SpriteSheet SpriteSheet { get; set; }

        [ContentSerializerIgnore]
        public int TotalFrames
        {
            get
            {
                if (Animation == null)
                    return 0;
                return Animation.Frames.Count - 1;
            }
        }

        Vector2 _scale = Vector2.One;
        [ContentSerializer]
        public Vector2 Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    UpdateRenderPosition();
                }
            }
        }

        Vector2 _origin = new Vector2(0.5f, 0.5f);
        [ContentSerializer(Optional = true)]
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                if (_origin != value)
                {
                    _origin = value;
                    UpdateRenderPosition();
                }
            }
        }

        Color _color = Color.White;
        //[ContentSerializer(ElementName = "Tint")]
        //public Color Color
        //{
        //    get { return _color; }
        //    set
        //    {
        //        _color.R = value.R;
        //        _color.G = value.G;
        //        _color.B = value.B;
        //    }
        //}

        //float _alpha = 1.0f;
        //[ContentSerializer]
        //public float Alpha
        //{
        //    get { return _alpha; }
        //    set
        //    {
        //        if (_alpha != value)
        //        {
        //            _alpha = MathHelper.Clamp(value, 0f, 1f);
        //            _color.A = (byte)(255f * _alpha);
        //        }
        //    }
        //}

        protected override void LightUpdate(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            if (Animation != null)
                UpdateImageFrame(gameTime);
            base.LightUpdate(gameTime);
        }

        protected internal virtual void UpdateAnimation(GameTime gameTime)
        {
            if (SpriteSheet == null)
            {
                Animation = null;
                return;
            }
            if (Animation == null || Animation.Name != AnimationName)
            {
                Animation = SpriteSheet.GetAnimation(AnimationName);
                if (Animation != null)
                    OnAnimationChanged();
            }
        }

        protected internal virtual void UpdateImageFrame(GameTime gameTime)
        {
            if (_currentImageFrame != ImageFrame)
            {
                _currentImageFrame = ImageFrame;
                Rectangle previousSourceRectangle = SourceRectangle;
                SourceRectangle = SpriteSheet.GetFrameSourceRectangle(Animation.ImageName, ImageFrame);
                if (SourceRectangle != Rectangle.Empty)
                {
                    if (previousSourceRectangle.Width != SourceRectangle.Width || previousSourceRectangle.Height != SourceRectangle.Height)
                    {
                        Size = new Vector2(SourceRectangle.Width, SourceRectangle.Height);
                        UpdateRenderPosition();
                    }
                }
            }
        }
        
        protected void UpdateRenderPosition()
        {
            if (SourceRectangle != Rectangle.Empty)
                _renderOrigin = Origin * Size;
            else
                _renderOrigin = Vector2.Zero;
            _renderPosition = Position + (_renderOrigin * _scale);
        }

        protected override bool CanDraw()
        {
            if (SpriteSheet == null || SpriteSheet.Texture == null || Animation == null) return false;
            return base.CanDraw();
        }

        protected internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteSheet.Texture, Position, SourceRectangle, _color, Rotation, _renderOrigin, _scale, SpriteEffects, 0);
        }

        protected virtual void OnAnimationChanged()
        {
        }

    }

}
