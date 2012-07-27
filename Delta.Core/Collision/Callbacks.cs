using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
   
    public delegate bool BeforeCollisionEventHandler(CollisionBody them);

    public delegate bool OnCollisionEventHandler(CollisionBody them, Vector2 normal);

    public delegate bool OnTopCollisionEventHandler(CollisionBody them, Vector2 normal);
    public delegate bool OnBottomCollisionEventHandler(CollisionBody them, Vector2 normal);
    public delegate bool OnLeftCollisionEventHandler(CollisionBody them, Vector2 normal);
    public delegate bool OnRightCollisionEventHandler(CollisionBody them, Vector2 normal);

    public delegate void AfterCollisionEventHandler(CollisionBody them, Vector2 normal);

    public delegate void OnSeparationEventHandler(CollisionBody them);

}
