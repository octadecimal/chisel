using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModel
{
    public float Seed;
    public List<CellModel> Cells;
    public List<BuildingModel> Buildings;
    public List<Task> RunningTasks;
    public List<Task> FinishedTasks;

    void Start ()
    {

	}

    public void CreateNewGame()
    {
        Cells = new List<CellModel>();
        Buildings = new List<BuildingModel>();
        RunningTasks = new List<Task>();
        FinishedTasks = new List<Task>();

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
}
