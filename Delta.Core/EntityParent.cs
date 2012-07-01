using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{

    public class EntityParent<T> : Entity, IEntityParent where T : IEntity
    {
        [ContentSerializer(FlattenContent=true, CollectionItemName="Entity")]
        internal List<T> _children = new List<T>();

        [ContentSerializerIgnore]
        public ReadOnlyCollection<T> Children { get; private set; }
        [ContentSerializerIgnore]
        public bool NeedsToSort { get; set; }

        IComparer<T> _comparer = null;
        [ContentSerializerIgnore]
        protected internal IComparer<T> Comparer
        {
            get { return _comparer; }
            set
            {
                if (_comparer != value)
                {
                    _comparer = value;
                    NeedsToSort = true;
                }
            }
        }

        public EntityParent()
            : base()
        {
            Children = new ReadOnlyCollection<T>(_children);
            NeedsToSort = false;
        }

        public List<bool> AddRange(List<T> entityRange, int order)
        {
            List<bool> results = new List<bool>(entityRange.Count);
            foreach (T entity in entityRange)
            {
                entity.Order = order;
                results.Add(Add(entity, order));
            }
            return results;
        }

        public bool Add(T entity, int order)
        {
            entity.Order = order;
            return Add(entity);
        }

        public bool Add(T entity)
        {
            if (!Entity.AddReferenceID(entity, entity.ID))
                return false;
            _children.Add(entity);
            entity.Parent = this;
            NeedsToSort = true;
            return true;
        }

        public bool Remove(T entity)
        {
            bool returnValue = false;
            returnValue = _children.Remove(entity);
            if (returnValue)
                Entity.RemoveReferenceID(entity.ID);
            return returnValue;
        }

        public override void LoadContent()
        {
            for (int x = 0; x < _children.Count; x++)
                _children[x].LoadContent();
        }

        protected internal override void EndUpdate(GameTime gameTime)
        {
            if (NeedsToSort)
                Sort();
            for (int x = 0; x < _children.Count; x++)
                _children[x].InternalUpdate(gameTime);
            base.EndUpdate(gameTime);
        }

        protected internal override void EndDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _children.Count; x++)
                _children[x].InternalDraw(gameTime, spriteBatch);
            base.EndDraw(gameTime, spriteBatch);
        }

        protected virtual void Sort()
        {
            if (_comparer == null)
                _children.Sort((a, b) => (a.Order.CompareTo(b.Order)));
            else
                _children.Sort(_comparer);
            NeedsToSort = false;
        }

    }

}
