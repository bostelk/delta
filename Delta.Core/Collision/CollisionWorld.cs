using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Delta.Collision
{
    public class CollisionWorld : AbstractCollisionWorld
    {
        List<Collider> _colliders;

        IBroadphase _broadphase;
        INarrowphase _narrowphase;
        bool _forceUpdateAABBs; // forces an update of all aabbs for a single frame

        public ReadOnlyCollection<Collider> GlobalColliders { get { return new ReadOnlyCollection<Collider>(_colliders); } }

        public CollisionWorld(INarrowphase narrowphase, IBroadphase broadphase)
        {
            _colliders = new List<Collider>(200);
            _broadphase = broadphase;
            _narrowphase = narrowphase;
            _forceUpdateAABBs = true;
        }

        public void DefineWorld(int width, int height, int size)
        {
            _broadphase = new UniformGridBroadphase(width, height, size);
        }

        public override void AddCollider(Collider collider)
        {
            Debug.Assert(collider != null);
            if (!_colliders.Contains(collider))
            {
                _colliders.Add(collider);
                collider.OnAdded();

                // add broadphase proxy
                collider.BroadphaseProxy = BroadphaseProxy.Create(collider);
                UpdateAABB(collider);
                CollisionGlobals.TotalColliders++;
            }
        }

        public override void RemoveCollider(Collider collider)
        {
            Debug.Assert(collider != null);
            if (_colliders.Contains(collider))
            {
                _colliders.FastRemove<Collider>(collider);
                _broadphase.RemoveProxy(collider.BroadphaseProxy);
                collider.OnRemoved();
                CollisionGlobals.TotalColliders--;
            }
        }

        public override void Simulate(float timeStep)
        {
            if (CanSimulate())
                InternalUpdate();
            CollisionGlobals.BroadphaseDetections = _broadphase.CollisionPairs.Count();
        }

        protected bool CanSimulate()
        {
            return _broadphase != null && _narrowphase != null;
        }

        protected void InternalUpdate()
        {
            // make sure bounding-boxes match the Collider's world position, scale, and rotation.
            UpdateAABBs();
            // perofrm a coarse-grain detection to find potential collision pairs.
            _broadphase.CalculateCollisionPairs();
            // perform a finer-grain detection to find collisions and resolve them.
            _narrowphase.SolveCollisions(_broadphase.CollisionPairs);
        }

        protected void UpdateAABBs()
        {
            CollisionGlobals.UpdatedAABBs = 0;
            for (int i = 0; i < _colliders.Count; i++)
            {
                Collider collider = _colliders[i];

                // only update an aabb if the collider has moved
                if (collider.IsActive || _forceUpdateAABBs)
                {
                    UpdateAABB(collider);
                    _colliders[i].IsActive = false; // HACK: collider should deactivate itself.
                    CollisionGlobals.UpdatedAABBs++;
                }
            }
            _forceUpdateAABBs = false;
        }

        protected void UpdateAABB(Collider collider)
        {
            AABB aabb;
            Transform worldTransform = collider.WorldTransform;
            collider.Shape.CalculateAABB(ref worldTransform, out aabb);
            _broadphase.SetProxyAABB(collider.BroadphaseProxy, ref aabb);
        }

        public override void DrawDebug(ref Matrix view, ref Matrix projection)
        {
            G.PrimitiveBatch.Begin(ref projection, ref view);
            for(int i = 0; i < _colliders.Count; i++)
            {
                Collider col = _colliders[i];
                if (col.Shape == null) 
                    continue;

                if (CollisionGlobals.DebugViewOptions.HasFlag(DebugViewFlags.Shape))
                {
                    if (col.Shape is Box)
                    {
                        Box box = (Box)col.Shape;
                        Vector2 halfwidthX, halfwidthY;
                        Transform transform = col.WorldTransform;
                        box.CalculateExtents(ref transform, out halfwidthX, out halfwidthY);
                        G.PrimitiveBatch.DrawSegment(col.Position, col.Position + halfwidthX, CollisionGlobals.ExtentsColor);
                        G.PrimitiveBatch.DrawSegment(col.Position, col.Position + halfwidthY, CollisionGlobals.ExtentsColor);
                    }
                    else if (col.Shape is Circle)
                    {
                        Circle circle = (Circle)col.Shape;
                        Vector2 extents;
                        Transform transform = col.WorldTransform;
                        circle.CalculateExtents(ref transform, out extents);
                        G.PrimitiveBatch.DrawCircle(col.Position, circle.Radius, CollisionGlobals.PolygonColor);
                        G.PrimitiveBatch.DrawSegment(col.Position, col.Position + extents, CollisionGlobals.ExtentsColor);
                    }
                    else
                    {
                        Polygon poly = (Polygon)col.Shape;
                        G.PrimitiveBatch.DrawPolygon(poly.Vertices, poly.Vertices.Length, CollisionGlobals.PolygonColor);
                    }
                }

                if (CollisionGlobals.DebugViewOptions.HasFlag(DebugViewFlags.AABB))
                {
                    AABB aabb;
                    Transform worldTransform = col.WorldTransform;
                    col.Shape.CalculateAABB(ref worldTransform, out aabb);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Min.X, aabb.Min.Y), new Vector2(aabb.Max.X, aabb.Min.Y), CollisionGlobals.BoundingColor);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Max.X, aabb.Min.Y), new Vector2(aabb.Max.X, aabb.Max.Y), CollisionGlobals.BoundingColor);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Min.X, aabb.Max.Y), new Vector2(aabb.Max.X, aabb.Max.Y), CollisionGlobals.BoundingColor);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Min.X, aabb.Min.Y), new Vector2(aabb.Min.X, aabb.Max.Y), CollisionGlobals.BoundingColor);
                }
            }
            if (CollisionGlobals.DebugViewOptions.HasFlag(DebugViewFlags.CollisionResponse) && CollisionGlobals.Results.Count > 0)
            {
                while (CollisionGlobals.Results.Count > 0)
                {
                    CollisionResult result = CollisionGlobals.Results.Pop();
                    G.PrimitiveBatch.DrawSegment(result.Us.Position, result.Us.Position + 10 * result.CollisionResponse, CollisionGlobals.ResponseColor);
                }
            }
            G.PrimitiveBatch.End();

            G.SpriteBatch.Begin();
            G.SpriteBatch.DrawString(G.Font, DebugInfo, new Vector2(0, 50), Color.White);
            G.SpriteBatch.End();
        }

        public string DebugInfo
        {
            get
            {
                return String.Format("Total Colliders:{0}\nTotal Proxies:{1}\nBroadphase Collisions:{2}\nNarrowphase Collisions:{3}\nUpdated AABBs:{4}\nProxies In Cells:{5}\n", new object[] {
                    CollisionGlobals.TotalColliders, 
                    CollisionGlobals.TotalProxies,
                    CollisionGlobals.BroadphaseDetections, 
                    CollisionGlobals.NarrowphaseDetections,
                    CollisionGlobals.UpdatedAABBs,
                    CollisionGlobals.ProxiesInCells,
                });
            }
        }

    }
}
