using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    /// <summary>
    /// Map scale.
    /// </summary>
    public float Scale;

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

    /// <summary>
    /// Prefab for farm building.
    /// </summary>
    public GameObject BuildingFarmPrefab;

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
    public CellController SelectedCell;

    /// <summary>
    /// Reference to the game model.
    /// </summary>
    private GameModel gameModel;

    /// <summary>
    /// Lookup list of created cells.
    /// </summary>
    private List<CellController> cells;

    /// <summary>
    /// Lookup table for building types.
    /// </summary>
    private GameObject[] buildingTypes;

    
    /// <summary>
    /// Start routine.
    /// </summary>
    private void Start ()
    {
        cells = new List<CellController>();

        // Building types
        buildingTypes = new GameObject[8];
        buildingTypes[BuildingTypes.Keep] = BuildingKeepPrefab;
        buildingTypes[BuildingTypes.Farm] = BuildingFarmPrefab;

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

        // Create connections
        foreach(ConnectionModel connectionModel in gameModel.Connections)
        {
            Debug.Log("Restoring connection: " + connectionModel.Output + "->" + connectionModel.Input);

            CellController outputCell = GetCell(connectionModel.Output);
            CellController inputCell = GetCell(connectionModel.Input);

            outputCell.Building.Output = inputCell.Building;
            inputCell.Building.Inputs.Add(outputCell.Building);
        }
    }

    /// <summary>
    /// Gets a cell by id.
    /// </summary>
    /// <param name="id"></param>
    public CellController GetCell(int id)
    {
        return cells[id];
    }

    /// <summary>
    /// Selects a cell by id.
    /// </summary>
    /// <param name="cellId"></param>
    public void SelectCell(int cellId)
    {
        CellController cell = GetCell(cellId);

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
    public void CreateBuilding(BuildingModel model, bool notify = true)
    {
        CellController cell = cells[model.CellId];

        GameObject prefab = Instantiate(buildingTypes[model.Type], cell.transform);
        BuildingController buildingController = prefab.GetComponent<BuildingController>();
        buildingController.AttachModel(model);

        Debug.Assert(buildingController != null, "Building controller not found.");

        cell.Building = buildingController;

        if (notify)
        {
            NotificationManager.Broadcast(GameNotifications.Invalidated);
        }
    }

    /// <summary>
    /// Creates cells around the surrounding cell.
    /// </summary>
    /// <param name="cellId"></param>
    public void RevealSurroundingCellsOf(int cellId)
    {
        CellController cell = GetCell(cellId);

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
    public IEnumerable<CellController> GetNeighbors(CellController cell)
    {
        GridPosition[] positions = GetNeighborPositions(cell.X, cell.Z);
        CellController[] output = new CellController[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            CellController neighbor = GetCellAt(positions[i].X, positions[i].Z);

            if(neighbor != null)
            {
                output[i] = neighbor;
            }
        }

        return output;
    }

    /// <summary>
    /// Gets a cell by position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public CellController GetCellAt(int x, int z)
    {
        foreach(CellController cell in cells)
        {
            if(cell.X == x && cell.Z == z)
            {
                return cell;
            }
        }

        return null;
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
        CellController cellController = cell.GetComponent<CellController>();

        // Attach model to controller
        cellController.AttachModel(model);

        // Position cell
        if (model.X % 2 == 0)
        {
            cell.transform.Translate(model.X * SpacingX, 0, model.Z * SpacingZ);
        }
        else
        {
            cell.transform.Translate(model.X * SpacingX, 0, model.Z * SpacingZ + 0.5f);
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
        CellController cellController = cell.GetComponent<CellController>();
        cell.name = "Cell_" + x + "_" + z;
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
        CellController cell = (CellController)data;

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