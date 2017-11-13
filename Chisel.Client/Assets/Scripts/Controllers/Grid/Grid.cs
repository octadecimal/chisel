using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chisel
{
    public class Grid : MonoBehaviour
    {
        /// <summary>
        /// Grid factory
        /// </summary>
        public GridFactory Factory { get; private set; }

        /// <summary>
        /// Grid navigator
        /// </summary>
        public GridNavigator Navigator { get; private set; }

        /// <summary>
        /// Grid interacter
        /// </summary>
        public GridInteractor Interactor { get; private set; }

        /// <summary>
        /// Game model.
        /// </summary>
        GameModel gameModel;



        // CORE
        // ========================================================================

        /// <summary>
        /// Awake routine.
        /// </summary>
        public void Awake()
        {
            // Get subcontrollers
            Factory = GetComponent<GridFactory>();
            Navigator = GetComponent<GridNavigator>();
            Interactor = GetComponent<GridInteractor>();

            // Assert
            Debug.Assert(Factory != null, "Factory not found.");
            Debug.Assert(Navigator != null, "Navigator not found.");
            Debug.Assert(Interactor != null, "Interactor not found.");
        }

        /// <summary>
        /// Configures this manager, providing any necessary references.
        /// </summary>
        /// <param name="gameModel"></param>
        public void Configure(GameModel gameModel)
        {
            this.gameModel = gameModel;

            Factory.Configure(gameModel);
            Navigator.Configure(gameModel);
            Interactor.Configure(gameModel);
        }

        /// <summary>
        /// Restores the grid and all of its entities to the state provided by the
        /// supplied game model.
        /// </summary>
        public void Restore()
        {
            // Restore cells
            foreach (CellModel cellModel in gameModel.Cells)
            {
                Factory.CreateCell(cellModel, false);
            }

            // Restore buildings
            foreach(BuildingModel buildingModel in gameModel.Buildings)
            {
                Factory.CreateBuilding(buildingModel, false);
            }
        }


        // PUBLIC
        // ========================================================================

        /// <summary>
        /// Creates and reveals any unexplored cells surrounding the cell with the
        /// passed cell id.
        /// </summary>
        /// <param name="cellId"></param>
        public void RevealSurroundingCells(int cellId)
        {
            Cell cell = Navigator.GetCell(cellId);

            GridPosition[] neighborPositions = Navigator.GetNeighborPositions(cell.X, cell.Z);

            foreach(GridPosition neighborPosition in neighborPositions)
            {
                Factory.CreateCell(new CellModel(gameModel.GetNextCellId(), neighborPosition.X, neighborPosition.Z));
            }
        }

        /// <summary>
        /// Adds a supplied number of resource units to the supplied cell id.
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="resourceType"></param>
        /// <param name="quantity"></param>
        public void AddCellResource(int cellId, int resourceType, long quantity)
        {

        }

        /// <summary>
        /// Removes a supplied number of resource units from the supplied cell id.
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="resourceType"></param>
        /// <param name="quantity"></param>
        public void RemoveCellResource(int cellId, int resourceType, long quantity)
        {

        }
    }
}