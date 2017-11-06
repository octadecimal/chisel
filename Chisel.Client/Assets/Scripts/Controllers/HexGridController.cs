using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridController : MonoBehaviour
{
    public int Size;
    public float Seed;
    public float Scale;
    public float SpacingX;
    public float SpacingZ;

    public GameObject CellPrefab;
    public GameObject CellSelectionPrefab;

    public GameObject CellSelection;

    public Material MountainMaterial;
    public Material GrassMaterial;
    public Material DirtMaterial;
    public Material WaterMaterial;

    public HexCellController SelectedCell;
    public Coordinate SelectedCellCoord;

    private List<HexCellController> cells;
    private List<Coordinate> createdCells;

    private int cellCount;
    
    /// <summary>
    /// Start routine.
    /// </summary>
    private void Start ()
    {
        cells = new List<HexCellController>();
        createdCells = new List<Coordinate>();

        // Create selection
        CellSelection = Instantiate(CellSelectionPrefab, transform);
        CellSelection.transform.Translate(-10000f, 0f, -10000f);

        // Build initial visible cells
        BuildInitialCells();
    }


    // PUBLIC
    // ========================================================================

    public void RevealSurroundingCellsOf(int cellId)
    {
        HexCellController cell = cells[cellId];

        Coordinate[] neighbors = GetNeighborPositions(cell.X, cell.Z);

        foreach (Coordinate neighbor in neighbors)
        {
            CreateCellAt(neighbor.X, neighbor.Z, new CellSpawnInfo(0, false, 0));
        }
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
    private void CreateCellAt(int x, int z, CellSpawnInfo info)
    {
        // Check if cell already exists
        if(createdCells.Exists(coord => coord.X == x && coord.Z == z))
        {
            return;
        }

        // Instantiate cell prefab
        GameObject cell = GetCellPrefab(x, z);

        // Get cell controller
        HexCellController cellController = cell.GetComponent<HexCellController>();

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

        // Listen for cell events
        cellController.OnClicked += HandleCellClicked;

        // Play spawn animation
        cellController.Spawn(info);
    }

    /// <summary>
    /// Gets a prefab.
    /// </summary>
    /// <param name="threshold"></param>
    private GameObject GetCellPrefab(int x, int z)
    {
        // Create cell
        GameObject cell = Instantiate(CellPrefab, this.transform);
        HexCellController cellController = cell.GetComponent<HexCellController>();
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
            case 1:
                prefabRenderer.material = WaterMaterial;
                cell.transform.Translate(0, -1f, 0);
                break;

            case 2:
                prefabRenderer.material = DirtMaterial;
                cell.transform.Translate(0, -0.05f, 0);
                break;

            case 3:
                prefabRenderer.material = GrassMaterial;
                break;

            case 4:
                prefabRenderer.material = MountainMaterial;
                cell.transform.Translate(0, threshold * 3f - 1.5f, 0);
                break;
        }

        // Notify
        NotificationManager.Broadcast(CellNotifications.Created, cellController);

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

        if (threshold < -0.25) return 1;
        if (threshold < 0.35) return 2;
        if (threshold < 0.65) return 3;
        return 4;
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
    private void HandleCellClicked(HexCellController cell)
    {
        // Notify
        NotificationManager.Broadcast(CellNotifications.Clicked, cell);

        // Select cell
        if (cell.X == SelectedCellCoord.X && cell.Z == SelectedCellCoord.Z)
        {
            CellSelection.transform.Translate(-100000f, 0f, -100000f);
            SelectedCellCoord.X = -1000000;
            SelectedCellCoord.Z = -1000000;

            SelectedCell = null;

            NotificationManager.Broadcast(CellNotifications.Deselected, cell);
        }
        else
        {
            CellSelection.transform.position = cell.transform.position;
            SelectedCellCoord.X = cell.X;
            SelectedCellCoord.Z = cell.Z;

            SelectedCell = cell;

            NotificationManager.Broadcast(CellNotifications.Selected, cell);
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