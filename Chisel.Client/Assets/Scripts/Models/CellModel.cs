using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CellModel
{
    public readonly int Id;
    public readonly int X;
    public readonly int Z;

    public CellModel(int id, int x, int z)
    {
        Id = id;
        X = x;
        Z = z;
    }
}
