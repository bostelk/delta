using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    public class BroadphaseProxy : Poolable
    {
        static CollisionGroups _defaultGroup = CollisionGroups.Group1; // belong to group 1.
        static CollisionGroups _defaultMask = CollisionGroups.All; // collide with all groups.

        public AABB AABB;
        public CollisionGroups CollisionFilterGroup;
        public CollisionGroups CollisionFilterMask;
        public Func<object, bool> NeedsCollisionWith;
        public object ClientObject;

        public BroadphaseProxy()
            : base()
        {
        }

        public static BroadphaseProxy Create(object client)
        {
            return Create(client, _defaultGroup, _defaultMask);
        }

        public static BroadphaseProxy Create(object client, CollisionGroups group, CollisionGroups mask)
        {
            BroadphaseProxy proxy = Pool.Fetch<BroadphaseProxy>();
            proxy.ClientObject = client;
            proxy.CollisionFilterGroup = group;
            proxy.CollisionFilterMask = mask;
            CollisionGlobals.TotalProxies++;
            return proxy;
        }

        protected override void Recycle(bool isReleasing)
        {
            AABB = AABB.Zero;
            ClientObject = null;
            CollisionFilterGroup = CollisionGroups.None;
            CollisionFilterMask = CollisionGroups.None;
            if (isReleasing)
                CollisionGlobals.TotalProxies--;
            base.Recycle(isReleasing);
        }

        public bool ShouldCollide(BroadphaseProxy other)
        {
            return (CollisionFilterGroup & other.CollisionFilterMask) !=  CollisionGroups.None;
        }

    }
}
