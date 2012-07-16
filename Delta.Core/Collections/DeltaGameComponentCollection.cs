using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public class DeltaGameComponentCollection : DeltaGameComponentCollection<IGameComponent>
    {
        public DeltaGameComponentCollection()
            : base()
        {
        }
    }

    public class DeltaGameComponentCollection<T> : DeltaGameComponent, IGameComponentCollection where T : IGameComponent
    {
        internal List<T> _components = new List<T>();

        public ReadOnlyCollection<T> Components { get; private set; }
        public bool NeedsToSort { get; set; }
        public IComparer<T> Comparer { get; set; }

        public DeltaGameComponentCollection()
            : base()
        {
            Components = new ReadOnlyCollection<T>(_components);
            NeedsToSort = true;
            Comparer = null;
        }

        void IGameComponentCollection.Add(IGameComponent item)
        {
            Add((T)item);
        }

        public void Add(T item)
        {
            _components.Add(item);
            NeedsToSort = true;
            IEntity entity = item as IEntity;
            if (entity != null)
            {
                entity.Collection = this;
                EntityHelper.AddIDReference(entity);
            }
        }

        public void Add(params T[] items)
        {
            foreach (T item in items)
                Add(item);
        }

        void IGameComponentCollection.Remove(IGameComponent item)
        {
            Remove((T)item);
        }

        public void Remove(T item)
        {
            _components.FastRemove<T>(item);
            NeedsToSort = true;
            IEntity entity = item as IEntity;
            if (entity != null)
            {
                entity.Collection = null;
                EntityHelper.RemoveIDReference(entity);
            }
        }

        public void Remove(params T[] items)
        {
            foreach (T item in items)
                Remove(item);
        }

        public override void LoadContent()
        {
            for (int i = 0; i < _components.Count; i++)
                _components[i].LoadContent();
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (NeedsToSort)
                Sort();
            for (int _loopIndex = 0; _loopIndex < _components.Count; _loopIndex++)
                _components[_loopIndex].Update(time);
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
                _components.Sort((a, b) => (a.Layer.CompareTo(b.Layer)));
            NeedsToSort = false;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DeltaGameComponentCollectionReader<T> : ContentTypeReader<DeltaGameComponentCollection<T>> where T : IGameComponent
    {
        protected override DeltaGameComponentCollection<T> Read(ContentReader input, DeltaGameComponentCollection<T> value)
        {
            if (value == null)
                value = new DeltaGameComponentCollection<T>();
            input.ReadRawObject<DeltaGameComponent>(value as DeltaGameComponent);
            List<T> gameComponents = input.ReadObject<List<T>>();
            foreach (var item in gameComponents)
                value.Add(item);
            return value;
        }
    }
}
