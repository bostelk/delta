using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Delta.Collision;

namespace Delta.Examples.Entities
{
    public class Obstacle : TransformableEntity
    {

        public Obstacle()
        {
        }

        protected override void Initialize()
        {
            WrappedBody = CollisionBody.CreateBody(new Box(Size.X, Size.Y));
            WrappedBody.SetGroup(CollisionGroups.Group2);
            WrappedBody.CollidesWithGroup(CollisionGroups.Group1);
            Position += Size / 2; // correct for tiled's top-left position.
            base.Initialize();
        }

    }
}
