using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Physics
{
    public class SpatialCell
    {
        public List<CollisionGeometry> CollisionGeoms;

        public bool Occupied
        {
            get
            {
                return CollisionGeoms.Count > 0;
            }
        }

        public SpatialCell()
        {
            CollisionGeoms = new List<CollisionGeometry>(100);
        }

        public void AddGeom(CollisionGeometry cg)
        {
            if (!CollisionGeoms.Contains(cg))
                CollisionGeoms.Add(cg);
        }

        public void RemoveGeom(CollisionGeometry cg)
        {
            CollisionGeoms.FastRemove<CollisionGeometry>(cg);
        }
    }
}
