using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision;
using Delta.Physics;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Collision
{
    public class Collider : IRecyclable
    {
        static Pool<Collider> _pool;

        public Entity Tag;

        private CollisionShape _shape;
        public CollisionShape Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
                IsActive = true;
            }
        }

        Transform _worldTransform;
        public Transform WorldTransform
        {
            get
            {
                return _worldTransform;
            }
            set
            {
                _worldTransform = value;
                IsActive = true;
            }
        }

        public Vector2 Position
        {
            get
            {
                return _worldTransform.Origin;
            }
            set
            {
                if (Vector2Extensions.AlmostEqual(_worldTransform.Origin, value))
                    return;
                _worldTransform.Origin = value;
                IsActive = true;
            }
        }

        public float Rotation
        {
            get
            {
                return _worldTransform.Rotation;
            }
            set
            {
                if (FloatExtensions.AlmostEqual(_worldTransform.Rotation, value))
                    return;
                _worldTransform.Rotation = value;
                IsActive = true;
            }
        }

        public BroadphaseProxy BroadphaseProxy { get; set; }

        /// <summary>
        /// Will not move due to collision. (No collision response).
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// The Collider has moved this frame.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The shape is about to collide. (broadphase overlap).
        /// </summary>
        public BeforeCollisionEventHandler BeforeCollision;

        /// <summary>
        /// The shape is colliding. (narrowphase collision).
        /// </summary>
        public OnCollisionEventHandler OnCollision;

        /// <summary>
        /// The shape has been resolved since collision.
        /// </summary>
        public AfterCollisionEventHandler AfterCollision;

        public OnSeparationEventHandler OnSeparation;

        static Collider()
        {
            _pool = new Pool<Collider>(200);
        }

        public static Collider Create(Entity entity, CollisionShape shape)
        {
            Collider collider = _pool.Fetch();
            collider.Tag = entity;
            collider.Shape = shape;
            return collider;
        }

        public Collider(Entity entity, CollisionShape shape)
            : this()
        {
            Tag = entity;
            Shape = shape;
        }

        public Collider() 
        {
            WorldTransform = Transform.Identity;
        }

        public void Recycle()
        {
            BroadphaseProxy.Recycle();
            WorldTransform = Transform.Identity;
            Tag = null;
            Shape = null;
            BeforeCollision = null;
            OnCollision = null;
            AfterCollision = null;
            OnSeparation = null;

            _pool.Release(this);
        }

        public override int GetHashCode()
        {
            return Shape.GetHashCode() + WorldTransform.GetHashCode();
        }
    }
}
