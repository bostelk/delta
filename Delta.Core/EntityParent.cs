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
    internal class EntityComparer<T> : IComparer<T> where T : IEntity
    {
        internal EntityComparer()
            : base()
        {
        }
        public int Compare(T x, T y)
        {
            return x.Depth.CompareTo(y.Depth);
        }
    }

    /// <summary>
    /// Base class for all game entity parents.
    /// </summary>
    /// <typeparam name="T">The type of entities in the <see cref="EntityParent{T}"/>.</typeparam>
    public class EntityParent<T> : Entity, IEntityCollection where T : IEntity
    {
        IComparer<T> _defaultComparer = new EntityComparer<T>();
        internal List<T> _children = new List<T>();

        /// <summary>
        /// Gets the <see cref="ReadOnlyCollection{T}"/> of children entities.
        /// </summary>
        public ReadOnlyCollection<T> Children { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="EntityParent{T}"/> needs to sort it's children.
        /// </summary>
        public bool NeedsToSort { get; set; }
        /// <summary>
        /// Gets or sets a custom <see cref="IComparer{T}"/> used to sort the <see cref="EntityParent{T}"/>'s children.
        /// </summary>
        /// <remarks>The default value is null.</remarks>
        public IComparer<T> Comparer { get; set; }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public EntityParent()
            : base()
        {
            Children = new ReadOnlyCollection<T>(_children);
            NeedsToSort = true;
            Comparer = null;
        }

        /// <summary>
        /// Adds an <see cref="{T}"/> to the end of the <see cref="EntityParent{T}"/> and sets <see cref="NeedsToSort"/> to true.
        /// </summary>
        /// <param name="item">The <see cref="IEntity"/> to add to the end of the <see cref="EntityParent{T}"/>.</param>
        public virtual void Add(T item)
        {
            if (_children.Contains(item)) 
                return;
            _children.Add(item);
            NeedsToSort = true;
            item.ParentCollection = this;
            if (!string.IsNullOrEmpty(item.Name))
                EntityHelper.AddIDReference(item);
            item.OnAdded();
        }
        void IEntityCollection.UnsafeAdd(IEntity item)
        {
            Add((T)item);
        }

        /// <summary>
        /// Removed an <see cref="{T}"/> to the end of the <see cref="EntityParent{T}"/> and sets <see cref="NeedsToSort"/> to true.
        /// </summary>
        /// <param name="item"></param>
        public virtual void Remove(T item)
        {
            if (!_children.Contains(item)) 
                return;
            _children.FastRemove<T>(item);
            item.ParentCollection = null;
            NeedsToSort = true;
            if (!string.IsNullOrEmpty(item.Name))
                EntityHelper.RemoveIDReference(item);
            item.OnRemoved();
        }
        void IEntityCollection.UnsafeRemove(IEntity item)
        {
            Remove((T)item);
        }

        /// <summary>
        /// Loads content handled by the <see cref="EntityParent{T}"/>.
        /// </summary>
        protected override void LoadContent()
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].LoadContent();
        }

        void IEntityCollection.Update(DeltaGameTime time)
        {
            InternalUpdate(time);
        }
        void IEntityCollection.Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            InternalDraw(time, spriteBatch);
        }

        /// <summary>
        /// Sorts all the entities in the <see cref="EntityParent{T}"/>.
        /// </summary>
        protected virtual void Sort()
        {
            if (Comparer != null)
                _children.Sort(Comparer);
            else
                _children.Sort(_defaultComparer);
            NeedsToSort = false;
        }

        /// <summary>
        /// Called when the <see cref="EntityParent{T}"/> is finished updating.
        /// </summary>
        /// <param name="time">time</param>
        protected override void OnEndUpdate(DeltaGameTime time)
        {
            base.OnEndUpdate(time);
            if (NeedsToSort)
                Sort();
            for (int i = 0; i < _children.Count; i++)
                _children[i].Update(time);
        }

        /// <summary>
        /// Called when the <see cref="EntityParent{T}"/> is finished drawing.
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="spriteBatch">spriteBatch</param>
        protected override void OnEndDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnEndDraw(time, spriteBatch);
            for (int i = 0; i < _children.Count; i++)
                _children[i].Draw(time, spriteBatch);
        }
    }

    /// <summary>
    /// Custom <see cref="ContentTypeReader{T}"/> for <see cref="EntityParent{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of entities in the <see cref="EntityParent{T}"/>.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityParentReader<T> : ContentTypeReader<EntityParent<T>> where T : IEntity
    {
        /// <summary>
        /// Reads a <see cref="EntityParent{T}"/> from the current stream.
        /// </summary>
        /// <param name="input">The <see cref="ContentReader"/> used to read the <see cref="EntityParent{T}"/>.</param>
        /// <param name="value">An existing <see cref="EntityParent{T}"/> to read into.</param>
        /// <returns>The <see cref="EntityParent{T}"/> that was read.</returns>
        protected override EntityParent<T> Read(ContentReader input, EntityParent<T> value)
        {
            if (value == null)
                value = new EntityParent<T>();
            input.ReadRawObject<Entity>(value as Entity);
            List<T> gameComponents = input.ReadObject<List<T>>();
            foreach (var item in gameComponents)
                value.Add(item);
            return value;
        }
    }
}
