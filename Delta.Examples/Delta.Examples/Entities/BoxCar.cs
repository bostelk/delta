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
    public class BoxCar : Entity
    {
        const float SPEED = 250;
        const float ROTATION_SPEED = 200;

        public Box OBB { get; private set; }

        public Vector2 Input { get; set; }

        public Vector2 Velocity;

        public override Vector2 Position
        {
            get
            {
                return OBB.Position;
            }
            set
            {
                OBB.Position = value;
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
                OBB.Rotation = value;
            }
        }

        public BoxCar()
        {
            G.Physics.AddCollisionPolygon(this, OBB = new Box(50, 100));
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            if (Input.X < 0)
                Rotation = MathHelper.SmoothStep(Rotation, Rotation - ROTATION_SPEED, 0.01f);
            if (Input.X > 0)
                Rotation = MathHelper.SmoothStep(Rotation, Rotation + ROTATION_SPEED, 0.01f);

            if (Input != Vector2.Zero)
            {
                float speedX = (float)Math.Cos(Rotation + Math.PI / 2) * SPEED * Input.Y;
                float speedY = (float)Math.Sin(Rotation + Math.PI / 2) * SPEED * Input.Y;
                Velocity.X = MathHelper.SmoothStep(Velocity.X, speedX, 0.3f);
                Velocity.Y = MathHelper.SmoothStep(Velocity.Y, speedY, 0.3f);
            }
            else
            {
                Velocity.X = MathHelper.SmoothStep(Velocity.X, 0, 0.1f);
                Velocity.Y = MathHelper.SmoothStep(Velocity.Y, 0, 0.1f);
            }
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.LightUpdate(gameTime);
        }

    }
}
