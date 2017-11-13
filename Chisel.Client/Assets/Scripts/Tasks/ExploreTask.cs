using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chisel
{
    public class ExploreTask : Task
    {
        int CellId;

        public ExploreTask(int cellId)
        {
            CellId = cellId;
        }

        public override void Start()
        {
            TaskTime = 3;
        }

        public override void Complete()
        {
            Game.Grid.ExploreCell(CellId);
        }
    }
}
