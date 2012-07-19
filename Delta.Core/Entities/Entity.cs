using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta.Movement;
using Delta.Structures;
using Delta.Collision;
using Delta.Collision.Geometry;
using Delta.Physics;

namespace Delta
{
    public abstract class Entity : EntityBase, IEntity
    {
        #region TEMP: Transformer
        Transformer _transformer;

        [ContentSerializer]
        public bool _fadeRandom { get; set; } // won't serialize fields? wtf. totally temp.
        TimedRange _fadeRange;
        [ContentSerializer]
        public TimedRange FadeRange
        {
            get { return _fadeRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _fadeRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    if (_fadeRandom)
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
            }
        }

        TimedRange _flickerRange;
        [ContentSerializer]
        public TimedRange FlickerRange
        {
            get { return _flickerRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _flickerRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    _transformer = Transformer.ThisEntity(this).FlickerFor(_flickerRange.Lower, _flickerRange.Upper, _flickerRange.Duration);
                    _transformer.Loop();
                }
            }
        }

        TimedRange _blinkRange;
        [ContentSerializer]
        public TimedRange BlinkRange
        {
            get { return _blinkRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _blinkRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    _transformer = Transformer.ThisEntity(this).BlinkFor(_blinkRange.Lower, _blinkRange.Duration);
                    _transformer.Loop();
                }
            }
        }
        #endregion

        Rectangle _renderArea = Rectangle.Empty;

        [ContentSerializerIgnore]
        protected Vector2 RenderPosition { get; private set; }
        [ContentSerializerIgnore]
        protected Vector2 RenderOrigin { get; private set; }
        [ContentSerializerIgnore]
        protected float RenderRotation { get; private set; }
        [ContentSerializerIgnore]
        protected Vector2 RenderSize { get; private set; }
        [ContentSerializerIgnore]
        protected Rectangle RenderArea { get { return _renderArea; } }

        Vector2 _position = Vector2.Zero;
        [ContentSerializer]
        public virtual Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPositionChanged();
                }
            }
        }

        Vector2 _offset = Vector2.Zero;
        [ContentSerializer]
        public virtual Vector2 Offset
        {
            get { return _offset; }
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    OnPositionChanged();
                }
            }
        }

        Vector2 _size = Vector2.Zero;
        [ContentSerializer]
        public virtual Vector2 Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnSizeChanged();
                }
            }
        }

        Vector2 _scale = Vector2.One;
        [ContentSerializer]
        public virtual Vector2 Scale
        {
            get { return _scale; }
            set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnScaleChanged();
                }
            }
        }

        float _rotation = 0.0f;
        [ContentSerializer]
        public virtual float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    OnRotationChanged();
                }
            }
        }

        Vector2 _origin = Vector2.Zero;
        [ContentSerializer]
        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                if (_origin != value)
                {
                    _origin = value.Clamp(Vector2.Zero, Vector2.One);
                    OnOriginChanged();
                }
            }
        }

        Vector2 _pivot = Vector2.One * 0.5f;
        [ContentSerializer]
        public Vector2 Pivot
        {
            get { return _pivot; }
            set
            {
                if (_pivot != value)
                {
                    _pivot = value.Clamp(Vector2.Zero, Vector2.One);
                    OnPivotChanged();
                }
            }
        }

        [ContentSerializer(ElementName = "Tint")]
        Color _tint = Color.White;
        [ContentSerializerIgnore]
        public virtual Color Tint
        {
            get { return _tint * _alpha; }
            set
            {
                if (_tint != value)
                {
                    _tint = value;
                    OnTintChanged();
                }
            }
        }

        float _alpha = 1.0f;
        [ContentSerializer]
        public virtual float Alpha
        {
            get { return _alpha; }
            set
            {
                value = value.Clamp(0.0f, 1.0f);
                if (_alpha != value)
                {
                    _alpha = value;
                    OnAlphaChanged();
                }
            }
        }

        IWrappedBody _wrappedBody;
        [ContentSerializerIgnore]
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

        public Entity()
            : base()
        {
            Name = string.Empty;
            NeedsHeavyUpdate = true;
            RenderOrigin = Vector2.Zero;
            RenderRotation = 0.0f;
        }

        public Entity(string name)
            : this()
        {
            Name = name;
        }

