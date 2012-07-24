using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    public class BruteForceBroadphase : IBroadphase
    {
        List<BroadphaseProxy> _proxies;
        OverlappingPairCache _pairs;

        public OverlappingPairCache CollisionPairs { get { return _pairs; } } 

        public BruteForceBroadphase()
        {
            _pairs = new OverlappingPairCache();
        }

        public void SetProxyAABB(BroadphaseProxy proxy, ref AABB aabb)
        {
            proxy.AABB = aabb;
        }

        public void RemoveProxy(BroadphaseProxy proxy)
        {
            _proxies.FastRemove<BroadphaseProxy>(proxy);
        }

        public void CalculateCollisionPairs()
        {
            _pairs.ClearCache();
            for (int i = 0; i < _proxies.Count; i++)
            {
                BroadphaseProxy proxyA = _proxies[i];
                for (int j = i; j < _proxies.Count; j++)
                {
                    BroadphaseProxy proxyB = _proxies[j];

                    if (AABB.TestOverlap(ref proxyA.AABB,ref proxyB.AABB))
                        _pairs.AddOverlappingPair(proxyA, proxyB);
                }
            }
        }

    }
}
