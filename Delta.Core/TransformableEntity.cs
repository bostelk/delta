using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    public class TransformableEntity : Entity
    {
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
                case "offset":
                    Position += Vector2Extensions.Parse(value);
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
            RenderPosition = Position + RenderOrigin - (Origin * Size * Scale);
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
