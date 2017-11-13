using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chisel
{
    [Serializable]
    public class ExploreTask : Task
    {
        /// <summary>
        /// Cell id to explore.
        /// </summary>
        int CellId;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cellId"></param>
        public ExploreTask(int cellId)
        {
            CellId = cellId;
        }

        /// <summary>
        /// Start routine.
        /// </summary>
        public override void Start()
        {
            TaskTime = 3;
        }

        /// <summary>
        /// Complete routine.
        /// </summary>
        public override void Complete()
        {
            Game.Grid.ExploreCell(CellId);
        }
    }
}
