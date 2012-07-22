using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Collision
{
    public class GridCell
    {
        public List<BroadphaseProxy> Proxies;

        public bool Occupied
        {
            get
            {
                return Proxies.Count > 0;
            }
        }

        public GridCell()
        {
            Proxies = new List<BroadphaseProxy>(100);
        }

        public void AddBroadphaseProxy(BroadphaseProxy cg)
        {
            Proxies.Add(cg);
        }

        public void RemoveBroadphaseProxy(BroadphaseProxy cg)
        {
            Proxies.FastRemove<BroadphaseProxy>(cg);
        }
    }
}
