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
    public class DeltaGameComponent : IRecyclable, IImportable, IGameComponent
    {
        IGameComponentCollection _collection = null;

        [ContentSerializerIgnore]
        protected internal IGameComponentCollection Collection { get { return _collection; } }

        [ContentSerializerIgnore]
        IGameComponentCollection IGameComponent.Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }

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
        [ContentSerializerIgnore]
        public bool RemoveNextUpdate { get; set; }

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

        public DeltaGameComponent()
            : base()
        {
            IsVisible = true;
            IsEnabled = true;
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
        public void Remove()
        {
            if (Collection != null)
                Collection.Remove(this);
        }

        protected virtual void LateInitialize()
        {
        }

        public virtual void LoadContent()
        {
        }

        void IGameComponent.Update(DeltaTime time)
        {
            InternalUpdate(time);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalUpdate(DeltaTime time)
        {
            if (RemoveNextUpdate)
                Remove();
            if (!IsLateInitialized)
            {
                IsLateInitialized = true;
                LateInitialize();
            }
            if (!HasLoadedContent)
            {
                HasLoadedContent = true;
                LoadContent();
            }
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
            NeedsHeavyUpdate = false;
        }

        void IGameComponent.Draw(DeltaTime time, SpriteBatch spriteBatch)
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
            HasLoadedContent = false;
            IsEnabled = true;
            IsLateInitialized = false;
            IsVisible = true;
            RemoveNextUpdate = false;
        }

        void IGameComponent.OnAdded()
        {
            OnAdded();
        }

        protected internal virtual void OnAdded()
        {
        }

        void IGameComponent.OnRemoved()
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
