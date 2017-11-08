using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : IEnumerable<Cell>
{
    public Cell LastStep { get; private set; }
    public Path PreviousSteps { get; private set; }
    public double TotalCost { get; private set; }

    private Path(Cell lastStep, Path previousSteps, double totalCost)
    {
        LastStep = lastStep;
        PreviousSteps = previousSteps;
        TotalCost = totalCost;
    }

    public Path(Cell start) : this(start, null, 0) { }

    public Path AddStep(Cell step, double stepCost)
    {
        return new Path(step, this, TotalCost + stepCost);
    }

    public IEnumerator<Cell> GetEnumerator()
    {
        for (Path p = this; p != null; p = p.PreviousSteps)
            yield return p.LastStep;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
