using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildTask : Task
{
    public BuildTask()
    {
        AutoExecute = true;
    }

    public override void Start()
    {
        // Flag cell
        Game.Grid.GetCell(EntityId).Model.HasActiveBuildTask = true;
    }

    public override void Execute()
    {
        // Get cell
        CellController cell = Game.Grid.GetCell(EntityId);

        // Create building model and add to game model
        BuildingModel model = new BuildingModel(Game.Model.GetNextBuildingId(), EntityId, Value);
        Game.Model.AddBuilding(model);

        // Create bulding
        BuildingController building = Game.Grid.CreateBuilding(model);

        // Get building production type
        ResourceTypes productionType = building.ProductionType;

        // Create production task for building
        Game.Tasks.AddTask(TaskTypes.Produce, 10, cell.Id, (int)productionType);

        // Unflag cell
        Game.Grid.GetCell(EntityId).Model.HasActiveBuildTask = true;

        // Select and move to cell
        //SelectAndMoveToCell();
    }
}