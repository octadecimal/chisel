using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public int Size;
    public float Seed;
    public float Scale;

    public float SpacingX;
    public float SpacingZ;

    public GameObject CellPrefab;
    public GameObject CellSelectionPrefab;

    public GameObject BuildingKeepPrefab;

    public GameObject CellSelection;

    public Material MountainMaterial;
    public Material GrassMaterial;
    public Material DirtMaterial;
    public Material WaterMaterial;

    public CellController SelectedCell;
    public Coordinate SelectedCellCoord;

    private List<CellController> cells;
    private List<Coordinate> createdCells;

    private int cellCount;

    private GameObject[] buildingTypes;
    
    /// <summary>
    /// Start routine.
    /// </summary>
    private void Start ()
    {
        cells = new List<CellController>();
        createdCells = new List<Coordinate>();

        // Building types
        buildingTypes = new GameObject[8];
        buildingTypes[BuildingTypes.Keep] = BuildingKeepPrefab;

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
        // Save seed
        Seed = gameModel.Seed;

        // Create cells
        foreach(CellModel cellModel in gameModel.Cells)
        {
            CreateCellAt(cellModel.X, cellModel.Z, new CellSpawnInfo(0, false, 0), false);
        }

        // Create buildings
        foreach(BuildingModel buildingModel in gameModel.Buildings)
        {
            CreateBuildingOn(buildingModel.CellId, buildingModel.Type, false);
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
        SelectedCellCoord.X = cell.X;
        SelectedCellCoord.Z = cell.Z;

        SelectedCell = cell;

        NotificationManager.Broadcast(CellNotifications.Selected, cell);
    }

    /// <summary>
    /// Deselects a cell (if any)
    /// </summary>
    public void DeselectCell(object data = null)
    {
        CellSelection.transform.Translate(-100000f, 0f, -100000f);
        SelectedCellCoord.X = -1000000;
        SelectedCellCoord.Z = -1000000;

        NotificationManager.Broadcast(CellNotifications.Deselected, SelectedCell);

        SelectedCell = null;
    }

    /// <summary>
    /// Creates cells around the surrounding cell.
    /// </summary>
    /// <param name="cellId"></param>
    public void RevealSurroundingCellsOf(int cellId)
    {
        CellController cell = GetCell(cellId);

        Coordinate[] neighbors = GetNeighborPositions(cell.X, cell.Z);

        foreach (Coordinate neighbor in neighbors)
        {
            CreateCellAt(neighbor.X, neighbor.Z, new CellSpawnInfo(0, false, 0));
        }
    }
    
    /// <summary>
    /// Creates a building.
    /// </summary>
    /// <param name="cellId"></param>
    /// <param name="buildingType"></param>
    /// <param name="notify"></param>
    public void CreateBuildingOn(int cellId, int buildingType, bool notify = true)
    {
        CellController cell = cells[cellId];

        GameObject prefab = Instantiate(buildingTypes[buildingType], cell.transform);

        if(notify)
        {
            NotificationManager.Broadcast(GameNotifications.Invalidated);
        }
    }

    public IEnumerable<CellController> GetNeighbors(CellController cell)
    {
        Coordinate[] positions = GetNeighborPositions(cell.X, cell.Z);
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
    /// Builds the initial visible cells.
    /// </summary>
    private void BuildInitialCells()
    {
        // Derive offset so our grid is centered
        int start = -Size / 2;

        // Build
        for (int x = start; x < Size; x++)
        {
            for (int y = start; y < Size; y++)
            {
                CreateCellAt(x, y, new CellSpawnInfo(0, false, 0));
            }
        }
    }

    /// <summary>
    /// Creates a cell at the passed position.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    private void CreateCellAt(int x, int z, CellSpawnInfo info, bool notify = true)
    {
        // Check if cell already exists
        if(createdCells.Exists(coord => coord.X == x && coord.Z == z))
        {
            return;
        }

        // Instantiate cell prefab
        GameObject cell = GetCellPrefab(x, z);

        // Get cell controller
        CellController cellController = cell.GetComponent<CellController>();

        // Assign cell properties
        cellController.X = x;
        cellController.Z = z;

        // Position cell
        if (x % 2 == 0)
        {
            cell.transform.Translate(x * SpacingX, 0, z * SpacingZ);
        }
        else
        {
            cell.transform.Translate(x * SpacingX, 0, z * SpacingZ + 0.5f);
        }

        // Add to list
        createdCells.Add(new Coordinate(x, z));

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
    private GameObject GetCellPrefab(int x, int z)
    {
        // Create cell
        GameObject cell = Instantiate(CellPrefab, this.transform);
        CellController cellController = cell.GetComponent<CellController>();
        cell.name = "HexCell_" + x + "_" + z;
        cell.transform.position = Vector3.zero;

        // Get cell type
        float threshold = GetThreshold(x, z);
        int type = GetCellType(x, z);

        // Set cell properties
        cellController.Type = type;
        cellController.Id = cellCount++;

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
        return SimplexNoise.Noise((px + Seed) * Scale, (pz + Seed) * Scale);
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
    private Coordinate[] GetNeighborPositions(int x, int z)
    {
        Coordinate[] output = new Coordinate[6];

        if (x % 2 == 0)
        { 
            output[0] = new Coordinate(x + 0, z + 1);
            output[1] = new Coordinate(x + 1, z + 0);
            output[2] = new Coordinate(x + 1, z - 1);
            output[3] = new Coordinate(x + 0, z - 1);
            output[4] = new Coordinate(x - 1, z - 1);
            output[5] = new Coordinate(x - 1, z + 0);
        }
        else
        {
            output[0] = new Coordinate(x + 0, z + 1);
            output[1] = new Coordinate(x + 1, z + 1);
            output[2] = new Coordinate(x + 1, z + 0);
            output[3] = new Coordinate(x + 0, z - 1);
            output[4] = new Coordinate(x - 1, z + 0);
            output[5] = new Coordinate(x - 1, z + 1);
        }

        return output;
    }


    // EVENTS
    // ========================================================================

    /// <summary>
    /// Cell clicked.
    /// </summary>
    /// <param name="cell"></param>
    private void HandleCellClicked(object data)
    {
        CellController cell = (CellController)data;

        // Select cell
        if (cell.X == SelectedCellCoord.X && cell.Z == SelectedCellCoord.Z)
        {
            DeselectCell();
        }
        else
        {
            SelectCell(cell.Id);
        }
    }
}

public struct Coordinate
{
    public int X;
    public int Z;

    public Coordinate(int x, int y)
    {
        X = x;
        Z = y;
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