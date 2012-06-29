using System;
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
        public List<Polygon> _polygons;
        public List<Polygon> _polygonsToAdd;

        public Stack<CollisionResult> _results;

        public DeltaPhysics()
        {
            _polygons = new List<Polygon>(100);
            _polygonsToAdd = new List<Polygon>(100);
            _results = new Stack<CollisionResult>(10);
        }

        public override void AddCollisionPolygon(Entity entity, Polygon geometry)
        {
            _polygonsToAdd.Add(geometry);
        }
       
        public override void Simulate(float seconds)
        {
            foreach (Polygon box in _polygonsToAdd)
            {
                _polygons.Add(box);
            }
            _polygonsToAdd.Clear();

            foreach (Polygon polya in _polygons)
            {
                foreach (Polygon polyb in _polygons)
                {
                    if (polya == polyb)
                        continue;

                    if (polya is Box && polyb is Box)
                    {
                        CollisionResult result = SAT.BoxBox(polya as Box, polyb as Box);
                        if (result.IsColliding)
                        {
                            _results.Push(result);
 
                            // translate the polygon to a safe non-interecting position.
                            polya.Position += result.CollisionResponse;
                        }
                    }
                    else if (polya is Circle && polyb is Circle)
                    {
                        SAT.CircleCircle(polya as Circle, polyb as Circle);
                    }
                    else if (polya is Box && polyb is Circle)
                    {
                        SAT.CircleBox(polyb as Circle, polya as Box);
                    }
                    else if (polya is Circle && polyb is Box)
                    {
                        SAT.CircleBox(polya as Circle, polyb as Box);
                    }
                }
            }
        }

        public override void DrawDebug(ref Matrix view, ref Matrix projection)
        {
            G.PrimitiveBatch.Begin(ref projection, ref view);
            foreach (Polygon poly in _polygons)
            {
                if (poly is Box)
                {
                    Box box = poly as Box;
                    G.PrimitiveBatch.DrawPolygon(box.Verticies, box.LocalVertices.Length, Color.White);
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