#if WINDOWS
        protected internal override bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "pos":
                case "position":
                    Position = Vector2Extensions.Parse(value);
                    return true;
                case "offset":
                    Offset = Vector2Extensions.Parse(value);
                    return true;
                case "scale":
                    Scale = Vector2Extensions.Parse(value);
                    return true;
                case "rot":
                case "rotation":
                    Rotation = float.Parse(value, CultureInfo.InvariantCulture).ToRadians();
                    return true;
                case "tint":
                case "color":
                    Tint = value.ToColor();
                    return true;
                case "opacity":
                case "alpha":
                    Alpha = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
                case "fade":
                    _fadeRange = TimedRange.Parse(value);
                    return true;
                case "faderandom":
                    _fadeRange = TimedRange.Parse(value);
                    _fadeRandom = true;
                    return true;
                case "flicker":
                    _flickerRange = TimedRange.Parse(value);
                    return true;
                case "blink":
                    _blinkRange = TimedRange.Parse(value);
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }
#endif

        protected virtual void UpdateRenderSize()
        {
            RenderSize = Size * Scale;
        }

        protected virtual void UpdateRenderPosition()
        {
            RenderPosition = Position + Offset + RenderOrigin - (Origin * RenderSize);
        }

        protected virtual void UpdateRenderOrigin()
        {
            RenderOrigin = Pivot * RenderSize;
        }

        protected virtual void UpdateRenderRotation()
        {
            RenderRotation = Rotation.ToRadians();
        }

        protected virtual void UpdateRenderArea()
        {
            _renderArea.X = (int)(Position.X + Offset.X);
            _renderArea.Y = (int)(Position.Y + Offset.Y);
            _renderArea.Width = (int)RenderSize.X;
            _renderArea.Height = (int)RenderSize.Y;
        }

        protected virtual void UpdateToWrappedBody()
        {
            if (WrappedBody != null)
            {
                WrappedBody.SimulationPosition = Position + Offset;
                WrappedBody.SimulationRotation = RenderRotation;
            }
        }

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

        protected internal virtual void OnPositionChanged()
        {
            UpdateRenderPosition();
            UpdateToWrappedBody();
            UpdateRenderArea();
        }

        protected internal virtual void OnSizeChanged()
        {
            UpdateRenderSize();
            UpdateRenderOrigin();
            UpdateRenderPosition();
            UpdateRenderArea();
        }

        protected internal virtual void OnScaleChanged()
        {
            UpdateRenderSize();
            UpdateRenderOrigin();
            UpdateRenderPosition();
            UpdateRenderArea();
        }

        protected internal virtual void OnRotationChanged()
        {
            UpdateRenderRotation();
            UpdateToWrappedBody();
        }

        protected virtual void OnOriginChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected virtual void OnPivotChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected virtual void OnTintChanged()
        {
        }

        protected virtual void OnAlphaChanged()
        {
        }

        protected virtual void OnWrappedBodyChanged()
        {
        }

        public override void Recycle()
        {
            base.Recycle();
            Name = string.Empty;
            _renderArea = Rectangle.Empty;
            RenderPosition = Vector2.Zero;
            RenderOrigin = Vector2.Zero;
            RenderRotation = 0.0f;
            RenderSize = Vector2.Zero;
            _alpha = 1.0f;
            _offset = Vector2.Zero;
            _origin = Vector2.Zero;
            _pivot = Vector2.One * 0.5f;
            _position = Vector2.Zero;
            _rotation = 0.0f;
            _scale = Vector2.One;
            _size = Vector2.Zero;
            _tint = Color.White;
            _wrappedBody = null;
        }

    }
}
