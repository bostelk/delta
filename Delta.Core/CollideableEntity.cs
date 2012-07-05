using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Physics;
using Microsoft.Xna.Framework;
using Delta.Physics.Geometry;
using Microsoft.Xna.Framework.Content;

namespace Delta
{
    public class CollideableEntity : TransformableEntity
    {
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
                _collider = value;
                /*
                if (value == null)
                    return;
                if (_collider != null && G.Physics != null)
                    G.Physics.RemoveColider(_collider);
                _collider = value;
                _collider.BeforeCollision = BeforeCollision;
                _collider.OnCollision = OnCollision;
                _collider.AfterCollision = AfterCollision;
                _collider.OnSeparation = OnSeparation;
                if (G.Physics != null)
                    G.Physics.AddCollider(_collider);
                */
            }
        }

        private Polygon _polygon;
        [ContentSerializer]
        public Polygon Polygon
        {
            get
            {
                return _polygon;
            }
            set
            {
                if (value == null)
                    return;
                if (_polygon == null)
                _polygon = value;
                if (_collider != null)
                {
                    _collider.Geom = value;
                    _collider.Geom.Position = base.Position;
                    _collider.Geom.Rotation = base.Rotation;
                }
            }
        }

        [ContentSerializerIgnore]
        public new Vector2 Position
        {
            get
            {
                if (Collider != null)
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
                G.Physics.AddCollider(Collider);
            }
            else if (Collider == null)
            {
                G.Physics.AddCollider(Collider);
            }
            base.LateInitialize();
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
