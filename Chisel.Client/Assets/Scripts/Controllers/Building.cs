using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Building : MonoBehaviour
{
    /// <summary>
    /// Reference to our building model.
    /// </summary>
    [HideInInspector]
    public BuildingModel Model;

    /// <summary>
    /// Building input connections.
    /// </summary>
    public List<Building> Inputs = new List<Building>();

    /// <summary>
    /// Building output connections.
    /// </summary>
    public Building Output;

    /// <summary>
    /// The resource type this building produces.
    /// </summary>
    public ResourceTypes ProductionType;


    // START
    // ========================================================================

    public void Start()
    {

    }


    // ACCESSORS
    // ========================================================================

    public int Id
    {
        get { return Model.Id; }
    }

    public int CellId
    {
        get { return Model.CellId; }
    }

    public int Type
    {
        get { return Model.Type; }
    }

    public Dictionary<int, long> Resources
    {
        get { return Model.Resources; }
    }


    // PUBLIC
    // ========================================================================

    public void AttachModel(BuildingModel model)
    {
        Model = model;
    }
}
