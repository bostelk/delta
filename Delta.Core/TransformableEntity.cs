using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta.Transformations;
using Delta.Structures;
using Delta.Collision;

namespace Delta
{
    /// <summary>
    /// Base class for all transformable game entities.
    /// </summary>
    public abstract class TransformableEntity : Entity
    {
        Transformer _transformer = null;

        Vector2 _position = Vector2.Zero;
        /// <summary>
        /// Gets or sets the position of the <see cref="TransformableEntity"/>.
        /// </summary>
        /// <remarks>The default is zero with a <see cref="Vector2"/> value of {0,0}.</remarks>
        [ContentSerializer, Description("The position of the game object.\nDefault is (0, 0)."), Category("Transform"), Browsable(true), ReadOnly(false), DefaultValue(typeof(Vector2), "0,0")]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPositionChanged();
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _offset = Vector2.Zero;
        /// <summary>
        /// Gets or sets the positional offset of the <see cref="TransformableEntity"/>.
        /// </summary>
        /// <remarks>The default is zero with a <see cref="Vector2"/> value of {0,0}.</remarks>
        [ContentSerializer, Description("The positional offset of the game object.\nDefault is (0, 0)."), Category("Transform"), Browsable(true), ReadOnly(false), DefaultValue(typeof(Vector2), "0,0")]
        public Vector2 Offset
        {
            get { return _offset; }
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    OnPositionChanged();
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _absolutePosition = Vector2.Zero;
        /// <summary>
        /// Gets the position of the <see cref="TransformableEntity"/> with respect it's <see cref="Parent"/>.
        /// </summary>
        /// <remarks>The default is zero with a <see cref="Vector2"/> value of {0,0}.</remarks>
        [ContentSerializerIgnore, Description("The absolute position of the game object.\nDefault is (0, 0)."), Category("Render & Absolute"), ReadOnly(true)]
        public Vector2 AbsolutePosition
        {
            get { return _absolutePosition; }
            internal set
            {
                if (_absolutePosition != value)
                {
                    _absolutePosition = value;
                    OnPositionChanged();
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _renderPosition = Vector2.Zero;
        /// <summary>
        /// Gets the position used when rendering the <see cref="TransformableEntity"/>.
        /// </summary>
        /// <remarks>To move this <see cref="TransformableEntity"/> see <see cref="Position"/> and <see cref="Offset"/></remarks>
        [ContentSerializerIgnore, Description("The final render position of the game object."), Category("Render & Absolute"), Browsable(true), ReadOnly(true)]
        public Vector2 RenderPosition
        {
            get { return _renderPosition; }
            protected set
            {
                if (_renderPosition != value)
                {
                    _renderPosition = value;
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _size = Vector2.Zero;
        /// <summary>
        /// Gets or sets the size of the <see cref="TransformableEntity"/> expressed in pixels <b>before it's scaled</b>.
        /// </summary>
        /// <remarks>The default is zero with a <see cref="Vector2"/> value of {0,0}.</remarks>
        [ContentSerializer, Description("The size of the game object expressed in pixels before it's scaled.\nDefault is (0, 0)."), Category("Transform"), DefaultValue(typeof(Vector2), "0,0")]
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged();
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _renderSize = Vector2.Zero;
        /// <summary>
        /// Gets the size used when rendering the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Description("The final render size of the game object expressed in pixels."), Category("Render & Absolute"), Browsable(true), ReadOnly(true)]
        public Vector2 RenderSize
        {
            get { return _renderSize; }
            protected set
            {
                if (_renderSize != value)
                {
                    _renderSize = value;
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _scale = Vector2.One;
        /// <summary>
        /// Gets or sets the scale of the <see cref="TransformableEntity"/> expressed in decimal percentage.
        /// </summary>
        /// <remarks>The default is 100% with a <see cref="Vector2"/> value of {1.0f,1.0f}.</remarks>
        [ContentSerializer, Description("The scale of the game object expressed in decimal percentage.\nDefault is (1, 1)."), Category("Transform"), DefaultValue(typeof(Vector2), "1,1")]
        public Vector2 Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnScaleChanged();
                    OnPropertyChanged();
                }
            }
        }

        float _rotation = 0.0f;
        /// <summary>
        /// Gets or sets the rotation of the <see cref="TransformableEntity"/> expressed in degrees.
        /// </summary>
        /// <remarks>The default is zero with a <see cref="float"/> value of 0.0f. <b>Positive</b> values result in a <b>clockwise</b> rotation. <b>Negative</b> values result in a <b>counter-clockwise</b> rotation.</remarks>
        [ContentSerializer, Description("The rotation of the game object expressed in degrees.\nDefault is 0."), Category("Transform"), DefaultValue(0.0f)]
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    OnRotationChanged();
                    OnPropertyChanged();
                }
            }
        }

        float _absoluteRotation = 0.0f;
        /// <summary>
        /// Gets the rotation of the <see cref="TransformableEntity"/> expressed in degrees with respect to it's <see cref="Parent"/>.
        /// </summary>
        [ContentSerializerIgnore, Description("The absolute rotation of the game object expressed in degrees."), Category("Render & Absolute"), ReadOnly(true)]
        public float AbsoluteRotation
        {
            get { return _absoluteRotation; }
            internal set
            {
                if (_absoluteRotation != value)
                {
                    _absoluteRotation = value;
                    OnRotationChanged();
                    OnPropertyChanged();
                }
            }
        }

        float _renderRotation = 0.0f;
        /// <summary>
        /// Gets the rotation used when rendering the <see cref="TransformableEntity"/> expressed in radians.
        /// </summary>
        [ContentSerializerIgnore, Description("The final render rotation of the game object expressed in radians."), Category("Render & Absolute"), Browsable(true), ReadOnly(true)]
        public float RenderRotation
        {
            get { return _renderRotation; }
            protected set
            {
                if (_renderRotation != value)
                {
                    _renderRotation = value;
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _origin = Vector2.Zero;
        /// <summary>
        /// Gets or sets the positional origin of the <see cref="TransformableEntity"/> expressed in decimal percetange relative to the <see cref="RenderSize"/>.
        /// </summary>
        /// <remarks>The default is 0% (the top left) with a <see cref="Vector2"/> value of {0.0f, 0.0f}.</remarks>
        [ContentSerializer, Description("The positional origin of the game object expressed in decimal percentage.\nDefault is (0, 0)."), Category("Transform"), DefaultValue(typeof(Vector2), "0,0")]
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                if (_origin != value)
                {
                    _origin = value.Clamp(Vector2.Zero, Vector2.One);
                    OnOriginChanged();
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _renderOrigin = Vector2.Zero;
        /// <summary>
        /// Gets the origin used when rendering the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Description("The final render origin of the game object."), Category("Render & Absolute"), Browsable(true), ReadOnly(true)]
        public Vector2 RenderOrigin
        {
            get { return _renderOrigin; }
            protected set
            {
                if (_renderOrigin != value)
                {
                    _renderOrigin = value;
                    OnPropertyChanged();
                }
            }
        }

        Vector2 _pivot = Vector2.One * 0.5f;
        /// <summary>
        /// Gets or sets the positional pivot of the <see cref="TransformableEntity"/> expressed in decimal percentange relative to the <see cref="RenderSize"/>.
        /// </summary>
        /// <remarks>The default is 50% (the center) with a <see cref="Vector2"/> value of {0.5f, 0.5f}.</remarks>
        [ContentSerializer, Description("The positional pivot of the game object expressed in decimal percentage.\nDefault is (0.5, 0.5)."), Category("Transform"), DefaultValue(typeof(Vector2), "0.5,0.5")]
        public Vector2 Pivot
        {
            get { return _pivot; }
            set
            {
                if (_pivot != value)
                {
                    _pivot = value.Clamp(Vector2.Zero, Vector2.One);
                    OnPivotChanged();
                    OnPropertyChanged();
                }
            }
        }

        Color _tint = Color.White;
        /// <summary>
        /// Gets or sets the color of the <see cref="TransformableEntity"/> <b>before it's multipled with <see cref="Alpha"/></b>.
        /// </summary>
        /// <remarks>The default is white with a <see cref="Color"/> value of {255, 255, 255, 255}.</remarks>
        [ContentSerializer, Description("The tint color of the game object.\nDefault is (255, 255, 255, 255)."), Category("Transform"), DefaultValue(typeof(Color), "255,255,255,255")]
        public virtual Color Tint
        {
            get { return _tint; }
            set
            {
                if (_tint != value)
                {
                    _tint = value;
                    OnTintChanged();
                    OnPropertyChanged();
                }
            }
        }

        Color _renderColor = Color.White;
        /// <summary>
        /// Gets the premultipled color used when rendering the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Description("The final render color of the game object."), Category("Render & Absolute"), Browsable(true), ReadOnly(true)]
        public Color RenderColor
        {
            get { return _renderColor; }
            protected set
            {
                if (_renderColor != value)
                {
                    _renderColor = value;
                    OnPropertyChanged();
                }
            }
        }

        float _alpha = 1.0f;
        /// <summary>
        /// Gets or sets the alpha component of the <see cref="TransformableEntity"/> expressed in decimal percentage. Used to premultiple the <see cref="Tint"/>.
        /// </summary>
        /// <remarks>The default is 100% with a <see cref="float"/> value of 1.0f.</remarks>
        [ContentSerializer, Description("The alpha component of the game object expressed in decimal percentage.\nDefault is 1."), Category("Transform"), DefaultValue(1.0f)]
        public virtual float Alpha
        {
            get { return _alpha; }
            set
            {
                if (_alpha != value)
                {
                    _alpha = value.Clamp(0.0f, 1.0f);
                    OnAlphaChanged();
                    OnPropertyChanged();
                }
            }
        }

        float _absoluteAlpha = 1.0f;
        /// <summary>
        /// Gets or sets the alpha component of the <see cref="TransformableEntity"/> expressed in decimal percentage with respect to it's <see cref="Parent"/>. Used to premultiple the <see cref="Tint"/>.
        /// </summary>
        [ContentSerializerIgnore, Description("The absolute alpha component of the game object expressed in decimal percentage."), Category("Render & Absolute"), ReadOnly(true)]
        public virtual float AbsoluteAlpha
        {
            get { return _absoluteAlpha; }
            set
            {
                value = value.Clamp(0.0f, 1.0f);
                if (_absoluteAlpha != value)
                {
                    _absoluteAlpha = value;
                    OnAlphaChanged();
                    OnPropertyChanged();
                }
            }
        }

        IWrappedBody _wrappedBody = null;
        /// <summary>
        /// Gets or sets the <see cref="IWrappedBody"/> of the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializerIgnore, Browsable(false)]
        public IWrappedBody WrappedBody
        {
            get { return _wrappedBody; }
            set
            {
                if (value != null)
                {
                    _wrappedBody = value;
                    OnWrappedBodyChanged();
                }
            }
        }

        bool _fadeRandomly = false;
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="TransformableEntity"/> fades randomly.
        /// </summary>
        [ContentSerializer, Description("Indicates whether the game object fades in/out randomly.\nDefault is 0."), Category("Transform"), DefaultValue(false)]
        public bool FadeRandomly
        {
            get { return _fadeRandomly; }
            set
            {
                if (_fadeRandomly != value)
                {
                    _fadeRandomly = value;
                    OnPropertyChanged();
                }
            }
        }

        TimedRange _fadeRange = TimedRange.Empty;
        /// <summary>
        /// Gets or sets the fade timed range of the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializer, Description("The fade timed range of the game object.\nDefault is (0, 0, 0)."), Category("Transform"), DefaultValue(typeof(TimedRange), "0,0,0")]
        public TimedRange FadeRange
        {
            get { return _fadeRange; }
            set
            {
                if (_fadeRange != value)
                {
                    _fadeRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    if (!value.IsEmpty())
                    {
                        if (FadeRandomly)
                        {
                            // start the transformer off in a random position within the range.
                            float startupAlpha = G.Random.Between(_fadeRange.Lower, _fadeRange.Upper);
                            _transformer = Transformer.ThisEntity(this).FadeTo(startupAlpha, (startupAlpha / _fadeRange.Upper) * _fadeRange.Duration);
                            _transformer.OnTransformFinished(() =>
                            {
                                // remove the start-up transform logic.
                                _transformer.ClearSequence();
                                _transformer.OnTransformFinished(null);
                                // 50/50 chance to fade from lower to upper or from upper to lower. provides more fade varieties.
                                if (G.Random.FiftyFifty())
                                    _transformer.FadeTo(_fadeRange.Upper, _fadeRange.Duration).FadeTo(_fadeRange.Lower, _fadeRange.Duration);
                                else
                                    _transformer.FadeTo(_fadeRange.Lower, _fadeRange.Duration).FadeTo(_fadeRange.Upper, _fadeRange.Duration);
                                _transformer.Loop();
                            });
                        }
                        else
                        {
                            _transformer = Transformer.ThisEntity(this).FadeTo(_fadeRange.Lower, _fadeRange.Duration).FadeTo(_fadeRange.Upper, _fadeRange.Duration);
                            _transformer.Loop();
                        }
                    }
                    else
                        _transformer = null;
                    OnPropertyChanged();
                }
            }
        }

        TimedRange _flickerRange = TimedRange.Empty;
        /// <summary>
        /// Gets or sets the flicker range of the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializer, Description("The flicker timed range of the game object.\nDefault is (0, 0, 0)."), Category("Transform"), DefaultValue(typeof(TimedRange), "0,0,0")]
        public TimedRange FlickerRange
        {
            get { return _flickerRange; }
            set
            {
                if (_flickerRange != value)
                {
                    _flickerRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    if (!value.IsEmpty())
                    {
                        _transformer = Transformer.ThisEntity(this).FlickerFor(_flickerRange.Lower, _flickerRange.Upper, _flickerRange.Duration);
                        _transformer.Loop();
                    }
                    else
                        _transformer = null;
                    OnPropertyChanged();
                }
            }
        }

        TimedRange _blinkRange = TimedRange.Empty;
        /// <summary>
        /// Gets or sets the blink range of the <see cref="TransformableEntity"/>.
        /// </summary>
        [ContentSerializer, Description("The blink timed range of the game object.\nDefault is (0, 0, 0)."), Category("Transform"), DefaultValue(typeof(TimedRange), "0,0,0")]
        public TimedRange BlinkRange
        {
            get { return _blinkRange; }
            set
            {
                if (_blinkRange != value)
                {
                    _blinkRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    if (!value.IsEmpty())
                    {
                        _transformer = Transformer.ThisEntity(this).BlinkFor(_blinkRange.Lower, _blinkRange.Duration);
                        _transformer.Loop();
                    }
                    else
                        _transformer = null;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public TransformableEntity()
            : base()
        {
            RenderColor = Color.White;
        }

        /// <summary>
        /// Recycles the <see cref="TransformableEntity"/> so it may be re-used.
        /// </summary>
        public override void Recycle()
        {
            base.Recycle();
            _position = Vector2.Zero;
            _offset = Vector2.Zero;
            _absolutePosition = Vector2.Zero;
            _origin = Vector2.Zero;
            _pivot = Vector2.One * 0.5f;
            _rotation = 0.0f;
            _absoluteRotation = 0.0f;
            _scale = Vector2.One;
            _size = Vector2.Zero;
            _tint = Color.White;
            _alpha = 1.0f;
            _absoluteAlpha = 1.0f;
            _wrappedBody = null;
            RenderPosition = Vector2.Zero;
            RenderOrigin = Vector2.Zero;
            RenderRotation = 0.0f;
            RenderSize = Vector2.Zero;
            RenderColor = Color.White;
        }

#if WINDOWS
        protected internal override bool SetValue(string name, string value)
        {
            switch (name)
            {
                case "pos":
                case "position":
                    Position = (Vector2)TypeDescriptor.GetConverter(typeof(Vector2)).ConvertFromString(value);
                    return true;
                case "offset":
                    Offset = (Vector2)TypeDescriptor.GetConverter(typeof(Vector2)).ConvertFromString(value);
                    return true;
                case "scale":
                    Scale = (Vector2)TypeDescriptor.GetConverter(typeof(Vector2)).ConvertFromString(value);
                    return true;
                case "rot":
                case "rotation":
                    Rotation = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "tint":
                case "color":
                    Tint = value.ToColor();
                    return true;
                case "opacity":
                case "alpha":
                case "a":
                    Alpha = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "fade":
                    _fadeRange = TimedRange.Parse(value);
                    return true;
                case "faderandom":
                    _fadeRange = TimedRange.Parse(value);
                    FadeRandomly = true;
                    return true;
                case "flicker":
                    _flickerRange = TimedRange.Parse(value);
                    return true;
                case "blink":
                    _blinkRange = TimedRange.Parse(value);
                    return true;
            }
            return base.SetValue(name, value);
        }
#endif

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="AbsolutePosition"/>.
        /// </summary>
        protected virtual void UpdateAbsolutePosition()
        {
            _absolutePosition = _position;
            if (Parent != null)
            {
                TransformableEntity transformable = Parent as TransformableEntity;
                if (transformable != null)
                    _absolutePosition += transformable.AbsolutePosition;
            }
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="AbsoluteRotation"/>.
        /// </summary>
        protected virtual void UpdateAbsoluteRotation()
        {
            _absoluteRotation = _rotation;
            if (Parent != null)
            {
                TransformableEntity transformable = Parent as TransformableEntity;
                if (transformable != null)
                    _absoluteRotation += transformable.AbsoluteRotation;
            }
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="AbsoluteAlpha"/>.
        /// </summary>
        protected virtual void UpdateAbsoluteAlpha()
        {
            _absoluteAlpha = _alpha;
            if (Parent != null)
            {
                TransformableEntity transformable = Parent as TransformableEntity;
                if (transformable != null)
                    _absoluteAlpha *= transformable.AbsoluteAlpha;
            }
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="RenderPosition"/>.
        /// </summary>
        protected virtual void UpdateRenderPosition()
        {
            RenderPosition = _absolutePosition + _offset
                - (Origin * RenderSize) //Offset by the Origin.
                + (RenderOrigin * Scale) //Offset by the RenderOrigin (pivot) in the opposite direction to return to the original position.
                ;
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="RenderSize"/>.
        /// </summary>
        protected virtual void UpdateRenderSize()
        {
            RenderSize = Size * Scale;
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="RenderOrigin"/>.
        /// </summary>
        protected virtual void UpdateRenderOrigin()
        {
            RenderOrigin = Pivot * Size; //We don't want to multiply the scale in here because SpriteBatch automatically does that for us when drawing.
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/>'s <see cref="RenderRotation"/>.
        /// </summary>
        protected virtual void UpdateRenderRotation()
        {
            RenderRotation = AbsoluteRotation.ToRadians();
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/> to the <see cref="WrappedBody"/>.
        /// </summary>
        protected virtual void UpdateToWrappedBody()
        {
            if (WrappedBody != null)
            {
                WrappedBody.SimulationPosition = Position + Offset;
                WrappedBody.SimulationRotation = RenderRotation;
            }
        }

        /// <summary>
        /// Updates the <see cref="TransformableEntity"/> from the <see cref="WrappedBody"/>.
        /// </summary>
        protected virtual void UpdateFromWrappedBody()
        {
            if (WrappedBody != null)
            {
                //don't fire OnChanged functions! We don't want to get into a fight between reading and writing off the WrappedBody!
                _position = WrappedBody.SimulationPosition - Offset;
                _rotation = WrappedBody.SimulationRotation.ToDegrees();
                UpdateRenderPosition();
                UpdateRenderRotation();
            }
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Position"/>, or <see cref="Offset"/> or <see cref="AbsolutePosition"/> has changed.
        /// </summary>
        protected internal virtual void OnPositionChanged()
        {
            UpdateAbsolutePosition();
            UpdateRenderSize();
            UpdateRenderOrigin();
            UpdateRenderPosition();
            UpdateToWrappedBody();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Size"/> has changed.
        /// </summary>
        protected internal virtual void OnSizeChanged()
        {
            UpdateRenderSize();
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Scale"/> has changed.
        /// </summary>
        protected internal virtual void OnScaleChanged()
        {
            UpdateRenderSize();
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Rotation"/> has changed.
        /// </summary>
        protected internal virtual void OnRotationChanged()
        {
            UpdateAbsoluteRotation();
            UpdateRenderRotation();
            UpdateToWrappedBody();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Origin"/> has changed.
        /// </summary>
        protected virtual void OnOriginChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Pivot"/> has changed.
        /// </summary>
        protected virtual void OnPivotChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Tint"/> has changed.
        /// </summary>
        protected virtual void OnTintChanged()
        {
            UpdateAbsoluteAlpha();
            RenderColor = Tint * AbsoluteAlpha;
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Alpha"/> has changed.
        /// </summary>
        protected virtual void OnAlphaChanged()
        {
            UpdateAbsoluteAlpha();
            RenderColor = Tint * AbsoluteAlpha;
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="WrappedBody"/> has changed.
        /// </summary>
        protected virtual void OnWrappedBodyChanged()
        {
            WrappedBody.AddToSimulation();
            WrappedBody.Owner = this;
            UpdateToWrappedBody();
        }

        /// <summary>
        /// Called when the <see cref="Entity"/> has been removed from an <see cref="IEntityCollection"/>.
        /// </summary>
        protected internal override void OnRemoved()
        {
            if (WrappedBody != null)
                WrappedBody.RemoveFromSimulation();
            base.OnRemoved();
        }

        /// <summary>
        /// Called when the <see cref="TransformableEntity"/>'s <see cref="Parent"/> has changed.
        /// </summary>
        protected internal override void OnParentChanged()
        {
            base.OnParentChanged();
            UpdateAbsolutePosition();
            UpdateRenderPosition();
            UpdateAbsoluteRotation();
            UpdateRenderRotation();
        }
    }
}
