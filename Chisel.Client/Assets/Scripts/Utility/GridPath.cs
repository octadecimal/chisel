using Chisel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Chisel
{
    public class GridPath : IEnumerable<Cell>
    {
        public Cell LastStep { get; private set; }
        public GridPath PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }

        public GridPath(Cell start) : this(start, null, 0)
        {

        }

        private GridPath(Cell lastStep, GridPath previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }

        public GridPath AddStep(Cell step, double stepCost)
        {
            return new GridPath(step, this, TotalCost + stepCost);
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (GridPath p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
