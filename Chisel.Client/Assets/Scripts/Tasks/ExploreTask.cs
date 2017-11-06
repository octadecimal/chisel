using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExploreTask : Task
{
    public override void Execute()
    {
        Debug.Log(this + " Executing task.");

        Game.Grid.RevealSurroundingCellsOf(CellId);
    }
}
