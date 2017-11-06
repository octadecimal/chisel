using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExploreTask : Task
{
    public override void Execute()
    {
        Game.Grid.RevealSurroundingCellsOf(CellId);

        SelectAndMoveToCell();
    }
}
