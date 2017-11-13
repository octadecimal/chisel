using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    public class GridNavigator : MonoBehaviour
    {
        /// <summary>
        /// Game model reference.
        /// </summary>
        GameModel gameModel;

        /// <summary>
        /// Reference to parent grid controller.
        /// </summary>
        Grid grid;

        /// <summary>
        /// List of all created cells.
        /// </summary>
        List<Cell> cells;


        // CORE
        // ========================================================================

        /// <summary>
        /// Awale routine.
        /// </summary>
        public void Awake()
        {
            // Create
            cells = new List<Cell>();

            // Get components
            grid = GetComponent<Grid>();
        }

        /// <summary>
        /// Configure routine.
        /// </summary>
        /// <param name="gameModel"></param>
        public void Configure(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }


        // PUBLIC
        // ========================================================================

        /// <summary>
        /// Sets and saves a cell into storage.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public void SetCell(Cell cell)
        {
            if(GetCell(cell.Id))
            {
                throw new Exception("Cell already exists: " + cell.Id);
            }

            cells.Add(cell);
        }

        /// <summary>
        /// Retrieves an individual cell by id.
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public Cell GetCell(int cellId)
        {
            return cells.Find(c => c.Id == cellId);
        }

        /// <summary>
        /// Gets a cell by position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Cell GetCellAt(int x, int z)
        {
            return cells.Find(c => c.X == x && c.Z == z);
        }

        /// <summary>
        /// Gets the grid positions of every neighboring cell, regardless of if 
        /// the cell yet exists or not.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public GridPosition[] GetNeighborPositions(int x, int z)
        {
            GridPosition[] output = new GridPosition[6];

            if (x % 2 == 0)
            {
                output[0] = new GridPosition(x + 0, z + 1);
                output[1] = new GridPosition(x + 1, z + 0);
                output[2] = new GridPosition(x + 1, z - 1);
                output[3] = new GridPosition(x + 0, z - 1);
                output[4] = new GridPosition(x - 1, z - 1);
                output[5] = new GridPosition(x - 1, z + 0);
            }
            else
            {
                output[0] = new GridPosition(x + 0, z + 1);
                output[1] = new GridPosition(x + 1, z + 1);
                output[2] = new GridPosition(x + 1, z + 0);
                output[3] = new GridPosition(x + 0, z - 1);
                output[4] = new GridPosition(x - 1, z + 0);
                output[5] = new GridPosition(x - 1, z + 1);
            }

            return output;
        }

        /// <summary>
        /// Gets references to all neighboring cells.
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public IEnumerable<Cell> GetCellNeighbors(Cell cell)
        {
            return null;
        }

        /// <summary>
        /// Returns true if the passed cell has any unexplored neighbors.
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public bool CellHasUnexploredNeighbors(int cellId)
        {
            return false;
        }

        /// <summary>
        /// Returns the shortest path from one cell to another.
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="destinationCell"></param>
        /// <returns></returns>
        public GridPath GetPath(Cell startCell, Cell destinationCell)
        {
            HashSet<Cell> closed = new HashSet<Cell>();
            var queue = new PriorityQueue<double, GridPath>();
            queue.Enqueue(0, new GridPath(startCell));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                { 
                    continue;
                }

                if (path.LastStep.Equals(destinationCell))
                { 
                    return path;
                }

                closed.Add(path.LastStep);

                IEnumerable<Cell> neighbors = grid.Navigator.GetCellNeighbors(path.LastStep);

                foreach (Cell n in neighbors)
                {
                    if (n != null && n.Type != CellType.Water && n.Type != CellType.Mountain)
                    {
                        double d = ApproximateDistance(path.LastStep, n);
                        var newPath = path.AddStep(n, d);
                        queue.Enqueue(newPath.TotalCost + 1, newPath);
                    }
                }
            }

            return null;
        }


        // PRIVATE
        // ========================================================================

        /// <summary>
        /// Gets the approximate distance between two cells.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double ApproximateDistance(Cell a, Cell b)
        {
            float dx = Mathf.Abs(b.X - a.X);
            float dy = Mathf.Abs(b.Z - a.Z);
            int z1 = -(a.X + a.Z);
            int z2 = -(b.X + b.Z);
            float dz = Mathf.Abs(z2 - z1);

            return Mathf.Max(dx, dy, dz);
        }
    }
}