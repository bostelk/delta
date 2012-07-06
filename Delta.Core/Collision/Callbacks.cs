using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
       public delegate bool BeforeCollisionEventHandler(Collider them, Vector2 normal);

    public delegate bool OnCollisionEventHandler(Collider them, Vector2 normal);

    public delegate bool OnTopCollisionEventHandler(Collider them, Vector2 normal);
    public delegate bool OnBottomCollisionEventHandler(Collider them, Vector2 normal);
    public delegate bool OnLeftCollisionEventHandler(Collider them, Vector2 normal);
    public delegate bool OnRightCollisionEventHandler(Collider them, Vector2 normal);

    public delegate void AfterCollisionEventHandler(Collider them, Vector2 normal);

    public delegate void OnSeparationEventHandler(Collider them);

}
