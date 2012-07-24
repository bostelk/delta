using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    interface ICollideable
    {
        bool IsAwake { get; set; }
        bool RemoveNextUpdate { get; set; }
        CollisionShape Shape { get; set; }
        BroadphaseProxy BroadphaseProxy { get; set; }
        Matrix2D WorldTransform { get; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }

        void OnRemoved();
        void OnAdded();
    }
}
