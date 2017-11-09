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
        Cell cell = Game.Grid.GetCell(EntityId);

        // Create building model and add to game model
        BuildingModel model = (BuildingModel)Activator.CreateInstance(Game.Grid.buildingModelTypes[Value], Game.Model.GetNextBuildingId(), EntityId, Value);
        Game.Model.AddBuilding(model);

        // Create bulding
        Building building = Game.Grid.CreateBuilding(model);

        // Get building producing type
        ResourceTypes producing = building.Producing;
        model.Produces = producing;

        // Create production task for building
        Game.Tasks.AddTask(TaskTypes.Produce, 3, cell.Id);

        // Unflag cell
        Game.Grid.GetCell(EntityId).Model.HasActiveBuildTask = true;

        // Select and move to cell
        //SelectAndMoveToCell();
    }
}