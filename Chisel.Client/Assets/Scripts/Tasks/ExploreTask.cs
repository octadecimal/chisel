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

    public override void Execute()
    {
        Game.Grid.RevealSurroundingCellsOf(EntityId);

        //SelectAndMoveToCell();
    }
}
