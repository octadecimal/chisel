using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class TransportTask : Task
{
    public TransportTask()
    {
        AutoExecute = true;
        Repeating = true;
    }

    public override void Execute()
    {
        // Remove from output building
        BuildingModel output = Game.Model.GetBuilding(EntityId);
        //output.RemoveResource(output.)

        // Add to input building
    }
}
