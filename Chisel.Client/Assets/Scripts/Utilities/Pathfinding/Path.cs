using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : IEnumerable<CellController>
{
    public CellController LastStep { get; private set; }
    public Path PreviousSteps { get; private set; }
    public double TotalCost { get; private set; }

    private Path(CellController lastStep, Path previousSteps, double totalCost)
    {
        LastStep = lastStep;
        PreviousSteps = previousSteps;
        TotalCost = totalCost;
    }

    public Path(CellController start) : this(start, null, 0) { }

    public Path AddStep(CellController step, double stepCost)
    {
        return new Path(step, this, TotalCost + stepCost);
    }

    public IEnumerator<CellController> GetEnumerator()
    {
        for (Path p = this; p != null; p = p.PreviousSteps)
            yield return p.LastStep;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
