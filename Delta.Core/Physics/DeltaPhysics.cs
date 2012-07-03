using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Delta.Physics.Geometry;

namespace Delta.Physics
{
    public class DeltaPhysics : PhysicsEngine
    {
        static Color PolygonColor = Color.White;
        static Color BoundingColor = Color.DarkBlue;
        static Color ResponseColor = Color.DarkGreen;

        int _narrowDetections = 0;

        SpatialGrid _grid;
        HashSet<Polygon> _polygons;
        HashSet<Polygon> _polygonsToAdd;
        HashSet<Polygon> _polygonsToRemove;

        Stack<CollisionResult> _results;

        public DeltaPhysics()
        {
            _grid = new SpatialGrid();
            _polygons = new HashSet<Polygon>();
            _polygonsToAdd = new HashSet<Polygon>();
            _polygonsToRemove = new HashSet<Polygon>();
            _results = new Stack<CollisionResult>(10);
        }

        public override void DefineWorld(int width, int height, int size)
        {
            _grid = new SpatialGrid(width, height, size);
        }

        public override void AddCollisionPolygon(CollisionGeometry cg)
        {
            _polygonsToAdd.Add(cg.Geom);
            _grid.AddCollsionGeom(cg);
        }

        public override void AddCollisionPolygon(Entity entity, Polygon geometry)
        {
            _polygonsToAdd.Add(geometry);
            _grid.AddCollsionGeom(new CollisionGeometry(geometry));
        }

        public override void RemoveCollisionPolygon(Polygon geometry)
        {
            _polygonsToRemove.Add(geometry);
            //_grid.RemoveCollisionGeom();
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
            _results.Clear();
            _grid.Update();

            foreach (Polygon poly in _polygonsToAdd)
            {
                _polygons.Add(poly);
            }

            List<CollisionPair> pairs = _grid.GetCollisionPairs();
            _narrowDetections = pairs.Count;
            foreach (CollisionPair pair in pairs)
            {
                CollisionResult result;
                Polygon polyA, polyB;
                polyA = pair.CGA.Geom;
                polyB = pair.CGB.Geom;

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
                    polyA.Position += result.CollisionResponse;
                    // on collision events
                    if (pair.CGA.OnCollision != null)
                        pair.CGA.OnCollision(null, Vector2.Zero);
                    if (pair.CGB.OnCollision != null)
                        pair.CGB.OnCollision(null, Vector2.Zero);
                }
            }
        }

        public override void DrawDebug(ref Matrix view, ref Matrix projection)
        {
            G.PrimitiveBatch.Begin(ref projection, ref view);
            foreach (Polygon poly in _polygons)
            {
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
                G.PrimitiveBatch.DrawPolygon(poly.AABB.Vertices, poly.AABB.Vertices.Length, BoundingColor);
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
                return String.Format("Polygons:{0}\nBruteforce Checks:{1}\nSpatialGrid Checks:{2}\n", _polygons.Count, DeltaMath.Sqr(_polygons.Count), _narrowDetections);
            }
        }

    }
}
