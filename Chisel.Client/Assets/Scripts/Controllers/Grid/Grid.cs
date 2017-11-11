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
        public GridInteractor Interacter { get; private set; }



        // CORE
        // ========================================================================

        /// <summary>
        /// Start routine.
        /// </summary>
        public void Start()
        {
            // Get subcontrollers
            Factory = GetComponent<GridFactory>();
            Navigator = GetComponent<GridNavigator>();
            Interacter = GetComponent<GridInteractor>();
        }

        /// <summary>
        /// Restores the grid and all of its entities to the state provided by the
        /// supplied game model.
        /// </summary>
        /// <param name="gameModel"></param>
        public void Restore(GameModel gameModel)
        {

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

        }

        /// <summary>
        /// Adds a supplied number of resource units to the supplied cell id.
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="resourceType"></param>
        /// <param name="quantity"></param>
        public void AddResource(int cellId, int resourceType, long quantity)
        {

        }

        /// <summary>
        /// Removes a supplied number of resource units from the supplied cell id.
        /// </summary>
        /// <param name="cellId"></param>
        /// <param name="resourceType"></param>
        /// <param name="quantity"></param>
        public void RemoveResource(int cellId, int resourceType, long quantity)
        {

        }
    }
}