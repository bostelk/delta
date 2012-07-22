using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    [Flags]
    public enum DebugViewFlags {
        None = 1 << 0,
        AABB = 1 << 1,
        Shape = 1 << 2,
        CollisionResponse = 1 << 3,
        All = AABB | Shape | CollisionResponse
    }

    public static class CollisionGlobals
    {
        public static Color PolygonColor = Color.White;
        public static Color BoundingColor = Color.Blue;
        public static Color ExtentsColor = Color.Green;
        public static Color ResponseColor = Color.Red;

        public static int TotalColliders;
        public static int TotalProxies;
        public static int BroadphaseDetections;
        public static int NarrowphaseDetections;
        public static int UpdatedAABBs;
        public static int ProxiesInCells;

        public static Stack<CollisionResult> Results;

        public static DebugViewFlags DebugViewOptions = DebugViewFlags.All;
    }
}
