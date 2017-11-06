using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildTask : Task
{
    public override void Execute()
    {
        Game.Model.AddBuilding(new BuildingModel(CellId, Value));
        Game.Grid.CreateBuildingOn(CellId, Value);
    }
}
