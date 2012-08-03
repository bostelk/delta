using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#if WINDOWS
using System.Drawing.Design;
using Delta.Editor;
#endif

namespace Delta.Graphics
{
    public abstract class BaseSpriteEntity : TransformableEntity
    {
        //[ContentSerializerIgnore, Browsable(false)]
        //public bool IsOutlined { get; set; }
        //[ContentSerializerIgnore, Browsable(false)]
        //public Color OutlineColor { get; set; }

        /// <summary>
        /// Gets the size of the <see cref="BaseSpriteEntity"/> <b>before it's scaled</b>.
        /// </summary>
        [ContentSerializerIgnore, Description("The size of the game object before it's scaled."), Category("Transform"), ReadOnly(true)]
        public new Vector2 Size { get { return base.Size; } }

        SpriteSheet _spriteSheet = null;
        /// <summary>
        /// Gets the <see cref="SpriteSheet"/> used by the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Browsable(false)]
        public SpriteSheet SpriteSheet
        {
            get { return _spriteSheet; }
            set
            {
                if (_spriteSheet != value)
                {
                    _spriteSheet = value;
                    OnSpriteSheetChanged();
                }
            }
        }

        string _spriteSheetName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the <see cref="SpriteSheet"/> used by the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        [ContentSerializer(ElementName = "SpriteSheet"), DisplayName("SpriteSheet"), Description("The name of the sprite sheet used by the game object."), Category("Sprite"), DefaultValue("")]
        public string SpriteSheetName
        {
            get { return _spriteSheetName; }
            set
            {
                if (_spriteSheetName != value)
                {
                    _spriteSheetName = value;
                    LoadSpriteSheet();
                    OnPropertyChanged();
                }
            }
        }

        Rectangle _sourceRectangle = Rectangle.Empty;
        /// <summary>
        /// Gets or sets the source <see cref="Rectangle"/> of the <see cref="BaseSpriteEntity"/>. The source rectangle is a <see cref="Rectangle"/> that specifies (in texture pixels) the source texels from a texture. Use null to draw the entire texture.
        /// </summary>
        [ContentSerializerIgnore, Browsable(false)]
        public Rectangle SourceRectangle
        {
            get { return _sourceRectangle; }
            protected set
            {
                if (_sourceRectangle != value)
                {
                    if (_sourceRectangle.Width != value.Width || _sourceRectangle.Height != value.Height)
                        base.Size = new Vector2(value.Width, value.Height);
                    _sourceRectangle = value;
                }
            }
        }

        SpriteEffects _spriteEffects = SpriteEffects.None;
        /// <summary>
        /// Gets or sets the <see cref="SpriteEffects"/> of the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        [ContentSerializer, Description("The sprite effects of the game object.\nDefault is None."), Category("Sprite"), DefaultValue(SpriteEffects.None), 
#if WINDOWS
        Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))
#endif
        ]
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

        internal int _frame = 0;
        /// <summary>
        /// Gets or sets the frame index of the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        [ContentSerializer, Description("The frame index of the game object.\nDefault is 0."), Category("Sprite"), DefaultValue(0)]
        public int Frame
        {
            get { return _frame; }
            set
            {
                if (_frame != value)
                {
                    _frame = value;
                    UpdateSourceRectangle();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public BaseSpriteEntity()
            : base()
        { 
        }

        /// <summary>
        /// Loads content handled by the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        protected override void LoadContent()
        {
            LoadSpriteSheet();
            base.LoadContent();
        }
        
        /// <summary>
        /// Loads the <see cref="SpriteSheet"/> of the <see cref="BaseSpriteEntity"/>.
        /// </summary>
        protected void LoadSpriteSheet()
        {
            if (!HasInitialized)
                return;
            if (!string.IsNullOrEmpty(_spriteSheetName))
                SpriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
        }

        /// <summary>
        /// Recycles the <see cref="BaseSpriteEntity"/> so it may be re-used.
        /// </summary>
        public override void Recycle()
        {
            base.Recycle();
            _spriteSheet = null;
            _spriteSheetName = string.Empty;
            _sourceRectangle = Rectangle.Empty;
            _spriteEffects = SpriteEffects.None;
            //IsOutlined = false;
            //OutlineColor = Color.White;
        }

#if WINDOWS
        protected internal override bool SetValue(string name, string value)
        {
            switch (name.ToLower())
            {
                case "spritesheet":
                case "spritesheetname":
                    _spriteSheetName = value;
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
            }
            return base.SetValue(name, value);
        }
#endif

        protected abstract void UpdateSourceRectangle();

        /// <summary>
        /// Gets a value indicating whether the <see cref="BaseSpriteEntity"/> is allowed to draw.
        /// </summary>
        /// <returns>A value indicating whether the <see cref="BaseSpriteEntity"/> is allowed to draw.</returns>
        protected override bool CanDraw()
        {
            if (_spriteSheet == null || _spriteSheet.Texture == null)
                return false;
            return base.CanDraw();
        }

        /// <summary>
        /// Draws the <see cref="Entity"/>. Override to add custom draw logic which is executed every frame.
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="spriteBatch">spriteBatch</param>
        protected override void Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.Draw(time, spriteBatch);
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
            spriteBatch.DrawTransformableEntity(this, _spriteSheet.Texture, _sourceRectangle, _spriteEffects, 0);
        }

        /// <summary>
        /// Called when the <see cref="BaseSpriteEntity"/>'s <see cref="SpriteSheet"/> has changed.
        /// </summary>
        protected virtual void OnSpriteSheetChanged()
        {
        }

    }
}
