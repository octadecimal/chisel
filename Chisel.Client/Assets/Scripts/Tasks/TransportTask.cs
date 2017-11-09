using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        try
        {
            Building output = Game.Grid.GetCell(EntityId).Building;

            // Remove from output building
            Game.Grid.RemoveResourceAt(EntityId, (int)output.Producing, 1);

            // Add to input building
            Game.Grid.AddResourceAt(Value, (int)output.Producing, 1);
        }
        catch(Exception exception)
        {
            Debug.Log("Error transporting resource from " + EntityId + " to " + Value + "\n" + exception);
        }

    }
}
