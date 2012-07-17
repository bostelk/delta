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
    public class DeltaGameComponentCollection : DeltaGameComponentCollection<IGameComponent>
    {
        public DeltaGameComponentCollection()
            : base()
        {
        }
    }

    public class DeltaGameComponentCollection<T> : DeltaGameComponent, IGameComponentCollection, IEnumerable<T>, IEnumerable where T : IGameComponent
    {
        static Comparison<T> _defaultComparer = (a, b) => (a.Layer.CompareTo(b.Layer));
        static List<IGameComponent> _globalComponents = new List<IGameComponent>();
        static ReadOnlyCollection<IGameComponent> _globalComponentsReadOnly = null; 
        public static ReadOnlyCollection<IGameComponent> GlobalComponents
        {
            get
            {
                if (_globalComponentsReadOnly == null)
                    _globalComponentsReadOnly = new ReadOnlyCollection<IGameComponent>(_globalComponents);
                return _globalComponentsReadOnly;
            }
        }

        internal List<T> _components = new List<T>();

        public ReadOnlyCollection<T> Components { get; private set; }
        public bool NeedsToSort { get; set; }
        public bool AlwaysSort { get; set; }
        public IComparer<T> Comparer { get; set; }

        public DeltaGameComponentCollection()
            : base()
        {
            Components = new ReadOnlyCollection<T>(_components);
            NeedsToSort = true;
            Comparer = null;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        void IGameComponentCollection.Add(IGameComponent item)
        {
            Add((T)item);
        }

        public void Add(T item)
        {
            _components.Add(item);
            _globalComponents.Add(item);
            NeedsToSort = true;
            item.Collection = this;
            IEntity entity = item as IEntity;
            if (entity != null)
                EntityHelper.AddIDReference(entity);
            item.OnAdded();
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
            _globalComponents.FastRemove<IGameComponent>(item);
            NeedsToSort = true;
            item.Collection = null;
            IEntity entity = item as IEntity;
            if (entity != null)
                EntityHelper.RemoveIDReference(entity);
            item.OnRemoved();
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
            if (AlwaysSort || NeedsToSort)
                Sort();
            for (int i = 0; i < _components.Count; i++)
                _components[i].Update(time);
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
