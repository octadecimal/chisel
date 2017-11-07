using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    /// <summary>
    /// Reference to our building model.
    /// </summary>
    [HideInInspector]
    public BuildingModel Model;

    /// <summary>
    /// Building input connections.
    /// </summary>
    public List<BuildingController> Inputs = new List<BuildingController>();

    /// <summary>
    /// Building output connections.
    /// </summary>
    public BuildingController Output;

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


    // PUBLIC
    // ========================================================================

    public void AttachModel(BuildingModel model)
    {
        Model = model;
    }
}
