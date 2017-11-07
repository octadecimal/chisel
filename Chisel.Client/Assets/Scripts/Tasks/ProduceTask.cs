using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ProduceTask : Task
{
    public ProduceTask()
    {
        AutoExecute = true;
        Repeating = true;
    }

    public override void Execute()
    {
        // Get building model
        BuildingModel buildingModel = Game.Model.GetBuilding(EntityId);

        Debug.Log("--> Producing: building="+buildingModel.Type+" resource="+Value);
    }
}
