using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chisel
{
    [Serializable]
    public class GameModel
    {
        /// <summary>
        /// Time of last game save.
        /// </summary>
        public long Time;

        /// <summary>
        /// Grid seed.
        /// </summary>
        public float Seed;

        /// <summary>
        /// Created cells.
        /// </summary>
        public List<CellModel> Cells;

        /// <summary>
        /// Created buildings.
        /// </summary>
        public List<BuildingModel> Buildings;

        /// <summary>
        /// Running tasks.
        /// </summary>
        public List<Task> RunningTasks;

        /// <summary>
        /// Finished tasks.
        /// </summary>
        public List<Task> FinishedTasks;


        /// <summary>
        /// Creates a new game state.
        /// </summary>
        public void CreateNewGame()
        {
            Cells = new List<CellModel>();
            Buildings = new List<BuildingModel>();
            RunningTasks = new List<Task>();
            FinishedTasks = new List<Task>();

            // Set time
            Time = DateTime.Now.Ticks;

            // Create seed
            Seed = 0;

            // Create initial cell
            Cells.Add(new CellModel(0, 0, 0));
        }

        /// <summary>
        /// Adds a cell to the game model.
        /// </summary>
        /// <param name="cell"></param>
        public void AddCell(Cell cell)
        {
            CellModel cellModel = new CellModel(cell.Id, cell.X, cell.Z);
            Cells.Add(cellModel);
        }

        /// <summary>
        /// Retrieves a cell model by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CellModel GetCell(int id)
        {
            return Cells.Find(c => c.Id == id);
        }

        /// <summary>
        /// Adds a building to the game model.
        /// </summary>
        /// <param name="building"></param>
        public void AddBuilding(BuildingModel building)
        {
            Buildings.Add(building);
        }

        /// <summary>
        /// Retrieves a building model by id.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public BuildingModel GetBuilding(int id)
        {
            return Buildings.Find(b => b.Id == id);
        }

        /// <summary>
        /// Gets the next available cell id.
        /// </summary>
        /// <returns></returns>
        public int GetNextCellId()
        {
            return Cells.Count;
        }

        /// <summary>
        /// Gets the next available building id.
        /// </summary>
        /// <returns></returns>
        public int GetNextBuildingId()
        {
            return Buildings.Count;
        }
    }
}