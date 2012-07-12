using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityBase : IRecyclable, IEntity
    {
        internal EntityCollection _collectionReference = null;
        internal Vector2 _renderPosition = Vector2.Zero;
        internal Rectangle _renderArea = Rectangle.Empty;

        [ContentSerializerIgnore]
        protected internal Rectangle RenderArea { get { return _renderArea; } }
        [ContentSerializerIgnore]
        protected Vector2 RenderPosition { get { return _renderPosition; } }
        [ContentSerializerIgnore]
        protected bool IsLateInitialized { get; private set; }
        [ContentSerializerIgnore]
        protected bool IsUpdating { get; private set; }
        [ContentSerializerIgnore]
        protected bool IsDrawing { get; private set; }
        [ContentSerializer]
        public bool IsVisible { get; set; }
        [ContentSerializer]
        public bool IsEnabled { get; set; }
        [ContentSerializerIgnore]
        public bool NeedsHeavyUpdate { get; set; }
        [ContentSerializerIgnore]
        public bool RemoveOnNextUpdate { get; set; }
        [ContentSerializerIgnore]
        public bool HasLoadedContent { get; internal set; }

        float _majorLayer = 0.0f;
        [ContentSerializer]
        public float MajorLayer
        {
            get { return _majorLayer; }
            set
            {
                if (_majorLayer != value)
                {
                    _majorLayer = value;
                    if (_collectionReference != null)
                        _collectionReference.NeedsToSort = true;
                }
            }
        }

        float _minorLayer = 0.0f;
        [ContentSerializer]
        public float MinorLayer
        {
            get { return _minorLayer; }
            set
            {
                if (_minorLayer != value)
                {
                    _minorLayer = value;
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
            _renderArea = Rectangle.Empty;
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
                    MajorLayer = float.Parse(value, CultureInfo.InvariantCulture);
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
            if (RemoveOnNextUpdate)
            {
                RemoveOnNextUpdate = false;
                Remove();
            }
            if (!IsLateInitialized)
                InternalInitialize();
            if (!HasLoadedContent)
                InternalLoadContent();
            if (CanUpdate())
            {
                BeginUpdate(time);
                LightUpdate(time);
                if (NeedsHeavyUpdate)
                {
                    BeginHeavyUpdate(time);
                    HeavyUpdate(time);
                    EndHeavyUpdate(time);
                }
                EndUpdate(time);
            }
        }

        protected virtual bool CanUpdate()
        {
            if (!IsEnabled || !IsLateInitialized || IsUpdating) return false;
            return true;
        }

        protected virtual void LightUpdate(DeltaTime time)
        {
        }

        protected internal virtual void BeginUpdate(DeltaTime time)
        {
            IsUpdating = true;
        }

        protected internal virtual void EndUpdate(DeltaTime time)
        {
            IsUpdating = false;
        }

        protected internal virtual void BeginHeavyUpdate(DeltaTime time)
        {
            NeedsHeavyUpdate = false;
        }

        protected internal virtual void HeavyUpdate(DeltaTime time)
        {
        }

        protected internal virtual void EndHeavyUpdate(DeltaTime time)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (CanDraw())
            {
                BeginDraw(time, spriteBatch);
                Draw(time, spriteBatch);
                EndDraw(time, spriteBatch);
            }
        }

        protected virtual bool CanDraw()
        {
            if (!IsVisible || !IsLateInitialized || IsDrawing) return false;
            if (RenderArea != Rectangle.Empty && _collectionReference != null)
            {
                if (_collectionReference.ViewingArea != Rectangle.Empty || !_collectionReference.ViewingArea.Contains(RenderArea) || !_collectionReference.ViewingArea.Intersects(RenderArea))
                    return false;
            }
            return true;
        }

        protected virtual void BeginDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            IsDrawing = true;
        }

        protected internal virtual void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
        }

        protected internal virtual void EndDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            IsDrawing = false;
        }

        public void Remove()
        {
            if (_collectionReference == null)
                return;
            _collectionReference.Remove(this);
        }

        public virtual void Recycle()
        {
            _collectionReference = null;
            _renderPosition = Vector2.Zero;
            _renderArea = Rectangle.Empty;
            _majorLayer = 0.0f;
            _minorLayer = 0.0f;
            HasLoadedContent = false;
            IsDrawing = false;
            IsEnabled = true;
            IsLateInitialized = false;
            IsUpdating = false;
            IsVisible = true;
            RemoveOnNextUpdate = false;
        }

    }
}
