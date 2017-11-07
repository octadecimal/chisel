using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingModel
{
    public readonly int Id;
    public readonly int CellId;
    public readonly int Type;
    public readonly Dictionary<int, int> Resources;

    public BuildingModel(int id, int cellId, int type)
    {
        Resources = new Dictionary<int, int>();

        Id = id;
        CellId = cellId;
        Type = type;
    }
}
