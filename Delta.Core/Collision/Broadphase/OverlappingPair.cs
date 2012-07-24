using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    /// <summary>
    /// Represents two unique colliders found overlapping in the Broadphase. Ie. A possible collision.
    /// </summary>
    public struct OverlappingPair : IEquatable<OverlappingPair>
    {
        public static OverlappingPair EmptyPair = new OverlappingPair();

        public BroadphaseProxy ProxyA { get; internal set; }
        public BroadphaseProxy ProxyB { get; internal set; }

        public OverlappingPair(BroadphaseProxy proxyA, BroadphaseProxy proxyB): this()
        {
            ProxyA = proxyA;
            ProxyB = proxyB;
        }

        public bool Equals(OverlappingPair other)
        {
            return (ProxyA == other.ProxyA && ProxyB == other.ProxyB);
        }

        public override bool Equals(object other)
        {
            bool result = false;
            if (other is OverlappingPair)
                result = Equals((OverlappingPair)other);
            return result;
        }

        public override int GetHashCode()
        {
            return ProxyA.GetHashCode() + ProxyB.GetHashCode();
        }

        public bool IsEmpty
        {
            get
            {
                return ProxyA == null || ProxyB == null;
            }
        }
    }
}
