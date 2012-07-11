using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Globalization;

namespace Delta
{
    public class Entity : IEntity
    {
        public static IEntity Get(string id)
        {
            if (EntityHelper._idReferences.ContainsKey(id.ToLower()))
                return EntityHelper._idReferences[id.ToLower()];
            return null;
        }

        public static ReadOnlyCollection<IEntity> GlobalEntities
        {
            get { return EntityHelper._globalEntities; }
        }

        [ContentSerializerIgnore]
        IEntityParent _parent = null;
        public IEntityParent Parent { get; private set; }
        
        //allow the parent to be only set by the interface
        IEntityParent IEntity.Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        protected bool ContentIsLoaded { get; private set; }
        [ContentSerializerIgnore]
        protected bool IsInitialized { get; private set; }
        [ContentSerializerIgnore]
        protected bool IsUpdating { get; private set; }
        [ContentSerializerIgnore]
        protected bool IsDrawing { get; private set; }
        [ContentSerializerIgnore]
        public bool NeedsHeavyUpdate { get; set; }
        [ContentSerializerIgnore]
        public object Tag { get; set; }

        bool _isEnabled = false;
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

        bool _isVisible = false;
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

        float _order = 0; //allows entities to be sorted for drawing and updating.
        [ContentSerializer]
        public float Order
        {
            get { return _order; }
            set
            {
                if (_order != value)
                {
                    _order = value;
                    if (Parent != null)
                        Parent.NeedsToSort = true;
                }
            }
        }

        string _id = string.Empty; //identification of the entity, forced to be unique when added to a EntityParent
        [ContentSerializer]
        public string ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    string oldID = _id;
                    _id = value.ToLower();
                    EntityHelper.ChangeReferenceID(oldID, _id);
                }
            }
        }

        public Entity()
            : base()
        {
            NeedsHeavyUpdate = true;
            IsEnabled = true;
            IsVisible = true;
        }

        internal void InternalInitialize()
        {
            if (!IsInitialized)
            {
                if (G.Instance == null || G.GraphicsDevice == null)
                    return;
                IsInitialized = true;
                LateInitialize();
            }
        }

        protected virtual void LateInitialize()
        {
        }

        internal void InternalLoadContent()
        {
            if (!ContentIsLoaded)
            {
                ContentIsLoaded = true;
                LoadContent();
            }
        }

        public virtual void LoadContent() 
        {
        }

#if WINDOWS
        protected virtual bool ImportCustomValues(string name, string value)
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
                case "order":
                case "draworder":
                case "updateorder":
                    Order = float.Parse(value, CultureInfo.InvariantCulture);
                    return true;
            }
            return false;
        }
        bool IEntity.ImportCustomValues(string name, string value)
        {
            return ImportCustomValues(name, value);
        }
#endif

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalUpdate(GameTime gameTime)
        {
            if (!IsInitialized)
                InternalInitialize();
            if (!ContentIsLoaded)
                InternalLoadContent();
            if (CanUpdate())
            {
                BeginUpdate(gameTime);
                LightUpdate(gameTime);
                if (NeedsHeavyUpdate)
                {
                    BeginHeavyUpdate(gameTime);
                    HeavyUpdate(gameTime);
                    EndHeavyUpdate(gameTime);
                }
                EndUpdate(gameTime);
            }
        }

        protected virtual bool CanUpdate()
        {
            if (!IsEnabled || IsUpdating) return false;
            return true;
        }

        protected virtual void LightUpdate(GameTime gameTime)
        {
        }

        protected internal virtual void BeginUpdate(GameTime gameTime)
        {
            IsUpdating = true;
        }

        protected internal virtual void EndUpdate(GameTime gameTime)
        {
            IsUpdating = false;
        }

        protected internal virtual void BeginHeavyUpdate(GameTime gameTime)
        {
            NeedsHeavyUpdate = false;
        }

        protected internal virtual void HeavyUpdate(GameTime gameTime)
        {
        }

        protected internal virtual void EndHeavyUpdate(GameTime gameTime)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void InternalDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (CanDraw())
            {
                BeginDraw(gameTime, spriteBatch);
                Draw(gameTime, spriteBatch);
                EndDraw(gameTime, spriteBatch);
            }
        }

        protected virtual bool CanDraw()
        {
            if (!IsVisible || !IsInitialized || IsDrawing) return false;
            return true;
        }

        protected virtual void BeginDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            IsDrawing = true;
        }

        protected internal virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        protected internal virtual void EndDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            IsDrawing = false;
        }

        protected virtual void OnEnabledChanged()
        {
        }

        protected virtual void OnVisibleChanged()
        {
        }

    }
}
