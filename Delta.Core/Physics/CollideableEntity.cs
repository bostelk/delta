//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Delta.Collision;
//using Microsoft.Xna.Framework;
//using Delta.Collision.Geometry;
//using Microsoft.Xna.Framework.Content;

//namespace Delta
//{
//    public class CollideableEntity : TransformableEntity
//    {
//        [ContentSerializer]
//        public bool ColliderEnabled = true;

//        public CollideableEntity()
//        {
//        }

//        protected override void LateInitialize()
//        {
//            if (Collider == null && _polygon != null)
//            {
//                Collider = new Collider(this, _polygon);
//            }
//            base.LateInitialize();
//        }

//        protected internal virtual void OnColliderChanged()
//        {
//            if (_collider != null && G.Collision != null)
//                G.Collision.RemoveColider(_collider);
//            _collider.BeforeCollision = BeforeCollision;
//            _collider.OnCollision = OnCollision;
//            _collider.AfterCollision = AfterCollision;
//            _collider.OnSeparation = OnSeparation;
//            Polygon = _collider.Geom;
//            if (G.Collision != null)
//                G.Collision.AddCollider(_collider);
//        }

//        protected internal virtual void OnPolygonChanged()
//        {
//            if (_collider != null)
//            {
//                _collider.Geom = _polygon;
//                _collider.Geom.Position = base.Position;
//                _collider.Geom.Rotation = base.Rotation;
//            }
//        }

//        public virtual bool BeforeCollision(Collider them, Vector2 normal)
//        {
//            return true;
//        }

//        public virtual bool OnCollision(Collider them, Vector2 normal)
//        {
//            return true;
//        }

//        public virtual void AfterCollision(Collider them, Vector2 normal)
//        {
//        }

//        public virtual void OnSeparation(Collider them)
//        {
//        }
//    }
//}
