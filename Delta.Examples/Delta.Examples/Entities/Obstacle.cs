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
    public class Obstacle : TransformableEntity
    {
        const float SPEED = 50;

        public bool AutoRotate, MoveAndRotate;

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

        float _rotation;
        public override float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                Collider.Geom.Rotation = value;
            }
        }

        public Obstacle()
        {
            G.Physics.AddCollider(Collider = new Collider() 
            {
                Tag = this,
                Mass = 0f,
                Geom = new OBB(10, 10),
                //Geom = new Circle(5),
                OnCollision = OnCollision,
                BeforeCollision = BeforeCollision
            });
        }

        protected override void LateInitialize()
        {
            base.LateInitialize();

            Velocity = Vector2Extensions.RandomDirection() * SPEED;
        }

        public void SwitchBody()
        {
            if (Collider.Geom is Circle)
            {
                Collider.Geom = new OBB(10, 10);
            }
            else if (Collider.Geom is OBB)
            {
                Collider.Geom = new Circle(5);
            }

            Collider.Geom.Position = base.Position;
            Collider.Geom.Rotation = base.Rotation;
        }

        protected override void LightUpdate(GameTime gameTime)
        {
            if (AutoRotate || MoveAndRotate)
                Rotation += 0.01f;
            if (MoveAndRotate)
                Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.LightUpdate(gameTime);
        }

        public bool BeforeCollision(Collider them, Vector2 normal)
        {
            return true;
        }

        public bool OnCollision(Collider them, Vector2 normal)
        {
            Velocity = -Velocity;
            return true;
        }

        public void AfterCollision(Collider them, Vector2 normal)
        {
        }

        public void OnSeparation(Collider them)
        {
        }

    }
}
