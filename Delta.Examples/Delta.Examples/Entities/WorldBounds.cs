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
    public class WorldBounds : Entity
    {
        Polygon p1, p2, p3, p4;
        
        public WorldBounds()
        {
            p1 = new Polygon(new Vector2(-600, 0), new Vector2(600, 0));
            p1.Position = new Vector2(0, -320);

            p2 = new Polygon(new Vector2(-600, 0), new Vector2(600, 0));
            p2.Position = new Vector2(0, 320);

            p3 = new Polygon(new Vector2(600, -320), new Vector2(600, 320));
            p3.Position = new Vector2(-600, 0);

            p4 = new Polygon(new Vector2(-600, -320), new Vector2(-600, 320));
            p4.Position = new Vector2(600, 0);

            G.Physics.AddCollisionPolygon(this, p1);
            G.Physics.AddCollisionPolygon(this, p2);
            G.Physics.AddCollisionPolygon(this, p3);
            G.Physics.AddCollisionPolygon(this, p4);
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            // HACK: Keep them stationary for now..
            p1.Position = new Vector2(0, -320);
            p2.Position = new Vector2(0, 320);
            p3.Position = new Vector2(-600, 0);
            p4.Position = new Vector2(600, 0);
            base.LightUpdate(gameTime);
        }

    }
}
