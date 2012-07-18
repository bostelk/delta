using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Collision;
using Microsoft.Xna.Framework;
using Delta.Collision.Geometry;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    public class CollideableEntity : Entity
    {
        public bool ColliderEnabled = true;

        private Collider _collider;
        [ContentSerializerIgnore]
        public Collider Collider
        {
            get
            {
                return _collider;
            }
            set
            {
                _collider = value;
                OnColliderChanged();
            }
        }

        private Polygon _polygon;
        [ContentSerializerIgnore]
        public Polygon Polygon
        {
            get
            {
                return _polygon;
            }
            set
            {
                _polygon = value;
            }
        }

        public CollideableEntity(string name) : base(name) { }

        public CollideableEntity() : this(String.Empty) { }

        protected override void LateInitialize()
        {
            if (Collider == null && _polygon != null)
            {
                Collider = new Collider(this, _polygon);
            }
            base.LateInitialize();
        }

        protected internal virtual void OnColliderChanged()
        {
            if (_collider != null && G.Collision != null)
                G.Collision.RemoveColider(_collider);
            _collider.BeforeCollision = BeforeCollision;
            _collider.OnCollision = OnCollision;
            _collider.AfterCollision = AfterCollision;
            _collider.OnSeparation = OnSeparation;
            Polygon = _collider.Geom;
            if (G.Collision != null)
                G.Collision.AddCollider(_collider);
        }

        protected internal virtual void OnPolygonChanged()
        {
            if (_collider != null)
            {
                _collider.Geom = _polygon;
                _collider.Geom.Position = base.Position;
                _collider.Geom.Rotation = base.Rotation;
            }
        }

        protected override void LightUpdate(DeltaTime time)
        {
            if (ColliderEnabled)
                UpdateFromWrappedBody();

            base.LightUpdate(time);
        }

        protected virtual bool BeforeCollision(Collider them, Vector2 normal)
        {
            return true;
        }

        protected virtual bool OnCollision(Collider them, Vector2 normal)
        {
            return true;
        }

        protected virtual void AfterCollision(Collider them, Vector2 normal)
        {
        }

        protected virtual void OnSeparation(Collider them)
        {
        }
    }
}
