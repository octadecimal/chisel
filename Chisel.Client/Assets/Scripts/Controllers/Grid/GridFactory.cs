using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chisel
{
    public class GridFactory : MonoBehaviour
    {
        /// <summary>
        /// Map seed.
        /// </summary>
        public float Seed = 1;

        /// <summary>
        /// Map scale.
        /// </summary>
        public float Scale = 10f;

        /// <summary>
        /// Cell spacing along x axis.
        /// </summary>
        public float SpacingX;

        /// <summary>
        /// Cell spacing along z axis.
        /// </summary>
        public float SpacingZ;

        /// <summary>
        /// Grid cell prefab.
        /// </summary>
        public GameObject CellPrefab;

        /// <summary>
        /// Water material.
        /// </summary>
        public Material WaterMaterial;

        /// <summary>
        /// Dirt material.
        /// </summary>
        public Material DirtMaterial;

        /// <summary>
        /// Grass material.
        /// </summary>
        public Material GrassMaterial;

        /// <summary>
        /// Mountain material.
        /// </summary>
        public Material MountainMaterial;

        /// <summary>
        /// Game model reference.
        /// </summary>
        GameModel gameModel;

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
        /// Creates a cell from a cell model.
        /// </summary>
        /// <param name="cellModel">Cell model reference.</param>
        /// <param name="notify">If true, a notifcation will be sent upon creation.
        /// This functionality normally is desired when creating new entities at
        /// runtime, however we want to avoid it when restoring the grid, since 
        /// restored entities are not truly new entities.</param>
        public void CreateCell(CellModel cellModel, bool notify = true)
        {
            // Check if cell already exists
            if(Grid.Navigator.GetCell(cellModel.Id))
            {
                throw new Exception("Cell with id already exists: " + cellModel.Id);
            }

            // Check if cell already exists at location
            if(Grid.Navigator.GetCellAt(cellModel.X, cellModel.Z))
            {
                throw new Exception("Cell already exists at position: " + cellModel.Id);
            }

            // Instantiate cell prefab
            GameObject prefab = CreateCellPrefab(cellModel.Id, cellModel.X, cellModel.Z);

            // Get cell controller
            Cell cell = prefab.GetComponent<Cell>();

            // Attach model
            cell.AttachModel(cellModel);

            // Position cell
            if (cellModel.X % 2 == 0)
            {
                cell.transform.Translate(cellModel.X * SpacingX, 0, cellModel.Z * SpacingZ);
            }
            else
            {
                cell.transform.Translate(cellModel.X * SpacingX, 0, cellModel.Z * SpacingZ + (SpacingZ * 0.5f));
            }

            // Save cell
            Grid.Navigator.SetCell(cell);

            // Notify
            if (notify)
            {
                NotificationManager.Broadcast(CellNotifications.Created, cell);
                NotificationManager.Broadcast(GameNotifications.Invalidated);
            }
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
        public float GetCellThresholdAt(int x, int z)
        {
            float px = x / (float)128;
            float pz = z / (float)128;
            return SimplexNoise.Noise((px + gameModel.Seed) * Scale, (pz + gameModel.Seed) * Scale);
        }

        /// <summary>
        /// Gets the seeded cell threshold of the supplied grid position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public float GetTreeThreshold(int x, int z)
        {
            return 1f;
        }


        // PRIVATE
        // ========================================================================

        /// <summary>
        /// Creates a cell prefab.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        private GameObject CreateCellPrefab(int id, int x, int z)
        {
            // Instantiate game object
            GameObject prefab = Instantiate(CellPrefab, this.transform);

            // Get cell controller
            Cell cellController = prefab.GetComponent<Cell>();

            // Set game properties
            prefab.name = "Cell_" + id + "__" + x + "_" + z;
            prefab.transform.position = Vector3.zero;

            // Get cell threshold
            float cellThreshold = GetCellThresholdAt(x, z);

            // Get cell type
            cellController.Type = GetCellTypeAt(x, z);

            // Get cell renderer
            Renderer prefabRenderer = cellController.GetComponent<Renderer>();

            // Handle cell type
            switch(cellController.Type)
            {
                case CellType.Water:
                    prefabRenderer.material = WaterMaterial;
                    prefab.transform.Translate(0, -1f, 0);
                    break;

                case CellType.Grass:
                    prefabRenderer.material = GrassMaterial;
                    break;

                case CellType.Dirt:
                    prefabRenderer.material = DirtMaterial;
                    prefab.transform.Translate(0, -0.05f, 0);
                    break;

                case CellType.Mountain:
                    prefabRenderer.material = WaterMaterial;
                    prefab.transform.Translate(0, cellThreshold * 3f - 1.5f, 0);
                    break;
            }

            // Return prefab
            return prefab;
        }

        /// <summary>
        /// Returns the cell type of the passed coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private CellType GetCellTypeAt(int x, int z)
        {
            float threshold = GetCellThresholdAt(x, z);

            if (threshold < -0.25) return CellType.Water;
            if (threshold < 0.35) return CellType.Grass;
            if (threshold < 0.65) return CellType.Dirt;
            return CellType.Mountain;
        }
    }
}