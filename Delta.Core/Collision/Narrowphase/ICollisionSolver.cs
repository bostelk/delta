using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    interface ICollisionSolver
    {
        CollisionResult SolveCollision(Collider shapeA, Collider shapeB);
    }
}
