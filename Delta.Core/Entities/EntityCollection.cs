using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class EntityCollection : EntityCollection<IEntity>
    {
        public EntityCollection()
            : base()
        {
        }
    }

    public class EntityCollection<T> : EntityBase, IEntityCollection, IEnumerable where T : IEntity
    {
        static Comparison<T> _defaultComparer = (a, b) => (a.Layer.CompareTo(b.Layer));
        static List<IEntity> _globalComponents = new List<IEntity>();
        static ReadOnlyCollection<IEntity> _globalComponentsReadOnly = null; 
        public static ReadOnlyCollection<IEntity> GlobalComponents
        {
            get
            {
                if (_globalComponentsReadOnly == null)
                    _globalComponentsReadOnly = new ReadOnlyCollection<IEntity>(_globalComponents);
                return _globalComponentsReadOnly;
            }
        }

        internal List<T> _components = new List<T>();

        public ReadOnlyCollection<T> Children { get; private set; }
        public bool NeedsToSort { get; set; }
        public bool AlwaysSort { get; set; }
        public IComparer<T> Comparer { get; set; }

        public EntityCollection()
            : base()
        {
            Children = new ReadOnlyCollection<T>(_components);
            NeedsToSort = true;
            Comparer = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        void IEntityCollection.Add(IEntity item)
        {
            Add((T)item);
        }

        public virtual void Add(T item)
        {
            if (_components.Contains(item)) 
                return;
            _components.Add(item);
            _globalComponents.Add(item);
            NeedsToSort = true;
            item.Collection = this;
            if (!string.IsNullOrEmpty(item.Name))
                EntityHelper.AddIDReference(item);
            item.OnAdded();
        }

        public void Add(params T[] items)
        {
            foreach (T item in items)
                Add(item);
        }

        void IEntityCollection.Remove(IEntity item)
        {
            Remove((T)item);
        }

        public virtual void Remove(T item)
        {
            if (!_components.Contains(item)) 
                return;
            _components.FastRemove<T>(item);
            _globalComponents.FastRemove<IEntity>(item);
            NeedsToSort = true;
            item.Collection = null;
            if (!string.IsNullOrEmpty(item.Name))
                EntityHelper.RemoveIDReference(item);
            item.OnRemoved();
        }

        public void Remove(params T[] items)
        {
            foreach (T item in items)
                Remove(item);
        }

        protected override void LoadContent()
        {
            for (int i = 0; i < _components.Count; i++)
                _components[i].LoadContent();
        }

        void IEntityCollection.Update(DeltaTime time)
        {
            InternalUpdate(time);
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (AlwaysSort || NeedsToSort)
                Sort();
            for (int i = 0; i < _components.Count; i++)
                _components[i].Update(time);
        }

        void IEntityCollection.Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            InternalDraw(time, spriteBatch);
        }

        protected override void Draw(DeltaTime time, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _components.Count; i++)
                _components[i].Draw(time, spriteBatch);
        }

        protected virtual void Sort()
        {
            if (Comparer != null)
                _components.Sort(Comparer);
            else
                _components.Sort(_defaultComparer);
            NeedsToSort = false;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityCollectionReader<T> : ContentTypeReader<EntityCollection<T>> where T : IEntity
    {
        protected override EntityCollection<T> Read(ContentReader input, EntityCollection<T> value)
        {
            if (value == null)
                value = new EntityCollection<T>();
            input.ReadRawObject<EntityBase>(value as EntityBase);
            List<T> gameComponents = input.ReadObject<List<T>>();
            foreach (var item in gameComponents)
                value.Add(item);
            return value;
        }
    }
}
