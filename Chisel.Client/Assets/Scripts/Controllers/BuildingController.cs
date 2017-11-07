using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public BuildingModel Model;

    public List<BuildingController> Inputs = new List<BuildingController>();
    public BuildingController Output;


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
