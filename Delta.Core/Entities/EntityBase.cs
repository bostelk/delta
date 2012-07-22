using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    /// <summary>
    /// Base class for all Delta game components.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityBase : IRecyclable, IImportable, IEntity, IDisposable
    {
        public static IEntity Get(string id)
        {
            id = id.ToLower();
            if (EntityHelper._idReferences.ContainsKey(id))
                return EntityHelper._idReferences[id];
            return null;
        }

        IEntityCollection _collection = null;

        [ContentSerializerIgnore]
        protected internal IEntityCollection Collection { get { return _collection; } }

        [ContentSerializerIgnore]
        IEntityCollection IEntity.Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }

        [ContentSerializer]
        public string Name { get; internal set; }

        string IEntity.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        [ContentSerializerIgnore]
        protected bool IsLateInitialized { get; private set; }
        [ContentSerializerIgnore]
        protected bool NeedsHeavyUpdate { get; set; }
        [ContentSerializerIgnore]
        public bool IsLoaded { get; protected set; }
        [ContentSerializerIgnore]
        protected bool RemoveNextUpdate { get; set; }

        bool _isEnabled = true;
        [ContentSerializer]
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnEnabledChanged();
                }
            }
        }

        bool _isVisible = true;
        [ContentSerializer]
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnVisibleChanged();
                }
            }
        }

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
                    if (Collection != null)
                        Collection.NeedsToSort = true;
                }
            }
        }

        public EntityBase()
            : base()
        {
            IsVisible = true;
            IsEnabled = true;
        }

        ~EntityBase()
        {      
            Dispose(false);
        }
 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
  
        protected virtual void Dispose(bool disposing)
        {
        }

#if WINDOWS
        bool IImportable.ImportCustomValues(string name, string value)
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

        public void RemoveNextFrame()
        {
            RemoveNextUpdate = true;
        }

        public void RemoveImmediate()
        {
            if (Collection != null)
                Collection.Remove(this);
        }

        protected virtual void LateInitialize()
        {
        }

        void IEntity.LoadContent()
        {
            InternalLoadContent();
        }

        internal void InternalLoadContent()
        {
            if (!IsLoaded)
            {
                IsLoaded = true;
                LoadContent();
            }
        }

        protected virtual void LoadContent()
        {
        }

        void IEntity.Update(DeltaTime time)
        {
            InternalUpdate(time);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void InternalUpdate(DeltaTime time)
        {
            if (RemoveNextUpdate)
                RemoveImmediate();
            if (!IsLateInitialized)
            {
                IsLateInitialized = true;
                LateInitialize();
            }
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
            return IsEnabled;
        }

        protected virtual void LightUpdate(DeltaTime time)
        {
        }

        protected internal virtual void HeavyUpdate(DeltaTime time)
        {
        }

        void IEntity.Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            InternalDraw(time, spriteBatch);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalDraw(DeltaTime time, SpriteBatch spriteBatch)
        {
            if (CanDraw())
                Draw(time, spriteBatch);
        }

        protected virtual bool CanDraw()
        {
            return IsVisible; 
        }

        protected virtual void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
        }

        public virtual void Recycle()
        {
            _collection = null;
            _layer = 0.0f;
            IsLoaded = false;
            IsEnabled = true;
            IsLateInitialized = false;
            IsVisible = true;
            RemoveNextUpdate = false;
        }

        protected internal virtual void OnEnabledChanged()
        {
        }

        protected internal virtual void OnVisibleChanged()
        {
        }

        void IEntity.OnAdded()
        {
            OnAdded();
        }

        protected internal virtual void OnAdded()
        {
        }

        void IEntity.OnRemoved()
        {
            OnRemoved();
        }

        protected internal virtual void OnRemoved()
        {
            RemoveNextUpdate = false;
            _collection = null;
        }

    }

}
