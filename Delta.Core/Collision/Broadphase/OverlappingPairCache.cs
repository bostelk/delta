using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace Delta.Collision
{
    public class OverlappingPairCache //: IEnumerable
    {
        internal List<OverlappingPair> _pairs = new List<OverlappingPair>();

        public OverlappingPairCache()
            : base()
        {
        }

        //public IEnumerator GetEnumerator()
        //{
        //    return _pairs.GetEnumerator();
        //}
 
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return _pairs.GetEnumerator();
        //}

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
            bool shouldCollide = proxyA.ShouldCollide(proxyB) && proxyB.ShouldCollide(proxyA);
            bool ignoreBroadphaseA = false;
            bool ignoreBroadphaseB = false;
            //if (proxyA.BeforeCollision != null)
            //    ignoreBroadphaseColA = proxyA.BeforeCollision(proxyB);
            //if (proxyB.BeforeCollision != null)
            //    ignoreBroadphaseColB = proxyB.BeforeCollision(proxyA);
            return shouldCollide && !ignoreBroadphaseA && !ignoreBroadphaseB;
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
