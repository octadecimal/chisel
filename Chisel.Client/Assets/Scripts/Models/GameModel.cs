using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModel
{
    public long Time;
    public float Seed;
    public List<CellModel> Cells;
    public List<BuildingModel> Buildings;
    public List<ConnectionModel> Connections;
    public List<Task> RunningTasks;
    public List<Task> FinishedTasks;

    public void Start ()
    {

	}

    public void CreateNewGame()
    {
        Cells = new List<CellModel>();
        Buildings = new List<BuildingModel>();
        Connections = new List<ConnectionModel>();
        RunningTasks = new List<Task>();
        FinishedTasks = new List<Task>();

        // Set time
        Time = DateTime.Now.Ticks;

        // Create seed
        Seed = 0;

        // Create initial cell
        Cells.Add(new CellModel(0, 0, 0));
    }

    public void AddCell(CellController cell)
    {
        CellModel cellModel = new CellModel(cell.Id, cell.X, cell.Z);
        Cells.Add(cellModel);
    }

    public void AddBuilding(BuildingModel building)
    {
        Buildings.Add(building);
    }

    public BuildingModel GetBuilding(int value)
    {
        return Buildings.Find(b => b.Id == value);
    }

    public void AddConnection(ConnectionModel connectionModel)
    {
        Connections.Add(connectionModel);
    }

    public int GetNextCellId()
    {
        return Cells.Count;
    }

    public int GetNextBuildingId()
    {
        return Buildings.Count;
    }
}
