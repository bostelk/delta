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
    internal class EntityComparer<T> : IComparer<T> where T : Entity
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
    public class EntityParent<T> : Entity, IEntityCollection where T : Entity
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
        protected internal EntityParent()
            : base()
        {
            Children = new ReadOnlyCollection<T>(_children);
            NeedsToSort = true;
            Comparer = null;
        }

        /// <summary>
        /// Adds an <see cref="{T}"/> to the end of the <see cref="EntityParent{T}"/> and sets <see cref="NeedsToSort"/> to true.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to add to the end of the <see cref="EntityParent{T}"/>.</param>
        public virtual void Add(T entity)
        {
            if (_children.Contains(entity)) 
                return;
            _children.Add(entity);
            NeedsToSort = true;
            entity.ParentCollection = this;
            if (!string.IsNullOrEmpty(entity.Name))
                EntityHelper.AddIDReference(entity);
            if (!EntityHelper._globalEntityReferences.Contains(entity))
                EntityHelper._globalEntityReferences.Add(entity);
            entity.OnAdded();
        }
        void IEntityCollection.UnsafeAdd(Entity entity)
        {
            Add((T)entity);
        }

        /// <summary>
        /// Removed an <see cref="{T}"/> to the end of the <see cref="EntityParent{T}"/> and sets <see cref="NeedsToSort"/> to true.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(T entity)
        {
            if (!_children.Contains(entity)) 
                return;
            _children.FastRemove<T>(entity);
            entity.ParentCollection = null;
            NeedsToSort = true;
            if (!string.IsNullOrEmpty(entity.Name))
                EntityHelper.RemoveIDReference(entity);
            if (EntityHelper._globalEntityReferences.Contains(entity))
                EntityHelper._globalEntityReferences.Remove(entity);
            entity.OnRemoved();
        }
        void IEntityCollection.UnsafeRemove(Entity entity)
        {
            Remove((T)entity);
        }

        /// <summary>
        /// Loads content handled by the <see cref="EntityParent{T}"/>.
        /// </summary>
        protected override void LoadContent()
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].InternalLoadContent();
        }

        void IEntityCollection.Update(DeltaGameTime time)
        {
            InternalUpdate(time);
        }

        /// <summary>
        /// Updates the children in the <see cref="EntityParent{T}"/>.
        /// </summary>
        /// <param name="time">time</param>
        protected internal virtual void UpdateChildren(DeltaGameTime time)
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].InternalUpdate(time);
        }

        void IEntityCollection.Draw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            InternalDraw(time, spriteBatch);
        }

        /// <summary>
        /// Draws the children in the <see cref="EntityParent{T}"/>.
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="spriteBatch">spriteBatch</param>
        protected internal virtual void DrawChildren(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].InternalDraw(time, spriteBatch);
        }

        /// <summary>
        /// Sorts all the children in the <see cref="EntityParent{T}"/>.
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
            UpdateChildren(time);
        }

        /// <summary>
        /// Called when the <see cref="Entity"/> is finished drawing.
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="spriteBatch">spriteBatch</param>
        protected override void OnEndDraw(DeltaGameTime time, SpriteBatch spriteBatch)
        {
            base.OnEndDraw(time, spriteBatch);
            DrawChildren(time, spriteBatch);
        }
    }

    /// <summary>
    /// Custom <see cref="ContentTypeReader{T}"/> for <see cref="EntityParent{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The type of entities in the <see cref="EntityParent{T}"/>.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class EntityParentReader<T> : ContentTypeReader<EntityParent<T>> where T : Entity
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
