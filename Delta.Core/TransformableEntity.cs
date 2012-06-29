using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    public class TransformableEntity : Entity
    {

        Vector2 _position = Vector2.Zero;
        [ContentSerializer]
        public Vector2 Position
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
        public Vector2 Size
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
        public Vector2 Scale
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
        public float Rotation
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

        [ContentSerializer]
        public virtual Color Tint { get; set; }

        public TransformableEntity()
            : base()
        {
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
