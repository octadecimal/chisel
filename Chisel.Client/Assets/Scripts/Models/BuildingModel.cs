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
    public readonly Dictionary<int, long> Resources;

    public BuildingModel(int id, int cellId, int type)
    {
        Resources = new Dictionary<int, long>();

        Id = id;
        CellId = cellId;
        Type = type;
    }

    public void AddResource(int type, int quantity)
    {
        if(!Resources.ContainsKey(type))
        {
            Resources.Add(type, 0);
        }

        Resources[type] += quantity;

        Debug.Log("Added resource: type=" + type + " quantity=" + quantity + " total=" + Resources[type]);
    }

    public void RemoveResource(int type, int quantity)
    {
        if (Resources.ContainsKey(type))
        {
            if(Resources[type] >= quantity)
            { 
                Resources[type] -= quantity;
            }
            else
            {
                Debug.Log("Attempted to remove more resources than are available.");
            }
        }
        else
        {
            Debug.Log("Attempted to remove non-existant resource.");
        }
    }
}
