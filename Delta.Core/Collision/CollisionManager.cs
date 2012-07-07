using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Delta.Collision.Geometry;
using System.Collections.Generic;

namespace Delta.Collision
{
    public class CollisionManager : CollisionEngine
    {
        static Color PolygonColor = Color.White;
        static Color BoundingColor = Color.DarkBlue;
        static Color ResponseColor = Color.DarkGreen;

        int _narrowDetections = 0;

        SpatialGrid _grid;
        List<Collider> _colliders;
        List<Collider> _collidersToAdd;
        List<Collider> _collidersToRemove;

        Stack<CollisionResult> _results;

        public CollisionManager()
        {
            _colliders = new List<Collider>(200);
            _collidersToAdd = new List<Collider>(50);
            _collidersToRemove = new List<Collider>(50);
            _results = new Stack<CollisionResult>(10);
        }

        /// <summary>
        /// Define the SpatialGrid dimensions.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="size">Cell size in pixels.</param>
        public override void DefineWorld(int width, int height, int size)
        {
            _grid = new SpatialGrid(width, height, size);
        }

        public override void AddCollider(Collider collider)
        {
            if (!_colliders.Contains(collider))
            {
                _collidersToAdd.Add(collider);
            }
        }

        public override void RemoveColider(Collider collider)
        {
            if (_colliders.Contains(collider))
            {
                _collidersToRemove.Add(collider);
            }
        }

        public override List<Polygon> Raycast(Vector2 start, Vector2 end, bool returnFirst)
        {
            throw new NotImplementedException();
        }

        public override List<Polygon> InArea(Rectangle area)
        {
            throw new NotImplementedException();
        }

        public override void Simulate(float seconds)
        {
            if (_grid == null)
                return;

            foreach (Collider collider in _collidersToAdd)
            {
                _colliders.Add(collider);
            }
            foreach (Collider collider in _collidersToRemove)
            {
                _colliders.Remove(collider);
            }
            _collidersToAdd.Clear();
            _collidersToRemove.Clear();
 
            _results.Clear();
            _grid.Update(_colliders);
                              
            _narrowDetections = 0;
            List<CollisionPair> pairs = _grid.GetCollisionPairs();
            foreach (CollisionPair pair in pairs)
            {
                if (pair.ColliderA.Mass == 0 && pair.ColliderB.Mass == 0)
                    continue;
                _narrowDetections++;

                CollisionResult result;
                Polygon polyA, polyB;
                polyA = pair.ColliderA.Geom;
                polyB = pair.ColliderB.Geom;

                if (polyA is OBB && polyB is OBB)
                {
                    result = SAT.BoxBox(polyA as OBB, polyB as OBB);
                }
                else if (polyA is Circle && polyB is Circle)
                {
                    result = SAT.CircleCircle(polyA as Circle, polyB as Circle);
                }
                else if (polyA is OBB && polyB is Circle)
                {
                    result = SAT.CircleBox(polyB as Circle, polyA as OBB);
                }
                else if (polyA is Circle && polyB is OBB)
                {
                    result = SAT.CircleBox(polyA as Circle, polyB as OBB);
                }
                else
                {
                    result = SAT.PolyPoly(polyA, polyB);
                }

                if (result.IsColliding)
                {
                    _results.Push(result);

                    // translate the polygon to a safe non-interecting position.
                    result.Us.Position += pair.ColliderA.Mass * result.CollisionResponse;
                    result.Them.Position += -pair.ColliderB.Mass * result.CollisionResponse;
                    // on collision events
                    if (pair.ColliderA.OnCollision != null)
                        pair.ColliderA.OnCollision(pair.ColliderB, Vector2.Zero);
                    if (pair.ColliderB.OnCollision != null)
                        pair.ColliderB.OnCollision(pair.ColliderA, Vector2.Zero);
                }
            }
        }

        public override void DrawDebug(ref Matrix view, ref Matrix projection)
        {
            G.PrimitiveBatch.Begin(ref projection, ref view);
            foreach (Collider collider in _colliders)
            {
                Polygon poly = collider.Geom;
                if (poly is OBB)
                {
                    OBB box = poly as OBB;
                    G.PrimitiveBatch.DrawSegment(box.Position, box.Position + box.HalfWidthX, PolygonColor);
                    G.PrimitiveBatch.DrawSegment(box.Position, box.Position + box.HalfWidthY, PolygonColor);
                }
                else if (poly is Circle)
                {
                    Circle circle = poly as Circle;
                    G.PrimitiveBatch.DrawCircle(circle.Position, circle.Radius, PolygonColor);
                    G.PrimitiveBatch.DrawSegment(circle.Position, circle.Position + circle.Orientation, PolygonColor);
                }
#if DEBUG
                //G.PrimitiveBatch.DrawCircle(poly.Position, poly.BoundingRadius, BoundingColor);
                G.PrimitiveBatch.DrawPolygon(poly.AABB.Vertices, 4, BoundingColor);
#endif
                G.PrimitiveBatch.DrawPolygon(poly.Vertices, poly.Vertices.Length, PolygonColor);
            }
            if (_results.Count > 0)
            {
                CollisionResult result = _results.Pop();
                G.PrimitiveBatch.DrawSegment(result.Us.Position, result.Us.Position + 10 * result.CollisionResponse, ResponseColor);
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
                return String.Format("Polygons:{0}\nBruteforce Checks:{1}\nSpatialGrid Checks:{2}\n", _colliders.Count, _colliders.Count.Square(), _narrowDetections);
            }
        }

    }
}
