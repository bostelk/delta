using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Delta.Physics.Geometry;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Physics
{
    public struct CollisionPair
    {
        public CollisionGeometry CGA;
        public CollisionGeometry CGB;
    }

    /// <summary>
    /// A loose collision grid to handle broad-phase detection.
    /// </summary>
    public class SpatialGrid : SpatialStructure
    {
        SpatialCell[] _cells;
        List<CollisionPair> _pairs;
        List<CollisionGeometry> _cgs;
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

        public Point Offset;

        public SpatialGrid() { }

        public SpatialGrid(int width, int height, int cellSize)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            CellsHigh = Height / cellSize;
            CellsWide = Width / cellSize;
            TotalCells = CellsHigh * CellsWide;
            Offset = new Point(width / 2, height / 2);

            _invCellSize = 1 / cellSize;
            _cgs = new List<CollisionGeometry>(100);
            _pairs = new List<CollisionPair>(100);
            _cells = new SpatialCell[TotalCells];

            for (int i = 0; i < _cells.Length; i++)
                _cells[i] = new SpatialCell();

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

        private void ClearGrid()
        {
            for (int i = 0; i < _cells.Length; i++)
                _cells[i].CollisionGeoms.Clear();
        }

        private Point GetCellPosition(Vector2 position)
        {
            Point result = Point.Zero;
            result.X = (int)(position.X + Offset.X) / CellSize;
            result.Y = (int)(position.Y + Offset.Y) / CellSize;
            result.X = DeltaMath.Clamp(result.X, 0, CellsWide - 1);
            result.Y = DeltaMath.Clamp(result.Y, 0, CellsHigh - 1);
            return result;
        }

        public void AddCollsionGeom(CollisionGeometry cg)
        {
            Point start = GetCellPosition(cg.Geom.AABB.Min);
            Point end = GetCellPosition(cg.Geom.AABB.Max);

            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
                {
                    _cells[x + y * CellsWide].AddGeom(cg);
                }
            }
            if (!_cgs.Contains(cg))
                _cgs.Add(cg);
        }

        public void RemoveCollisionGeom(CollisionGeometry cg)
        {
            Point start = GetCellPosition(cg.Geom.AABB.Min);
            Point end = GetCellPosition(cg.Geom.AABB.Max);

            for (int y = start.Y; y < end.Y; y++)
            {
                for (int x = start.X; x < end.X; x++)
                {
                    _cells[x + y * CellsWide].RemoveGeom(cg);
                }
            }
            _cgs.FastRemove<CollisionGeometry>(cg);
        }

        /// <summary>
        /// Pairs of objects that require narrow phase testing.
        /// </summary>
        /// <returns></returns>
        public List<CollisionPair> GetCollisionPairs() {
            SpatialCell cell;
            CollisionGeometry cga, cgb;

            _pairs.Clear();
            for (int i = 0; i < _cells.Length; i++)
            {
                for (int n = 4; n < neighbourOffset.Length; n++)
                {
                    int ii = neighbourOffset[n] + i;
                    if (ii < 0 || ii > _cells.Length - 1)
                        continue;

                    cell = _cells[ii];

                    for (int j = 0; j < cell.CollisionGeoms.Count; j++)
                    {
                        cga = cell.CollisionGeoms[j];
                        for (int k = j + 1; k < cell.CollisionGeoms.Count; k++)
                        {
                            cgb = cell.CollisionGeoms[k];
                            if (AABB.TestOverlap(cga.Geom.AABB, cgb.Geom.AABB))
                            {
                                _pairs.Add(new CollisionPair()
                                {
                                    CGA = cga,
                                    CGB = cgb
                                });

                                if (cga.BeforeCollision != null)
                                    cga.BeforeCollision(null, Vector2.Zero);
                                if (cgb.BeforeCollision != null)
                                    cgb.BeforeCollision(null, Vector2.Zero);
                            }
                        }
                    }
                }
            }

            return _pairs;
        }

        public void Update()
        {
            ClearGrid();

            for (int i = 0; i < _cgs.Count; i++)
            {
                AddCollsionGeom(_cgs[i]);
            }
        }

        public override void DrawDebug(ref Matrix view, ref Matrix projection)
        {
            /*
            G.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, view);
            G.SpriteBatch.dr
            G.SpriteBatch.End();
            */
        }
    }
}
