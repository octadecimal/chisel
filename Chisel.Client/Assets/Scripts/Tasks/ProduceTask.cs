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
        // Get cell
        Cell cell = Game.Grid.GetCell(EntityId);

        // Add resources
        Game.Grid.AddResourceAt(EntityId, Value, 1);
    }
}
