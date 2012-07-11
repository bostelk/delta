using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Delta.Movement;
using Delta.Structures;

namespace Delta
{
    public class TransformableEntity : Entity
    {
        #region TEMP: Transformer
        Transformer _transformer;

        [ContentSerializer]
        public bool _fadeRandom { get; set; } // won't serialize fields? wtf. totally temp.
        OverRange _fadeRange;
        [ContentSerializer]
        public OverRange FadeRange
        {
            get { return _fadeRange; }
            set
            {
                if (!value.IsEmpty())
                {
                    _fadeRange = value;
                    if (_transformer != null)
                        _transformer.ClearSequence();
                    if (_fadeRandom) {
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
                    } else {
                        _transformer = Transformer.ThisEntity(this).FadeTo(_fadeRange.Lower, _fadeRange.Duration).FadeTo(_fadeRange.Upper, _fadeRange.Duration);
                        _transformer.Loop();
                    }
                }
            }
        }

        OverRange _flickerRange;
        [ContentSerializer]
        public OverRange FlickerRange
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

        OverRange _blinkRange;
        [ContentSerializer]
        public OverRange BlinkRange
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

        protected Vector2 RenderPosition { get; private set; }
        protected Vector2 RenderOrigin { get; private set; }

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

        //thanks to Rob, we need this. If I had my way I wouldn't have this.
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

        Vector2 _scale = Vector2.Zero;
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

        Vector2 _pivot = new Vector2(0.5f, 0.5f); //automatically set the pivot as the center
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
        [ContentSerializerIgnore] //we don't want to save the pre-multipled tint!
        public virtual Color Tint
        {
            get { return _tint * _alpha; }
            set
            {
                if (_tint != value)
                    _tint = value;
            }
        }

        float _alpha = 1f;
        [ContentSerializer(ElementName = "Opacity")]
        public virtual float Alpha
        {
            get { return _alpha; }
            set
            {
                if (_alpha != value)
                    _alpha = value.Clamp(0.0f, 1.0f);
            }
        }

        public TransformableEntity()
            : base()
        {
            Tint = Color.White;
            Scale = Vector2.One;
        }

#if WINDOWS
        protected override bool ImportCustomValues(string name, string value)
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
                    _fadeRange = OverRange.Parse(value);
                    return true;
                case "faderandom":
                    _fadeRange = OverRange.Parse(value);
                    _fadeRandom = true;
                    return true;
                case "flicker":
                    _flickerRange = OverRange.Parse(value);
                    return true;
                case "blink":
                    _blinkRange = OverRange.Parse(value);
                    return true;
            }
            return base.ImportCustomValues(name, value);
        }

#endif

        protected virtual void UpdateRenderOrigin()
        {
            RenderOrigin = Pivot * Size * Scale;
        }

        protected virtual void UpdateRenderPosition()
        {
            RenderPosition = Position + Offset + RenderOrigin - (Origin * Size * Scale);
        }

        protected internal virtual void OnPositionChanged()
        {
            UpdateRenderPosition();
        }

        protected internal virtual void OnSizeChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected internal virtual void OnScaleChanged()
        {
            UpdateRenderOrigin();
            UpdateRenderPosition();
        }

        protected internal virtual void OnRotationChanged()
        {
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

    }
}
