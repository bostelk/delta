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

        public AABB AABB;
        public Object ClientObject;

        static BroadphaseProxy()
        {
            _pool = new Pool<BroadphaseProxy>(100);
        }

        public static BroadphaseProxy Create(object client)
        {
            BroadphaseProxy proxy = _pool.Fetch();
            proxy.ClientObject = client;
            return proxy;
        }

        public BroadphaseProxy()
        {
        }

        public BroadphaseProxy(object client, AABB aabb)
        {
            ClientObject = client;
            AABB = aabb;
        }

        public void Recycle()
        {
            AABB = AABB.Zero;
            ClientObject = null;
            _pool.Release(this);
        }

    }
}
