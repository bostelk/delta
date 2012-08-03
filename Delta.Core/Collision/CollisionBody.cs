using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Collision
{
    [Flags]
    public enum CollisionFlags
    {
        Solid,
        Response,
        /// <summary>
        /// Stops the detection at the broadphase.
        /// </summary>
        Ignore,
    }

    /// <summary>
    /// A Body that belongs in the CollisionWorld. Provides the detection of collisions between shapes.
    /// </summary>
    public sealed class CollisionBody : ICollideable, IWrappedBody, IRecyclable
    {
        static Pool<CollisionBody> _pool;

        public object Owner { get; set; }

        public BroadphaseProxy BroadphaseProxy { get; set; }

        public CollisionFlags Flags { get; set; }

        private CollisionShape _shape;
        /// <summary>
        /// The shape the body is represented by.
        /// </summary>
        public CollisionShape Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
                IsAwake = true;
            }
        }

        Matrix3 _worldTransform;
        /// <summary>
        /// Transforms the body to world space.
        /// </summary>
        public Matrix3 WorldTransform
        {
            get
            {
                if (IsAwake) // re-calculate on awaken and cache.
                {
                    Matrix3 translation = Matrix3.CreateTranslation(_position);
                    Matrix3 rotation = Matrix3.CreateRotation(_rotation);
                    Matrix3.Multiply(ref translation, ref rotation, out _worldTransform);
                }
                return _worldTransform;
            }
            private set
            {
                _worldTransform = value;
            }
        }

        Vector2 _position;
        /// <summary>
        /// Position of the Body in world space.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (!Vector2Extensions.AlmostEqual(_position, value))
                {
                    _position = value;
                    IsAwake = true; // re-calculate aabb
                }
            }
        }

        float _rotation;
        /// <summary>
        /// Rotation of the body in world space in radians.
        /// </summary>
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                if (!FloatExtensions.AlmostEqual(_rotation, value))
                {
                    _rotation = value;
                    IsAwake = true; // re-calculate aabb
                }
            }
        }

        /// <summary>
        /// The Collider has moved this frame.
        /// </summary>
        public bool IsAwake { get; set; }

        public bool RemoveNextUpdate { get; set; }

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

        #region IWrappedBody Contract
        public Vector2 SimulationPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public float SimulationRotation
        {
            get { return Rotation; }
            set { Rotation = value; }
        }

        public Func<IWrappedBody, bool> BeforeCollisionEvent { get; set; }

        public Func<IWrappedBody, Vector2, bool> OnCollisionEvent { get; set; }
        
        // shortcut
        public static IWrappedBody CreateBody(CollisionShape shape)
        {
            return Create(null, shape) as IWrappedBody;
        }

        // shortcut
        public static IWrappedBody CreateBody(TransformableEntity entity, CollisionShape shape)
        {
            return Create(entity, shape) as IWrappedBody;
        }
        #endregion

        static CollisionBody()
        {
            _pool = new Pool<CollisionBody>(200);
        }

        public static CollisionBody Create(TransformableEntity entity, CollisionShape shape)
        {
            CollisionBody collider = _pool.Fetch();
            collider.Owner = entity;
            collider.Shape = shape;
            collider.OnCollision += collider.HandleOnCollision;
            collider.BeforeCollision += collider.HandleBeforeCollision;
            return collider;
        }

        public CollisionBody(TransformableEntity entity, CollisionShape shape)
            : this()
        {
            Owner = entity;
            Shape = shape;
        }

        public CollisionBody() 
        {
            WorldTransform = Matrix3.Identity;
        }

        public void AddToSimulation()
        {
            G.Collision.AddCollider(this);
        }

        public void RemoveFromSimulation()
        {
            G.Collision.RemoveCollider(this);
        }

        public void OnAdded() { }

        public void OnRemoved()
        {
            RemoveNextUpdate = false;
            Recycle(); // will also recycle the collider's broadphase proxy too.
        }

        public void SetGroup(CollisionGroups group)
        {
            BroadphaseProxy.CollisionFilterGroup = group;
        }

        public void CollidesWithGroup(CollisionGroups mask)
        {
            BroadphaseProxy.CollisionFilterMask = mask;
        }

        public bool BelongsToGroup(CollisionGroups group)
        {
            return (BroadphaseProxy.CollisionFilterGroup & group) != CollisionGroups.None;
        }
        
        private bool HandleBeforeCollision(CollisionBody them)
        {
            if (BeforeCollisionEvent != null)
                return BeforeCollisionEvent(them as IWrappedBody);
            return true;
        }

        private bool HandleOnCollision(CollisionBody them, Vector2 normal)
        {
            if (OnCollisionEvent != null)
                return OnCollisionEvent(them as IWrappedBody, normal);
            return true;
        }

        public void Recycle()
        {
            BroadphaseProxy.Recycle();
            WorldTransform = Matrix3.Identity;
            RemoveNextUpdate = false;
            Owner = null;
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
