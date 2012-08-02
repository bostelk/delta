using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IEntityCollection
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="IEntityCollection"/> needs to sort it's children.
        /// </summary>
        bool NeedsToSort { get; set; }
        /// <summary>
        /// Adds an <see cref="IEntity"/> to the <see cref="IEntityCollection"/> without respecting if the <see cref="IEntityCollection"/> is strongly typed.
        /// </summary>
        /// <param name="item">The <see cref="IEntity"/> to add to the <see cref="IEntityCollection"/>.</param>
        void UnsafeAdd(IEntity item);
        /// <summary>
        /// Removes an <see cref="IEntity"/> from the <see cref="IEntityCollection"/> without respecting if the <see cref="IEntityCollection"/> is strongly typed.
        /// </summary>
        /// <param name="item">The <see cref="IEntity"/> to remove the <see cref="IEntityCollection"/>.</param>
        void UnsafeRemove(IEntity item);
        /// <summary>
        /// Updates the <see cref="IEntityCollection"/>.
        /// </summary>
        /// <param name="time">time</param>
        void Update(DeltaGameTime time);
        /// <summary>
        /// Draws the <see cref="IEntityCollection"/>.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="spriteBatch"></param>
        void Draw(DeltaGameTime time, SpriteBatch spriteBatch);
    }
}
