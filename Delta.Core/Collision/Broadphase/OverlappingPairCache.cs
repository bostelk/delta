using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Delta.Collision
{
    public class OverlappingPairCache
    {
        HashSet<OverlappingPair> _pairs;

        /// <summary>
        /// lazy hack: should implement IEnumerable...
        /// </summary>
        public HashSet<OverlappingPair> hashset
        {
            get
            {
                return _pairs;
            }
        }

        public OverlappingPairCache()
        {
            _pairs = new HashSet<OverlappingPair>();
        }

        public OverlappingPair AddOverlappingPair(BroadphaseProxy proxyA, BroadphaseProxy proxyB) 
        {
            // do not form a pair with yourself and ignore null proxies!
            if (proxyA == proxyB || proxyA == null || proxyB == null)
                return OverlappingPair.EmptyPair;
            else if (!NeedsBroadphaseCollision(proxyA, proxyB))
                return OverlappingPair.EmptyPair;
            else
                return InternalAddPair(proxyA, proxyB);
        }

        private bool NeedsBroadphaseCollision(BroadphaseProxy proxyA, BroadphaseProxy proxyB) 
        {
            bool shouldCollide = true; // proxyA and proxyB should collide with each other (same ground or wtvr).
            bool ignoreBroadphaseColA = false;
            bool ignoreBroadphaseColB = false;
            //if (proxyA.BeforeCollision != null)
            //    ignoreBroadphaseColA = proxyA.BeforeCollision(proxyB);
            //if (proxyB.BeforeCollision != null)
            //    ignoreBroadphaseColB = proxyB.BeforeCollision(proxyA);
            return shouldCollide && !ignoreBroadphaseColA && !ignoreBroadphaseColB;
        }

        private OverlappingPair InternalAddPair(BroadphaseProxy proxyA, BroadphaseProxy proxyB) 
        {
            // order proxies
            OverlappingPair newPair = new OverlappingPair(proxyA, proxyB);

            // we only want unique pairs. measure twice, check once.
            if (!_pairs.Contains(newPair))
            {
                _pairs.Add(newPair);
                return newPair;
            }
            else
            {
                return OverlappingPair.EmptyPair;
            }
        }

        public void ClearCache()
        {
            _pairs.Clear();
        }

        public int Count()
        {
            return _pairs.Count;
        }
    }
}
