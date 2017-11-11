using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chisel
{
    public class ProduceTask : Task
    {
        public override bool ValidateStart()
        {
            // Check enough resources exist
            return true;
        }

        public override void Start()
        {
            // Remove and consume resources
            Cell = Game.Map.GetCell(Entity0);
            Cell.
        }

        public override void Complete()
        {
            // Produce and add resources
        }
    }
}