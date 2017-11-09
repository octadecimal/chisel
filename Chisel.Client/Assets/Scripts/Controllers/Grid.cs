using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    /// <summary>
    /// Map scale.
    /// </summary>
    public float Scale;

    /// <summary>
    /// Cell stagger amount.
    /// </summary>
    public float Stagger;

    /// <summary>
    /// Cell spacing along the X axis.
    /// </summary>
    public float SpacingX;

    /// <summary>
    /// Cell spacing along the Z axis.
    /// </summary>
    public float SpacingZ;

    /// <summary>
    /// Prefab for map grid cell.
    /// </summary>
    public GameObject CellPrefab;

    /// <summary>
    /// Prefab for cell selection.
    /// </summary>
    public GameObject CellSelectionPrefab;

    /// <summary>
    /// Prefab for keep building.
    /// </summary>
    public GameObject BuildingKeepPrefab;
    public GameObject BuildingFarmPrefab;
    public Type BuildingFarmModel;
    public Type BuildingKeepModel;

    /// <summary>
    /// Reference to the created cell selection prefab.
    /// </summary>
    public GameObject CellSelection;

    /// <summary>
    /// Material for mountain cell prefab.
    /// </summary>
    public Material MountainMaterial;

    /// <summary>
    /// Material for grass cell prefab.
    /// </summary>
    public Material GrassMaterial;

    /// <summary>
    /// Material for dirt cell prefab.
    /// </summary>
    public Material DirtMaterial;

    /// <summary>
    /// Material for water cell prefab.
    /// </summary>
    public Material WaterMaterial;

    /// <summary>
    /// Reference to the selected cell.
    /// </summary>
    public Cell SelectedCell;

    /// <summary>
    /// Reference to the game model.
    /// </summary>
    private GameModel gameModel;

    /// <summary>
    /// Lookup list of created cells.
    /// </summary>
    private List<Cell> cells;

    /// <summary>
    /// Lookup table for building types.
    /// </summary>
    private GameObject[] buildingPrefabTypes;
    public Type[] buildingModelTypes;   // TODO: Boy do these need to be offloaded


    // CORE
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    private void Start ()
    {
        cells = new List<Cell>();

        // Building prefab types
        buildingPrefabTypes = new GameObject[8];
        buildingPrefabTypes[BuildingTypes.Keep] = BuildingKeepPrefab;
        buildingPrefabTypes[BuildingTypes.Farm] = BuildingFarmPrefab;

        // Building model types
        buildingModelTypes = new Type[8];
        buildingModelTypes[BuildingTypes.Keep] = typeof(KeepModel);
        buildingModelTypes[BuildingTypes.Farm] = typeof(FarmModel);

        // Create selection
        CellSelection = Instantiate(CellSelectionPrefab, transform);
        DeselectCell();

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Clicked, HandleCellClicked);
        NotificationManager.Subscribe(InputNotifications.BackgroundClicked, DeselectCell);
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Build routine.
    /// </summary>
    /// <param name="gameModel"></param>
    public void Build(GameModel gameModel)
    {
        this.gameModel = gameModel;

        // Create cells
        foreach(CellModel cellModel in gameModel.Cells)
        {
            Debug.Log("Restoring cell: id=" + cellModel.Id + " x=" + cellModel.X + " z=" + cellModel.Z);
            CreateCellAt(cellModel, new CellSpawnInfo(0, false, 0), false);
        }

        // Create buildings
        foreach(BuildingModel buildingModel in gameModel.Buildings)
        {
            Debug.Log("Restoring building: id=" + buildingModel.Id + " cell=" + buildingModel.CellId + " type=" + buildingModel.Type);
            CreateBuilding(buildingModel, false);
        }
    }

    /// <summary>
    /// Gets a cell by id.
    /// </summary>
    /// <param name="id"></param>
    public Cell GetCell(int id)
    {
        Cell cell = cells[id];
        Debug.Assert(cell != null, "Can't find cell: " + id);
        return cell;
    }

    /// <summary>
    /// Selects a cell by id.
    /// </summary>
    /// <param name="cellId"></param>
    public void SelectCell(int cellId)
    {
        Cell cell = GetCell(cellId);

        CellSelection.transform.position = cell.transform.position;

        SelectedCell = cell;

        CellSelection.SetActive(true);

        NotificationManager.Broadcast(CellNotifications.Selected, cell);
    }

    /// <summary>
    /// Deselects a cell (if any)
    /// </summary>
    public void DeselectCell(object data = null)
    {
        CellSelection.SetActive(false);

        NotificationManager.Broadcast(CellNotifications.Deselected, SelectedCell);

        SelectedCell = null;
    }

    /// <summary>
    /// Creates a building.
    /// </summary>
    /// <param name="cellId"></param>
    /// <param name="buildingType"></param>
    /// <param name="notify"></param>
    public Building CreateBuilding(BuildingModel model, bool notify = true)
    {
        // Get building cell
        Cell cell = cells[model.CellId];

        // Create prefab
        GameObject prefab = Instantiate(buildingPrefabTypes[model.Type], cell.transform);
        Building buildingController = prefab.GetComponent<Building>();

        // Attach model to controller
        buildingController.AttachModel(model);

        // Set cell properties
        cell.Building = buildingController;

        // Notify
        if (notify)
        {
            NotificationManager.Broadcast(CellNotifications.Updated, cell);
            NotificationManager.Broadcast(GameNotifications.Invalidated);
        }

        return buildingController;
    }

    /// <summary>
    /// Creates cells around the surrounding cell.
    /// </summary>
    /// <param name="cellId"></param>
    public void RevealSurroundingCellsOf(int cellId)
    {
        Cell cell = GetCell(cellId);

        GridPosition[] neighbors = GetNeighborPositions(cell.X, cell.Z);

        foreach (GridPosition neighbor in neighbors)
        {
            CellModel neighborModel = new CellModel(gameModel.GetNextCellId(), neighbor.X, neighbor.Z);
            CreateCellAt(neighborModel, new CellSpawnInfo(0, false, 0));
        }
    }

    /// <summary>
    /// Creates and gets the neighboring cells to the passed cell.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public IEnumerable<Cell> GetNeighbors(Cell cell)
    {
        GridPosition[] positions = GetNeighborPositions(cell.X, cell.Z);
        Cell[] output = new Cell[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            Cell neighbor = GetCellAt(positions[i].X, positions[i].Z);

            if(neighbor != null)
            {
                output[i] = neighbor;
            }
        }

        return output;
    }

    /// <summary>
    /// Returns true if a cell has any unexplored neighbors.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    public bool HasUnexploredNeighbors(Cell cell)
    {
        GridPosition[] positions = GetNeighborPositions(cell.X, cell.Z);

        for (int i = 0; i < positions.Length; i++)
        {
            Cell neighbor = GetCellAt(positions[i].X, positions[i].Z);

            if (neighbor == null)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets a cell by position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Cell GetCellAt(int x, int z)
    {
        foreach(Cell cell in cells)
        {
            if(cell.X == x && cell.Z == z)
            {
                return cell;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds resources to a building at the specified cell.
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="value"></param>
    /// <param name="v"></param>
    public void AddResourceAt(int cellId, int resourceId, int quantity)
    {
        Cell cell = GetCell(cellId);

        // Assert
        Debug.Assert(cell.Building != null, "Attempted to add resources to a cell without a building. cell="+cellId);

        // Add resource to building
        cell.Building.Model.AddResource(resourceId, quantity);

        // Notify
        NotificationManager.Broadcast(CellNotifications.Updated, cell);
    }

    /// <summary>
    /// Removes resources to a building at the specified cell.
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="value"></param>
    /// <param name="v"></param>
    public void RemoveResourceAt(int cellId, int resourceId, int quantity)
    {
        Cell cell = GetCell(cellId);

        // Assert
        Debug.Assert(cell.Building != null, "Attempted to Remove resources to a cell without a building.");

        // Add resource to building
        cell.Building.Model.RemoveResource(resourceId, quantity);

        // Notify
        NotificationManager.Broadcast(CellNotifications.Updated, cell);
    }


    // PRIVATE
    // ========================================================================

    /// <summary>
    /// Creates a cell at the passed position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    private void CreateCellAt(CellModel model, CellSpawnInfo info, bool notify = true)
    {
        // Check if cell already exists
        if(cells.Exists(c => c.X == model.X && c.Z == model.Z))
        {
            return;
        }

        // Instantiate cell prefab
        GameObject cell = GetCellPrefab(model.Id, model.X, model.Z);

        // Get cell controller
        Cell cellController = cell.GetComponent<Cell>();

        // Attach model to controller
        cellController.AttachModel(model);

        // Attach grid to controller
        cellController.AttachGrid(this);

        // Position cell
        if (model.X % 2 == 0)
        {
            cell.transform.Translate(model.X * SpacingX, 0, model.Z * SpacingZ);
        }
        else
        {
            cell.transform.Translate(model.X * SpacingX, 0, model.Z * SpacingZ + Stagger);
        }

        // Play spawn animation
        cellController.Spawn(info);

        // Notify
        if(notify)
        { 
            NotificationManager.Broadcast(CellNotifications.Created, cellController);
            NotificationManager.Broadcast(GameNotifications.Invalidated);
        }
    }

    /// <summary>
    /// Gets a prefab.
    /// </summary>
    /// <param name="threshold"></param>
    private GameObject GetCellPrefab(int id, int x, int z)
    {
        // Create cell
        GameObject cell = Instantiate(CellPrefab, this.transform);
        Cell cellController = cell.GetComponent<Cell>();
        cell.name = "Cell_" + id+"__" + x + "_" + z;
        cell.transform.position = Vector3.zero;

        // Get cell type
        float threshold = GetThreshold(x, z);
        int type = GetCellType(x, z);

        // Set cell properties
        cellController.Type = type;

        // Save cell
        cells.Add(cellController);

        // Get cell material
        Renderer prefabRenderer = cell.GetComponent<Renderer>();

        // Handle type
        switch (type)
        {
            case CellTypes.Water:
                prefabRenderer.material = WaterMaterial;
                cell.transform.Translate(0, -1f, 0);
                break;

            case CellTypes.Grass:
                prefabRenderer.material = GrassMaterial;
                break;

            case CellTypes.Dirt:
                prefabRenderer.material = DirtMaterial;
                cell.transform.Translate(0, -0.05f, 0);
                break;

            case CellTypes.Mountain:
                prefabRenderer.material = MountainMaterial;
                cell.transform.Translate(0, threshold * 3f - 1.5f, 0);
                break;
        }

        return cell;
    }

    /// <summary>
    /// Gets the threshold of a cell at the passed position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private float GetThreshold(int x, int z)
    {
        float px = x / (float)128;
        float pz = z / (float)128;
        return SimplexNoise.Noise((px + gameModel.Seed) * Scale, (pz + gameModel.Seed) * Scale);
    }

    /// <summary>
    /// Gets the cell type of a cell at the passed position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private int GetCellType(int x, int z)
    {
        float threshold = GetThreshold(x, z);

        if (threshold < -0.25) return CellTypes.Water;
        if (threshold < 0.35) return CellTypes.Grass;
        if (threshold < 0.65) return CellTypes.Dirt;
        return CellTypes.Mountain;
    }

    /// <summary>
    /// Gets the neighboring coordinates.
    /// </summary>
    /// <param name="px"></param>
    /// <param name="py"></param>
    /// <returns></returns>
    private GridPosition[] GetNeighborPositions(int x, int z)
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


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell clicked.
    /// </summary>
    /// <param name="cell"></param>
    private void HandleCellClicked(object data)
    {
        Cell cell = (Cell)data;

        // Select cell
        if (SelectedCell != null && cell.X == SelectedCell.X && cell.Z == SelectedCell.Z)
        {
            DeselectCell();
        }
        else
        {
            SelectCell(cell.Id);
        }
    }
}

public struct CellSpawnInfo
{
    public int Index;
    public bool Dim;
    public float Delay;

    public CellSpawnInfo(int index, bool dim, float delay)
    {
        Index = index;
        Dim = dim;
        Delay = delay;
    }
}