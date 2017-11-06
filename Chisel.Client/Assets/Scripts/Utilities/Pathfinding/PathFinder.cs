using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathFinder
{
    public static Path FindPath(GridController grid, CellController start, CellController destination)
    {
        var closed = new HashSet<CellController>();
        var queue = new PriorityQueue<double, Path>();
        queue.Enqueue(0, new Path(start));

        while (!queue.IsEmpty)
        {
            var path = queue.Dequeue();

            if (closed.Contains(path.LastStep))
                continue;
            if (path.LastStep.Equals(destination))
                return path;

            closed.Add(path.LastStep);

            IEnumerable<CellController> neighbors = grid.GetNeighbors(path.LastStep);

            foreach (CellController n in neighbors)
            {
                if(n != null && n.Type != CellTypes.Water && n.Type != CellTypes.Mountain)
                { 
                    double d = Distance(path.LastStep, n);
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + Estimate(n, destination),
                        newPath);
                }
            }
        }

        return null;
    }

    static double Distance(CellController tile1, CellController tile2)
    {
        return 1;
    }

    static double Estimate(CellController tile, CellController destTile)
    {
        float dx = Mathf.Abs(destTile.X - tile.X);
        float dy = Mathf.Abs(destTile.Z - tile.Z);
        int z1 = -(tile.X + tile.Z);
        int z2 = -(destTile.X + destTile.Z);
        float dz = Mathf.Abs(z2 - z1);

        return Mathf.Max(dx, dy, dz);
    }
}