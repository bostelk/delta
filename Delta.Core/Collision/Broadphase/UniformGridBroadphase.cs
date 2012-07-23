using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Collision
{

    /// <summary>
    /// A loose collision grid to handle broad-phase detection.
    /// </summary>
    public class UniformGridBroadphase : IBroadphase
    {
        GridCell[] _cells;
        OverlappingPairCache _pairs;
        float _invCellSize;
        int[] neighbourOffset;  

        /// <summary>
        /// In pixels.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// In pixels.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The width and height of a cell in pixels.
        /// </summary>
        public int CellSize { get; private set; }

        public int CellsWide { get; private set; }

        public int CellsHigh { get; private set; }

        public int TotalCells { get; private set; }

        public OverlappingPairCache CollisionPairs { get { return _pairs; } }

        public Point Offset;

        public UniformGridBroadphase() 
        { 
            // keep the collision from complaining if the 'world' isn't defined.
            _pairs = new OverlappingPairCache();
            _cells = new GridCell[0];
        }

        public UniformGridBroadphase(int width, int height, int cellSize) : this()
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            CellsHigh = Height / cellSize;
            CellsWide = Width / cellSize;
            TotalCells = CellsHigh * CellsWide;
            Offset = new Point(width / 2, height / 2);

            _invCellSize = 1f / (float)cellSize;
            _pairs = new OverlappingPairCache();
            _cells = new GridCell[TotalCells];

            for (int i = 0; i < _cells.Length; i++)
                _cells[i] = new GridCell();

            neighbourOffset = new int[] 
            {
                -CellsWide - 1, // top-left
                -CellsWide,     // top
                -CellsWide + 1, // top-right
                -1,             // left
                0,
                1,              // right
                CellsWide - 1,  // bottom-left
                CellsWide,      // bottom
                CellsWide + 1,  // bottom-right
            };
        }

        public void SetProxyAABB(BroadphaseProxy proxy, ref AABB aabb)
        {
            // maintain the uniform grid as the aabb moves. this works by removing
            // the stale aabb proxies and then adding the fresh aabb proxies.
            RemoveProxyFromCells(proxy);
            proxy.AABB = aabb;
            AddProxyToCells(proxy);
        }

        public void RemoveProxy(BroadphaseProxy proxy)
        {
            RemoveProxyFromCells(proxy);
        }

        public void AddProxyToCells(BroadphaseProxy proxy)
        {
            Point start = GetCellPosition(proxy.AABB.Min);
            Point end = GetCellPosition(proxy.AABB.Max);

            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
                {
                    _cells[x + y * CellsWide].AddBroadphaseProxy(proxy);
                }
            }
        }

        public void RemoveProxyFromCells(BroadphaseProxy proxy)
        {
            Point start = GetCellPosition(proxy.AABB.Min);
            Point end = GetCellPosition(proxy.AABB.Max);

            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
                {
                    _cells[x + y * CellsWide].RemoveBroadphaseProxy(proxy);
                }
            }
        }

        /// <summary>
        /// Pairs of objects that require narrow phase testing.
        /// </summary>
        /// <returns></returns>
        public void CalculateCollisionPairs() {
            GridCell cell;
            BroadphaseProxy proxyA, proxyB;

            _pairs.ClearCache();
            CollisionGlobals.ProxiesInCells = 0;
            for (int i = 0; i < _cells.Length; i++)
            {
                CollisionGlobals.ProxiesInCells += _cells[i].Proxies.Count;
                for (int n = 4; n < neighbourOffset.Length; n++)
                {
                    int ii = neighbourOffset[n] + i;
                    if (ii < 0 || ii > _cells.Length - 1)
                        continue;

                    cell = _cells[ii];

                    for (int j = 0; j < cell.Proxies.Count; j++)
                    {
                        proxyA = cell.Proxies[j];
                        for (int k = j + 1; k < cell.Proxies.Count; k++)
                        {
                            proxyB = cell.Proxies[k];
                            if (AABB.TestOverlap(proxyA.AABB, proxyB.AABB))
                                _pairs.AddOverlappingPair(proxyA, proxyB);
                        }
                    }
                }
            }
        }

        private Point GetCellPosition(Vector2 position)
        {
            Point result = Point.Zero;
            result.X = (int)((position.X + Offset.X) * _invCellSize);
            result.Y = (int)((position.Y + Offset.Y) * _invCellSize);
            result.X = result.X.Clamp(0, CellsWide - 1);
            result.Y = result.Y.Clamp(0, CellsHigh - 1);
            return result;
        }

    }
}
