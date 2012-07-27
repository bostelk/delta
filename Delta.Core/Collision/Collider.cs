using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision;
using Microsoft.Xna.Framework;
using Delta.Structures;

namespace Delta.Collision
{
    public sealed class Collider : ICollideable, IWrappedBody, IRecyclable
    {
        static Pool<Collider> _pool;

        public object Owner { get; set; }

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
                IsAwake = true;
            }
        }

        Matrix3 _worldTransform;
        /// <summary>
        /// Transform the shape into world space.
        /// </summary>
        public Matrix3 WorldTransform
        {
            get
            {
                if (IsAwake) // re-calculate on awaken, then cache.
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

        public BroadphaseProxy BroadphaseProxy { get; set; }

        /// <summary>
        /// Will not move due to collision. (No collision response).
        /// </summary>
        public bool IsStatic;

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

        #region IWrappedBody
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
        #endregion

        static Collider()
        {
            _pool = new Pool<Collider>(200);
        }

        // alias for entities
        public static IWrappedBody CreateBody(CollisionShape shape)
        {
            return Create(null, shape) as IWrappedBody;
        }

        public static IWrappedBody CreateBody(TransformableEntity entity, CollisionShape shape)
        {
            return Create(entity, shape) as IWrappedBody;
        }

        public static Collider Create(TransformableEntity entity, CollisionShape shape)
        {
            Collider collider = _pool.Fetch();
            collider.Owner = entity;
            collider.Shape = shape;
            collider.OnCollision += collider.HandleOnCollision;
            collider.BeforeCollision += collider.HandleBeforeCollision;
            return collider;
        }

        public Collider(TransformableEntity entity, CollisionShape shape)
            : this()
        {
            Owner = entity;
            Shape = shape;
        }

        public Collider() 
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
            //RemoveNextUpdate = true;
        }

        public void OnAdded() { }

        public void OnRemoved()
        {
            RemoveNextUpdate = false;
            Recycle(); // will also recycle the collider's broadphase proxy too.
        }

        private bool HandleBeforeCollision(Collider them)
        {
            if (BeforeCollisionEvent != null)
                return BeforeCollisionEvent(them as IWrappedBody);
            return true;
        }

        private bool HandleOnCollision(Collider them, Vector2 normal)
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
