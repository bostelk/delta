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
    public class CircleObstacle : Entity
    {
        public Circle AABB { get; private set; }

        public override Vector2 Position
        {
            get
            {
                return AABB.Position;
            }
            set
            {
                AABB.Position = value;
            }
        }

        float _rotation;
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                AABB.Rotation = value;
            }
        }

        public CircleObstacle()
        {
            G.Physics.AddCollisionPolygon(this, AABB = new Circle(50));
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Matrix projection = G.World.Camera.Projection;
            Matrix view = G.World.Camera.View;
        }

    }
}
