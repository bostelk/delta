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
    public class CollideableEntity : TransformableEntity
    {
        public bool ColliderEnabled = true;

        protected Collider _collider;
        [ContentSerializerIgnore]
        public Collider Collider
        {
            get
            {
                return _collider;
            }
            set
            {
                if (value != null)
                {
                    _collider = value;
                    OnColliderChanged();
                }
            }
        }

        Polygon _polygon;
        [ContentSerializer]
        public Polygon Polygon
        {
            get
            {
                return _polygon;
            }
            set
            {
                if (value != null)
                {
                    _polygon = value;
                    OnPolygonChanged();
                }
            }
        }

        [ContentSerializerIgnore]
        public new Vector2 Position
        {
            get
            {
                if (Collider != null && ColliderEnabled)
                    return Collider.Geom.Position;
                else
                    return base.Position;
            }
            set
            {
                base.Position = value;
                if (Collider != null)
                    Collider.Geom.Position = value;
            }
        }

        [ContentSerializerIgnore]
        public new float Rotation
        {
            get
            {
                if (Collider != null && ColliderEnabled)
                    return Collider.Geom.Rotation;
                else
                    return base.Rotation;
            }
            set
            {
                base.Rotation = value;
                if (Collider != null)
                    Collider.Geom.Rotation = value;
            }
        }

        public CollideableEntity()
        {
        }

        protected override void LateInitialize()
        {
            if (Collider == null && _polygon != null)
            {
                if (_polygon is Box)
                    _polygon = new OBB(_polygon as Box);
                Collider = new Collider(this, _polygon);
                Collider.Geom.Position = base.Position + new Vector2((_polygon as OBB).HalfWidth, (_polygon as OBB).HalfHeight);
                Collider.Geom.Rotation = base.Rotation;
                G.Collision.AddCollider(Collider);
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
            _polygon = _collider.Geom;
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

        public virtual bool BeforeCollision(Collider them, Vector2 normal)
        {
            return true;
        }

        public virtual bool OnCollision(Collider them, Vector2 normal)
        {
            return true;
        }

        public virtual void AfterCollision(Collider them, Vector2 normal)
        {
        }

        public virtual void OnSeparation(Collider them)
        {
        }
    }
}
