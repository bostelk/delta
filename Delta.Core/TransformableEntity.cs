using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    public class TransformableEntity : Entity
    {

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

        Color _tint = Color.White;
        [ContentSerializer]
        public virtual Color Tint
        {
            get { return _tint; }
            set
            {
                if (_tint != value)
                {
                    _tint = value;
                    _tint *= _alpha;
                }
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
                {
                    _alpha = value;
                    _tint *= _alpha;
                }
            }
        }

        public TransformableEntity()
            : base()
        {
            Tint = Color.White;
            Scale = Vector2.One;
        }

        protected internal virtual void OnPositionChanged()
        {
        }

        protected internal virtual void OnSizeChanged()
        {
        }

        protected internal virtual void OnScaleChanged()
        {
        }

        protected internal virtual void OnRotationChanged()
        {
        }

    }
}
