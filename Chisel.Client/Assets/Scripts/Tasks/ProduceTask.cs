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
        BuildingModel model = Game.Grid.GetCell(EntityId).Building.Model;

        // Check required resource
        if(!HasRequiredResources(model))
        {
            Pend();
            return;
        }

        // Consume resources
        foreach (ResourceTypes resourceType in model.Requires.Keys)
        {
            model.RemoveResource((int)resourceType, model.Requires[resourceType]);
        }

        // Add resources
        Game.Grid.AddResourceAt(EntityId, (int)model.Produces, 1);
    }

    public override bool Retry()
    {
        // Get building model
        BuildingModel model = Game.Grid.GetCell(EntityId).Building.Model;

        // Retry
        return HasRequiredResources(model);
    }

    private bool HasRequiredResources(BuildingModel model)
    {
        foreach (ResourceTypes resourceType in model.Requires.Keys)
        {
            if (!model.HasResource((int)resourceType, model.Requires[resourceType]))
            {
                return false;
            }
        }

        return true;
    }
}
