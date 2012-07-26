using System;
using Microsoft.Xna.Framework.Graphics;

namespace Delta
{
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the <see cref="IEntityCollection"/> which is responsible for the <see cref="Entity"/>.
        /// </summary>
        IEntityCollection ParentCollection { get; set; }
        /// <summary>
        /// Gets or sets the name of the <see cref="IEntity"/>.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the depth (update and draw order) of the <see cref="IEntity"/> in the <see cref="ParentCollection"/>.
        /// </summary>
        float Depth { get; }
        /// <summary>
        /// Loads content handled by the <see cref="IEntity"/>.
        /// </summary>
        void LoadContent();
        /// <summary>
        /// Performs a full update of the <see cref="IEntity"/>.
        /// </summary>
        /// <param name="time">time</param>
        void Update(DeltaGameTime time);
        /// <summary>
        /// Performs a full draw of the <see cref="IEntity"/>.
        /// </summary>
        /// <param name="time">time</param>
        /// <param name="spriteBatch">spriteBatch</param>
        void Draw(DeltaGameTime time, SpriteBatch spriteBatch);
        /// <summary>
        /// Called when the <see cref="IEntity"/> has been added to an <see cref="IEntityCollection"/>.
        /// </summary>
        void OnAdded();
        /// <summary>
        /// Called when the <see cref="IEntity"/> has been removed from an <see cref="IEntityCollection"/>.
        /// </summary>
        void OnRemoved();
    }
}
