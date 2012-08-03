using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delta.Structures;

namespace Delta.Collision
{
    public class BroadphaseProxy : IRecyclable
    {
        static Pool<BroadphaseProxy> _pool;
        static CollisionGroups _defaultGroup = CollisionGroups.Group1; // belong to group 1.
        static CollisionGroups _defaultMask = CollisionGroups.All; // collide with all groups.

        public AABB AABB;
        public CollisionGroups CollisionFilterGroup;
        public CollisionGroups CollisionFilterMask;
        public Func<object, bool> NeedsCollisionWith;
        public object ClientObject;

        static BroadphaseProxy()
        {
            _pool = new Pool<BroadphaseProxy>(100);
        }

        public static BroadphaseProxy Create(object client)
        {
            return Create(client, _defaultGroup, _defaultMask);
        }

        public static BroadphaseProxy Create(object client, CollisionGroups group, CollisionGroups mask)
        {
            BroadphaseProxy proxy = _pool.Fetch();
            proxy.ClientObject = client;
            proxy.CollisionFilterGroup = group;
            proxy.CollisionFilterMask = mask;
            CollisionGlobals.TotalProxies++;
            return proxy;
        }

        public BroadphaseProxy() { }

        public BroadphaseProxy(object client, AABB aabb)
        {
            ClientObject = client;
            AABB = aabb;
        }

        public bool ShouldCollide(BroadphaseProxy other)
        {
            return (CollisionFilterGroup & other.CollisionFilterMask) !=  CollisionGroups.None;
        }

        public void Recycle()
        {
            AABB = AABB.Zero;
            ClientObject = null;
            CollisionFilterGroup = CollisionGroups.None;
            CollisionFilterMask = CollisionGroups.None;

            CollisionGlobals.TotalProxies--;
            _pool.Release(this);
        }

    }
}
