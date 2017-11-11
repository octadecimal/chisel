using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chisel
{
    public struct GridPosition
    {
        public int X;
        public int Z;

        public GridPosition(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}