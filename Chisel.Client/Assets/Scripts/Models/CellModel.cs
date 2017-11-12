using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chisel
{
    [Serializable]
    public class CellModel
    {
        /// <summary>
        ///  Unique cell identifier.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Grid position along x axis.
        /// </summary>
        public readonly int X;

        /// <summary>
        /// Grid position along y axis.
        /// </summary>
        public readonly int Z;

        /// <summary>
        /// Cell currently has an active explore task.
        /// </summary>
        public bool HasActiveExploreTask;

        /// <summary>
        /// Cell currently has an active build task.
        /// </summary>
        public bool HasActiveBuildTask;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="z"></param>
        public CellModel(int id, int x, int z)
        {
            Id = id;
            X = x;
            Z = z;
        }
    }
}