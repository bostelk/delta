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

    public interface IWrappedBody
    {
        Vector2 SimulationPosition { get; set; }

        /// <summary>
        /// Rotation in radians.
        /// </summary>
        float SimulationRotation { get; set; }

        /// <summary>
        /// The Game object that the body belongs to.
        /// </summary>
        object Owner { get; set; }

        /// <summary>
        /// The shape used for collision detection and response.
        /// </summary>
        CollisionShape Shape { get; set; }

        /// <summary>
        /// The body will be added to the simulator on next frame.
        /// </summary>
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
