using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class KeepModel : BuildingModel
{
    public KeepModel(int id, int cellId, int type) : base(id, cellId, type)
    {
        Requires.Add(ResourceTypes.Food, 10);

        Produces = ResourceTypes.Worker;
    }
}
