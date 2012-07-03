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
    public class BoxLink : Entity
    {
        const float SPEED = 250;
        const float ROTATION_SPEED = 200;

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

        public BoxLink()
        {
            //G.Physics.AddCollisionPolygon(this, Body = new Circle(8));
            G.Physics.AddCollisionPolygon(this, Body = new OBB(16, 16));
        }

        public void SwitchBody()
        {
            if (Body is Circle)
            {
                G.Physics.RemoveCollisionPolygon(Body);
                G.Physics.AddCollisionPolygon(this, Body = new OBB(16, 16));
            }
            else if (Body is OBB)
            {
                G.Physics.RemoveCollisionPolygon(Body);
                G.Physics.AddCollisionPolygon(this, Body = new Circle(16));
            }

            Body.Position = base.Position;
            Body.Rotation = Rotation;
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
