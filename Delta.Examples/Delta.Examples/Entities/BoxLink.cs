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
    public class BoxLink : CollideableEntity
    {
        const float SPEED = 50;

        public Vector2 Input { get; set; }

        public Vector2 Velocity;

        public BoxLink()
        {
            Collider = new Collider()
            {
                Mass = 1f,
                Geom = new Circle(8)
            };
        }

        public void SwitchBody()
        {
            if (Polygon is Circle)
            {
                Polygon = new OBB(16, 16);
            }
            else if (Polygon is OBB)
            {
                Polygon = new Circle(8);
            }
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            Velocity = Input * SPEED;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.LightUpdate(gameTime);
        }

    }
}
