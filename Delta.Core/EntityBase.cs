using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityBase : IRecyclable, IEntity, IDrawable, IUpdateable
    {
        internal EntityCollection _collectionReference = null;
        //internal Rectangle _renderArea = Rectangle.Empty;

        //[ContentSerializerIgnore]
        //protected internal Rectangle RenderArea { get { return _renderArea; } }
        [ContentSerializerIgnore]
        protected bool IsLateInitialized { get; private set; }
        [ContentSerializer]
        public bool IsVisible { get; set; }
        [ContentSerializer]
        public bool IsEnabled { get; set; }
        [ContentSerializerIgnore]
        public bool NeedsHeavyUpdate { get; set; }
        [ContentSerializerIgnore]
        public bool HasLoadedContent { get; internal set; }

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

        public EntityBase()
            : base()
        {
            IsVisible = true;
            IsEnabled = true;
            //_renderArea = Rectangle.Empty;
        }

#if WINDOWS
        bool IEntity.ImportCustomValues(string name, string value)
        {
            return ImportCustomValues(name, value);
        }

        protected internal virtual bool ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "visible":
                case "isvisible":
                    IsVisible = bool.Parse(value);
                    return true;
                case "enabled":
                case "isenabled":
                    IsEnabled = bool.Parse(value);
                    return true;
                case "layer":
                case "order":
                case "draworder":
                case "updateorder":
                    Layer = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
             }
            return false;
        }
#endif

        internal void InternalInitialize()
        {
            if (!IsLateInitialized)
            {
                if (G.GraphicsDevice == null)
                    return;
                IsLateInitialized = true;
                LateInitialize();
            }
        }

        protected virtual void LateInitialize()
        {
        }

        internal void InternalLoadContent()
        {
            if (!HasLoadedContent)
            {
                HasLoadedContent = true;
                LoadContent();
            }
        }

        public virtual void LoadContent()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalUpdate(DeltaTime time)
        {
            if (!IsLateInitialized)
                InternalInitialize();
            if (!HasLoadedContent)
                InternalLoadContent();
            if (CanUpdate())
            {
                LightUpdate(time);
                if (NeedsHeavyUpdate)
                {
                    NeedsHeavyUpdate = false;
                    HeavyUpdate(time);
                }
            }
        }

        protected virtual bool CanUpdate()
        {
            if (!IsEnabled) return false;
            return true;
        }

        protected virtual void LightUpdate(DeltaTime time)
        {
        }

        protected internal virtual void HeavyUpdate(DeltaTime time)
        {
            NeedsHeavyUpdate = false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (CanDraw())
                Draw(time, spriteBatch);
        }

        protected virtual bool CanDraw()
        {
            if (!IsVisible) 
                return false;
            //if (RenderArea == Rectangle.Empty || _collectionReference == null || _collectionReference.ViewingArea == Rectangle.Empty)
            //    return true;
            //if (_collectionReference.ViewingArea.Contains(RenderArea) || _collectionReference.ViewingArea.Intersects(RenderArea))
            //    return true;
            return true;
        }

        protected internal virtual void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
        }

        public virtual void Remove()
        {
            if (_collectionReference == null)
                return;
            _collectionReference.Remove(this);
        }

        public virtual void Recycle()
        {
            _collectionReference = null;
            //_renderArea = Rectangle.Empty;
            _layer = 0.0f;
            HasLoadedContent = false;
            IsEnabled = true;
            IsLateInitialized = false;
            IsVisible = true;
        }

    }
}
