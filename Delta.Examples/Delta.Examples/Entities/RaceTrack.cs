using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Delta;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Delta.Physics;
using Delta.Physics.Geometry;

namespace Delta.Examples.Entities
{
    public class RaceTrack : Entity
    {
        public Polygon Body { get; private set; }

        public override Vector2 Position
        {
            get
            {
                return Body.Position;
            }
            set
            {
                base.Position = value;
                Body.Position = value;
            }
        }

        public RaceTrack()
        {
            G.Physics.AddCollisionPolygon(this, Body = new Polygon(new Vector2(0, -360), new Vector2(0, 360)));
            G.Physics.AddCollisionPolygon(this, Body = new Polygon(new Vector2(-640, 0), new Vector2(0, 0)));
            Body.Position = new Vector2(100, 0);
        }

    }
}
