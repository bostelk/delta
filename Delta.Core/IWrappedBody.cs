using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    [Flags]
    public enum CollisionOptions
    {
        Solid,
        Response,
        Ignore, // don't fire events.
    }

    /// <summary>
    /// Defines a wrapped body.
    /// </summary>
    public interface IWrappedBody
    {
        /// <summary>
        /// Gets or sets the position of the <see cref="IWrappedBody"/> which is used for collision logic.
        /// </summary>
        Vector2 SimulationPosition { get; set; }
        /// <summary>
        /// Gets or sets the rotation of the <see cref="IWrappedBody"/> expressed in radians which is used for collision logic. 
        /// </summary>
        float SimulationRotation { get; set; }
        /// <summary>
        /// Gets or sets the object instance the <see cref="IWrappedBody"/> belongs to.
        /// </summary>
        object Owner { get; set; }
        /// <summary>
        /// Gets or sets the shape of the <see cref="IWrappedBody"/> which is used for collision logic.
        /// </summary>
        CollisionShape Shape { get; set; }
        ///// <summary>
        ///// The body will be added to the simulator on next frame.
        ///// </summary>
        void AddToSimulation();
        /// <summary>
        /// The body will be removed from the simulator on next frame.
        /// </summary>
        void RemoveFromSimulation();
        /// <summary>
        /// The shape is about to collide with another. Broadphase detection.
        /// </summary>
        //Func<CollisionShape, bool> BeforeCollision { get; set; }
        /// <summary>
        /// The shape is colliding with another. Narrowpahse detection.
        /// </summary>
        Func<IWrappedBody, Vector2, bool> OnCollisionEvent { get; set; }
    }
}
