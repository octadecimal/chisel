using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class FarmModel : BuildingModel
{
    public FarmModel(int id, int cellId, int type) : base(id, cellId, type)
    {
        Produces = ResourceTypes.Food;
    }
}
