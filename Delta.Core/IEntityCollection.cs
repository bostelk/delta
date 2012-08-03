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
        /// Adds an <see cref="Entity"/> to the <see cref="IEntityCollection"/> without respecting if the <see cref="IEntityCollection"/> is strongly typed.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to add to the <see cref="IEntityCollection"/>.</param>
        void UnsafeAdd(Entity entity);
        /// <summary>
        /// Removes an <see cref="Entity"/> from the <see cref="IEntityCollection"/> without respecting if the <see cref="IEntityCollection"/> is strongly typed.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to remove the <see cref="IEntityCollection"/>.</param>
        void UnsafeRemove(Entity entity);
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
