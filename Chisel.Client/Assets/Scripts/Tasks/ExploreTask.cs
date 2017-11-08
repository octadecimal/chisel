using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExploreTask : Task
{
    public ExploreTask()
    {
        AutoExecute = true;
    }

    public override void Start()
    {
        Game.Grid.GetCell(EntityId).Model.HasActiveExploreTask = true;
    }

    public override void Execute()
    {
        // Show surrounding cells
        Game.Grid.RevealSurroundingCellsOf(EntityId);

        // Unflag
        Game.Grid.GetCell(EntityId).Model.HasActiveExploreTask = true;

        //SelectAndMoveToCell();
    }
}
