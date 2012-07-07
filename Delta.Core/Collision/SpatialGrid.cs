using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;
using Delta.Collision.Geometry;
using Microsoft.Xna.Framework.Graphics;

namespace Delta.Collision
{
    public struct CollisionPair
    {
        public Collider ColliderA;
        public Collider ColliderB;
    }

    /// <summary>
    /// A loose collision grid to handle broad-phase detection.
    /// </summary>
    public class SpatialGrid : SpatialStructure
    {
        SpatialCell[] _cells;
        List<CollisionPair> _pairs;
        List<Collider> _collidersGlobal;
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

        public Dictionary<Collider, bool> _tested;

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
            _collidersGlobal = new List<Collider>(100);
            _pairs = new List<CollisionPair>(100);
            _tested = new Dictionary<Collider, bool>(100);
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
                _cells[i].Colliders.Clear();
        }

        private Point GetCellPosition(Vector2 position)
        {
            Point result = Point.Zero;
            result.X = (int)(position.X + Offset.X) / CellSize;
            result.Y = (int)(position.Y + Offset.Y) / CellSize;
            result.X = result.X.Clamp(0, CellsWide - 1);
            result.Y = result.Y.Clamp(0, CellsHigh - 1);
            return result;
        }

        public void AddColliderToCells(Collider collider)
        {
            Point start = GetCellPosition(collider.Geom.AABB.Min);
            Point end = GetCellPosition(collider.Geom.AABB.Max);

            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
                {
                    _cells[x + y * CellsWide].AddCollider(collider);
                }
            }
        }

        public void RemoveColliderFromCells(Collider collider)
        {
            Point start = GetCellPosition(collider.Geom.AABB.Min);
            Point end = GetCellPosition(collider.Geom.AABB.Max);

            for (int y = start.Y; y < end.Y; y++)
            {
                for (int x = start.X; x < end.X; x++)
                {
                    _cells[x + y * CellsWide].RemoveCollider(collider);
                }
            }
        }

        /// <summary>
        /// Pairs of objects that require narrow phase testing.
        /// </summary>
        /// <returns></returns>
        public List<CollisionPair> GetCollisionPairs() {
            SpatialCell cell;
            Collider colliderA, colliderB;

            _pairs.Clear();
            for (int i = 0; i < _cells.Length; i++)
            {
                for (int n = 4; n < neighbourOffset.Length; n++)
                {
                    int ii = neighbourOffset[n] + i;
                    if (ii < 0 || ii > _cells.Length - 1)
                        continue;

                    cell = _cells[ii];

                    for (int j = 0; j < cell.Colliders.Count; j++)
                    {
                        colliderA = cell.Colliders[j];
                        for (int k = j + 1; k < cell.Colliders.Count; k++)
                        {
                            colliderB = cell.Colliders[k];
                            if (AABB.TestOverlap(colliderA.Geom.AABB, colliderB.Geom.AABB))
                            {
                                _pairs.Add(new CollisionPair()
                                {
                                    ColliderA = colliderA,
                                    ColliderB = colliderB
                                });

                                if (colliderA.BeforeCollision != null)
                                    colliderA.BeforeCollision(colliderB, Vector2.Zero);
                                if (colliderB.BeforeCollision != null)
                                    colliderB.BeforeCollision(colliderA, Vector2.Zero);
                            }
                        }
                    }
                }
            }

            return _pairs;
        }

        public void Update(List<Collider> colliders)
        {
            ClearGrid();

            _collidersGlobal = colliders;
            for (int i = 0; i < _collidersGlobal.Count; i++)
            {
                AddColliderToCells(_collidersGlobal[i]);
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
