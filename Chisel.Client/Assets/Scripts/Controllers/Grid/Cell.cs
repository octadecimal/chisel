using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// Cell model.
        /// </summary>
        public CellModel Model { get; private set; }

        /// <summary>
        /// Cell type.
        /// </summary>
        public CellType Type { get; set; }


        // CORE
        // ========================================================================

        /// <summary>
        /// Start routine.
        /// </summary>
        private void Start()
        {
            
        }


        // ACCESSORS
        // ========================================================================

        /// <summary>
        /// Cell id.
        /// </summary>
        public int Id
        {
            get { return Model.Id; }
        }

        /// <summary>
        /// Cell grid position along x axis.
        /// </summary>
        public int X
        {
            get { return Model.X; }
        }

        /// <summary>
        /// Cell grid position along z axis.
        /// </summary>
        public int Z
        {
            get { return Model.Z; }
        }


        // PUBLIC
        // ========================================================================

        /// <summary>
        /// Attaches a model to this controller.
        /// </summary>
        /// <param name="model"></param>
        public void AttachModel(CellModel model)
        {
            Model = model;
        }
    }
}

