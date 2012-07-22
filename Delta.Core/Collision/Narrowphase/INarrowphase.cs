using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    public interface INarrowphase
    {
        void SolveCollisions(OverlappingPairCache collisionPairs);
    }
}
