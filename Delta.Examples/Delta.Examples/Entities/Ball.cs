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
    public class Ball : TransformableEntity
    {
        const float SPEED = 50;

        public Polygon Body { get; private set; }

        public Vector2 Input { get; set; }

        public Vector2 Velocity;

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
                Body.Rotation = value;
            }
        }

        public Ball()
        {
            Body = new OBB(10, 10);
            G.Physics.AddCollisionPolygon(new CollisionGeometry() {
                Tag = this,
                Geom = Body,
                OnCollision = OnCollision,
                BeforeCollision = BeforeCollision
            });
        }

        protected override void LateInitialize()
        {
            base.LateInitialize();

            Velocity = Vector2Extensions.RandomDirection() * SPEED;
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            Rotation += 0.01f;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.LightUpdate(gameTime);
        }

        public bool BeforeCollision(Entity them, Vector2 normal)
        {
            return true;
        }

        public bool OnCollision(Entity them, Vector2 normal)
        {
            Velocity = -Velocity;
            return true;
        }

        public void AfterCollisionEventHandler(Entity them, Vector2 normal)
        {
        }

        public void OnSeparationEventHandler(Entity them)
        {
        }

    }
}
