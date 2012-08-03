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
        Extents = 1 << 3,
        CollisionResponse = 1 << 4,
        All = AABB | Shape | Extents | CollisionResponse
    }

    public static class CollisionGlobals
    {
        public static Color ShapeColor = Color.White;
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

        public static DebugViewFlags ViewFlags = DebugViewFlags.All;
        public static CollisionGroups ViewGroups = CollisionGroups.All;
    }

}
