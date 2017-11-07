using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildTask : Task
{
    public BuildTask()
    {
        AutoExecute = false;
    }

    public override void Execute()
    {
        // Create building and add to model
        Game.Model.AddBuilding(new BuildingModel(CellId, Value));
        Game.Grid.CreateBuildingOn(CellId, Value);

        // Create production task for building
        Game.Tasks.AddTask(TaskTypes.Produce, 10, CellId, (int)ResourceTypes.Food);

        // Select and move to cell
        SelectAndMoveToCell();
    }
}