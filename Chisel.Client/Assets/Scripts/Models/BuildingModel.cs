using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingModel
{
    public readonly int CellId;
    public readonly int Type;

    public BuildingModel(int cellId, int type)
    {
        CellId = cellId;
        Type = type;
    }
}
