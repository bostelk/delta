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
        public HashSet<Polygon> _polygons;
        public HashSet<Polygon> _polygonsToAdd;
        public HashSet<Polygon> _polygonsToRemove;

        public Stack<CollisionResult> _results;

        public DeltaPhysics()
        {
            _polygons = new HashSet<Polygon>();
            _polygonsToAdd = new HashSet<Polygon>();
            _polygonsToRemove = new HashSet<Polygon>();
            _results = new Stack<CollisionResult>(10);
        }

        public override void AddCollisionPolygon(Entity entity, Polygon geometry)
        {
            _polygonsToAdd.Add(geometry);
        }

        public override void RemoveCollisionPolygon(Polygon geometry)
        {
            _polygonsToRemove.Add(geometry);
        }
       
        public override void Simulate(float seconds)
        {
            foreach (Polygon poly in _polygonsToAdd)
            {
                _polygons.Add(poly);
            }
            _polygonsToAdd.Clear();
            
            foreach (Polygon poly in _polygonsToRemove)
            {
                _polygons.Remove(poly);
            }
            _polygonsToRemove.Clear();

            foreach (Polygon polya in _polygons)
            {
                foreach (Polygon polyb in _polygons)
                {
                    if (polya == polyb)
                        continue;

                    if (polya is OBB && polyb is OBB)
                    {
                        CollisionResult result = SAT.PolyPoly(polya as OBB, polyb as OBB);
                        if (result.IsColliding)
                        {
                            _results.Push(result);
                            // translate the polygon to a safe non-interecting position.
                            polya.Position += result.CollisionResponse;
                        }
                    }
                    else if (polya is Circle && polyb is Circle)
                    {
                        CollisionResult result = SAT.CircleCircle(polya as Circle, polyb as Circle);
                        if (result.IsColliding)
                        {
                            _results.Push(result);
                            // translate the polygon to a safe non-interecting position.
                            polya.Position += result.CollisionResponse;
                        }
                    }
                    else if (polya is OBB && polyb is Circle)
                    {
                        CollisionResult result = SAT.CircleBox(polyb as Circle, polya as OBB);
                        if (result.IsColliding)
                        {
                            _results.Push(result);
                            // translate the polygon to a safe non-interecting position.
                            polya.Position += result.CollisionResponse;
                        }
                    }
                    else if (polya is Circle && polyb is OBB)
                    {
                        CollisionResult result = SAT.CircleBox(polya as Circle, polyb as OBB);
                        if (result.IsColliding)
                        {
                            _results.Push(result);
                            // translate the polygon to a safe non-interecting position.
                            polya.Position += result.CollisionResponse;
                        }
                    }
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
                    G.PrimitiveBatch.DrawPolygon(box.Vertices, box.LocalVertices.Length, Color.White);
                    G.PrimitiveBatch.DrawSegment(box.Position, box.Position + box.HalfWidthX, Color.White);
                    G.PrimitiveBatch.DrawSegment(box.Position, box.Position + box.HalfWidthY, Color.White);
                }
                else if (poly is Circle)
                {
                    Circle circle = poly as Circle;
                    G.PrimitiveBatch.DrawCircle(circle.Position, circle.Radius, Color.White);
                    G.PrimitiveBatch.DrawSegment(circle.Position, circle.Position + circle.Orientation, Color.White);
                }
            }
            if (_results.Count > 0)
            {
                CollisionResult result = _results.Pop();
                G.PrimitiveBatch.DrawSegment(result.Us.Position, result.Us.Position + 10 * result.CollisionResponse, Color.Blue);
            }
            G.PrimitiveBatch.End();
        }

    }
}
