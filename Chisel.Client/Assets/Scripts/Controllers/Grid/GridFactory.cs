using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chisel
{
    public class GridFactory : MonoBehaviour
    {
        /// <summary>
        /// Reference to grid root.
        /// </summary>
        Grid Grid;


        // CORE
        // ========================================================================

        /// <summary>
        /// Awake routine.
        /// </summary>
        public void Awake()
        {
            Grid = GetComponent<Grid>();
        }


        // PUBLIC
        // ========================================================================

        /// <summary>
        /// Creates a cell from a cell model.
        /// </summary>
        /// <param name="cellmodel">Cell model reference.</param>
        /// <param name="notify">If true, a notifcation will be sent upon creation.
        /// This functionality normally is desired when creating new entities at
        /// runtime, however we want to avoid it when restoring the grid, since 
        /// restored entities are not truly new entities.</param>
        public void CreateCell(CellModel cellmodel, bool notify = true)
        {

        }

        /// <summary>
        /// Creates a cell from a cell model.
        /// </summary>
        /// <param name="buildingModel">Building model reference.</param>
        /// <param name="notify">If true, a notifcation will be sent upon creation.
        /// This functionality normally is desired when creating new entities at
        /// runtime, however we want to avoid it when restoring the grid, since 
        /// restored entities are not truly new entities.</param>
        public void CreateBuilding(BuildingModel buildingModel, bool notify = true)
        {

        }

        /// <summary>
        /// Gets the seeded cell threshold of the supplied grid position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void GetCellThreshold(int x, int z)
        {

        }

        /// <summary>
        /// Gets the seeded cell threshold of the supplied grid position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public void GetTreeThreshold(int x, int z)
        {

        }


        // PRIVATE
        // ========================================================================

        /// <summary>
        /// Creates
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        private void CreateCellPrefab(int id, int x, int z)
        {

        }

        private int GetCellType(CellModel model)
        {
            return -1;
        }
    }
}