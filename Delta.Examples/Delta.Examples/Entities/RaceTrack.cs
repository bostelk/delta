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
using Delta.Collision;
using Delta.Collision.Geometry;

namespace Delta.Examples.Entities
{
    public class RaceTrack : TransformableEntity
    {
        public Collider Collider { get; private set; }

        public override Vector2 Position
        {
            get
            {
                return Collider.Geom.Position;
            }
            set
            {
                base.Position = value;
                Collider.Geom.Position = value;
            }
        }

        public RaceTrack()
        {
            G.Collision.AddCollider(Collider = new Collider(new Polygon(new Vector2(0, -360), new Vector2(0, 360))));
            G.Collision.AddCollider(Collider = new Collider(new Polygon(new Vector2(-640, 0), new Vector2(0, 0))));
            Position = new Vector2(100, 0);
        }

    }
}
