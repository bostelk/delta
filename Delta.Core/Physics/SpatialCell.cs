using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Physics
{
    public class SpatialCell
    {
        public List<Collider> Colliders;

        public bool Occupied
        {
            get
            {
                return Colliders.Count > 0;
            }
        }

        public SpatialCell()
        {
            Colliders = new List<Collider>(100);
        }

        public void AddCollider(Collider cg)
        {
            if (!Colliders.Contains(cg))
                Colliders.Add(cg);
        }

        public void RemoveCollider(Collider cg)
        {
            Colliders.FastRemove<Collider>(cg);
        }
    }
}
