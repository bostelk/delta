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
    public class BoxLink : TransformableEntity
    {
        const float SPEED = 250;
        const float ROTATION_SPEED = 200;

        public Collider Collider { get; private set; }

        public Vector2 Input { get; set; }

        public Vector2 Velocity;

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

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                base.Rotation = value;
                Collider.Geom.Rotation = value;
            }
        }

        public BoxLink()
        {
            G.Physics.AddCollider(Collider = new Collider()
            {
                Geom = new OBB(16, 16)
            });
        }

        public void SwitchBody()
        {
            if (Collider.Geom is Circle)
            {
                Collider.Geom = new OBB(16, 16);
            }
            else if (Collider.Geom is OBB)
            {
                Collider.Geom = new Circle(16);
            }

            Collider.Geom.Position = base.Position;
            Collider.Geom.Rotation = Rotation;
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
