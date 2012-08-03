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

using Delta.Extensions;
using Microsoft.Xna.Framework.Input;

namespace Delta.Collision
{
    public class CollisionWorld : AbstractCollisionWorld
    {
        List<CollisionBody> _colliders;

        IBroadphase _broadphase;
        INarrowphase _narrowphase;
        bool _forceUpdateAABBs; // forces an update of all aabbs for a single frame

        public ReadOnlyCollection<CollisionBody> GlobalColliders { get { return new ReadOnlyCollection<CollisionBody>(_colliders); } }

        public CollisionWorld(INarrowphase narrowphase, IBroadphase broadphase)
        {
            _colliders = new List<CollisionBody>(200);
            _broadphase = broadphase;
            _narrowphase = narrowphase;
            _forceUpdateAABBs = true;
        }

        public void DefineWorld(int width, int height, int size)
        {
            _broadphase = new UniformGridBroadphase(width, height, size);
        }

        public override void AddCollider(CollisionBody collider)
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

        public override void RemoveCollider(CollisionBody collider)
        {
            Debug.Assert(collider != null);
            if (_colliders.Contains(collider))
            {
                _broadphase.RemoveProxy(collider.BroadphaseProxy);
                _colliders.FastRemove<CollisionBody>(collider);
                collider.OnRemoved();
                CollisionGlobals.TotalColliders--;
            }
        }

        public override void Simulate(float timeStep)
        {
            if (CanSimulate())
                InternalUpdate();
#if DEBUG
            if (G.Input.Keyboard.IsDown(Keys.LeftControl))
            {
                if (G.Input.Keyboard.IsPressed(Keys.D1))
                    CollisionGlobals.ViewGroups = CollisionGroups.Group1;
                else if (G.Input.Keyboard.IsPressed(Keys.D2))
                    CollisionGlobals.ViewGroups = CollisionGroups.Group2;
                else if (G.Input.Keyboard.IsPressed(Keys.D3))
                    CollisionGlobals.ViewGroups = CollisionGroups.Group3;
                else if (G.Input.Keyboard.IsPressed(Keys.D4))
                    CollisionGlobals.ViewGroups = CollisionGroups.Group4;
            }
#endif
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
                CollisionBody collider = _colliders[i];

                // only update an aabb if the collider has moved
                if (collider.IsAwake || _forceUpdateAABBs)
                {
                    UpdateAABB(collider);
                    _colliders[i].IsAwake = false; // HACK: collider should deactivate itself.
                    CollisionGlobals.UpdatedAABBs++;
                }
            }
            _forceUpdateAABBs = false;
        }

        protected void UpdateAABB(CollisionBody collider)
        {
            AABB aabb;
            Matrix3 worldTransform = collider.WorldTransform;
            collider.Shape.CalculateAABB(ref worldTransform, out aabb);
            _broadphase.SetProxyAABB(collider.BroadphaseProxy, ref aabb);
        }

        public override void DrawDebug(ref Matrix view, ref Matrix projection)
        {
            G.PrimitiveBatch.Begin(ref projection, ref view);
            for (int i = 0; i < _colliders.Count; i++)
            {
                CollisionBody col = _colliders[i];
                if (col.Shape == null || !col.BelongsToGroup(CollisionGlobals.ViewGroups))
                    continue;

                Matrix3 transform = col.WorldTransform;
                if (CollisionGlobals.ViewFlags.HasFlagFast(DebugViewFlags.Shape))
                {
                    Vector2[] transformedVerts = col.Shape.VerticesCopy;
                    for (int j = 0; j < transformedVerts.Length; j++)
                        transform.TransformVector(ref transformedVerts[j]);
                    G.PrimitiveBatch.DrawPolygon(transformedVerts, transformedVerts.Length, CollisionGlobals.ShapeColor);
                }

                if (CollisionGlobals.ViewFlags.HasFlagFast(DebugViewFlags.Extents))
                {
                    if (col.Shape is Box)
                    {
                        Box box = (Box)col.Shape;
                        Vector2 halfwidthX, halfwidthY;
                        box.CalculateExtents(ref transform, out halfwidthX, out halfwidthY);
                        G.PrimitiveBatch.DrawSegment(col.Position, halfwidthX, CollisionGlobals.ExtentsColor);
                        G.PrimitiveBatch.DrawSegment(col.Position, halfwidthY, CollisionGlobals.ExtentsColor);
                    }
                    else if (col.Shape is Circle)
                    {
                        Circle circle = (Circle)col.Shape;
                        Vector2 extents;
                        circle.CalculateExtents(ref transform, out extents);
                        G.PrimitiveBatch.DrawSegment(col.Position, extents, CollisionGlobals.ExtentsColor);
                    }
                }

                if (CollisionGlobals.ViewFlags.HasFlagFast(DebugViewFlags.AABB))
                {
                    AABB aabb;
                    col.Shape.CalculateAABB(ref transform, out aabb);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Min.X, aabb.Min.Y), new Vector2(aabb.Max.X, aabb.Min.Y), CollisionGlobals.BoundingColor);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Max.X, aabb.Min.Y), new Vector2(aabb.Max.X, aabb.Max.Y), CollisionGlobals.BoundingColor);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Min.X, aabb.Max.Y), new Vector2(aabb.Max.X, aabb.Max.Y), CollisionGlobals.BoundingColor);
                    G.PrimitiveBatch.DrawSegment(new Vector2(aabb.Min.X, aabb.Min.Y), new Vector2(aabb.Min.X, aabb.Max.Y), CollisionGlobals.BoundingColor);
                }
            }
            if (CollisionGlobals.ViewFlags.HasFlagFast(DebugViewFlags.CollisionResponse))
            {
                while (CollisionGlobals.Results.Count > 0)
                {
                    CollisionResult result = CollisionGlobals.Results.Pop();
                    G.PrimitiveBatch.DrawSegment(result.Us.Position, result.Us.Position + 10 * result.CollisionResponse, CollisionGlobals.ResponseColor);
                }
            }
            G.PrimitiveBatch.End();
            //G.SpriteBatch.Begin();
            //G.SpriteBatch.DrawString(G.Font, DebugInfo, new Vector2(0, 50), Color.White);
            //G.SpriteBatch.End();
        }

        public string DebugInfo
        {
            get
            {
                return String.Format("Total Colliders:{0}\nTotal Proxies:{1}\nBroadphase Collisions:{2}\nNarrowphase Collisions:{3}\nUpdated AABBs:{4}\nProxies In Cells:{5}\nViewing Collision Groups:{6}\n Viewing Flags:{7}\n", new object[] {
                    CollisionGlobals.TotalColliders, 
                    CollisionGlobals.TotalProxies,
                    CollisionGlobals.BroadphaseDetections, 
                    CollisionGlobals.NarrowphaseDetections,
                    CollisionGlobals.UpdatedAABBs,
                    CollisionGlobals.ProxiesInCells,
                    CollisionGlobals.ViewGroups,
                    CollisionGlobals.ViewFlags,
                });
            }
        }

    }
}
