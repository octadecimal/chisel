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

    public override void Execute()
    {
        // Create building model and add to game model
        BuildingModel model = new BuildingModel(Game.Model.GetNextBuildingId(), EntityId, Value);
        Game.Model.AddBuilding(model);

        // Create bulding
        BuildingController building = Game.Grid.CreateBuilding(model);

        // Get building production type
        ResourceTypes productionType = building.ProductionType;

        // Create production task for building
        Game.Tasks.AddTask(TaskTypes.Produce, 10, model.Id, (int)productionType);

        // Select and move to cell
        SelectAndMoveToCell();
    }
}