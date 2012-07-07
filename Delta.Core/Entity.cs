using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    public class Entity : IEntity
    {
        static Dictionary<string, IEntity> _idReferences = new Dictionary<string, IEntity>();

        public static IEntity Get(string id)
        {
            if (_idReferences.ContainsKey(id.ToLower()))
                return _idReferences[id.ToLower()];
            return null;
        }

        internal static bool AddReferenceID(IEntity item, string newID)
        {
            if (string.IsNullOrEmpty(newID))
                newID = Guid.NewGuid().ToString();
            if (_idReferences.ContainsKey(newID))
            {
                for (int x = 0; x < int.MaxValue; x++)
                {
                    string offsetID = newID + x;
                    if (!_idReferences.ContainsKey(offsetID))
                    {
                        newID = offsetID;
                        break;
                    }
                }
            }
            _idReferences.Add(newID, item);
            return true;
        }

        internal static void RemoveReferenceID(string id)
        {
            if (_idReferences.ContainsKey(id.ToLower()))
                _idReferences.Remove(id);
        }

        internal static bool ChangeReferenceID(string oldID, string newID)
        {
            IEntity item = Get(oldID);
            if (item == null)
                return false;
            if (!AddReferenceID(item, newID))
                return false;
            RemoveReferenceID(oldID);
            return true;
        }

        [ContentSerializerIgnore]
        IEntityParent _parent = null;
        public IEntityParent Parent { get; private set; }
        
        //allow the parent to be set by the interface
        IEntityParent IEntity.Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        protected bool ContentIsLoaded { get; private set; }
        [ContentSerializerIgnore]
        protected bool IsInitialized { get; private set; }
        [ContentSerializer]
        public bool IsEnabled { get; set; }
        [ContentSerializerIgnore]
        protected bool IsUpdating { get; private set; }
        [ContentSerializer]
        public bool IsVisible { get; set; }
        [ContentSerializerIgnore]
        protected bool IsDrawing { get; private set; }
        [ContentSerializerIgnore]
        public bool NeedsHeavyUpdate { get; set; }

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
                    Entity.ChangeReferenceID(oldID, _id);
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

    }
}
