using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityBase : IRecyclable, IEntity //for Tiles so they can be part of the giant layer train too!
    {
        internal EntityCollection _collectionReference = null;
        internal Vector2 _renderPosition = Vector2.Zero;

        [ContentSerializerIgnore]
        protected Vector2 RenderPosition { get { return _renderPosition; } }

        float _layer = 0.0f;
        [ContentSerializer]
        public float Layer
        {
            get { return _layer; }
            set
            {
                if (_layer != value)
                {
                    _layer = value;
                    if (_collectionReference != null)
                        _collectionReference.NeedsToSort = true;
                }
            }
        }

        public virtual void Recycle()
        {
            _collectionReference = null;
            _renderPosition = Vector2.Zero;
            _layer = 0.0f;
        }

#if WINDOWS

        bool IEntity.ImportCustomValues(string name, string value)
        {
            return ImportCustomValues(name, value);
        }

        protected virtual bool ImportCustomValues(string name, string value)
        {
            return false;
        }
#endif

        public virtual void LoadContent()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalUpdate(DeltaTime deltaTime)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalDraw(DeltaTime deltaTime, SpriteBatch spriteBatch)
        {
        }

        public void Remove()
        {
            if (_collectionReference == null)
                return;
            _collectionReference.Remove(this);
        }


    }
}
