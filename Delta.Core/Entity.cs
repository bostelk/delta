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
    [Flags]
    internal enum EntityState
    {
        None = 0x0,
        IsInitialized = 0x1,
        ContentIsLoaded = 0x2,
        IsUpdating = 0x4,
        IsDrawing = 0x8,
        NeedsHeavyUpdate = 0x10,
        Enabled = 0x20,
        Visible = 0x40
    }

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

        EntityState _state = EntityState.Enabled | EntityState.Visible;

        [ContentSerializerIgnore]
        IEntityParent _parent = null;
        public IEntityParent Parent { get; private set; }
        
        //allow the parent to be only set by the interface
        IEntityParent IEntity.Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        [ContentSerializerIgnore]
        public bool IsInitialized
        {
            get { return _state.HasFlag(EntityState.IsInitialized); }
            set
            {
                if (value)
                    _state |= EntityState.IsInitialized;
                else
                    _state &= ~EntityState.IsInitialized;
            }
        }

        [ContentSerializerIgnore]
        public bool ContentIsLoaded
        {
            get { return _state.HasFlag(EntityState.ContentIsLoaded); }
            set
            {
                if (value)
                    _state |= EntityState.ContentIsLoaded;
                else
                    _state &= ~EntityState.ContentIsLoaded;
            }
        }

        [ContentSerializerIgnore]
        public bool IsUpdating
        {
            get { return _state.HasFlag(EntityState.IsUpdating); }
            set
            {
                if (value)
                    _state |= EntityState.IsUpdating;
                else
                    _state &= ~EntityState.IsUpdating;
            }
        }

        [ContentSerializerIgnore]
        public bool IsDrawing
        {
            get { return _state.HasFlag(EntityState.IsDrawing); }
            set
            {
                if (value)
                    _state |= EntityState.IsDrawing;
                else
                    _state &= ~EntityState.IsDrawing;
            }
        }

        [ContentSerializerIgnore]
        public bool NeedsHeavyUpdate
        {
            get { return _state.HasFlag(EntityState.NeedsHeavyUpdate); }
            set
            {
                if (value)
                    _state |= EntityState.NeedsHeavyUpdate;
                else
                    _state &= ~EntityState.NeedsHeavyUpdate;
            }
        }

        [ContentSerializer]
        public bool IsEnabled
        {
            get { return _state.HasFlag(EntityState.Enabled); }
            set
            {
                if (value)
                    _state |= EntityState.Enabled;
                else
                    _state &= ~EntityState.Enabled;
            }
        }

        [ContentSerializer]
        public bool IsVisible
        {
            get { return _state.HasFlag(EntityState.Visible); }
            set
            {
                if (value)
                    _state |= EntityState.Visible;
                else
                    _state &= ~EntityState.Visible;
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
