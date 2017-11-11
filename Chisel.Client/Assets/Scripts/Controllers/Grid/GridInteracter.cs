using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    public class GridInteractor : MonoBehaviour
    {
        // Grid
        Grid Grid;


        // CORE
        // ========================================================================

        /// <summary>
        /// Awale routine.
        /// </summary>
        public void Awake()
        {
            Grid = GetComponent<Grid>();
        }


        // PUBLIC
        // ========================================================================


        public void SelectCell(int cellId)
        {

        }

        public void DeselectCell()
        {

        }


        // NOTIFICATIONS
        // ========================================================================

        private void OnCellClicked(object data)
        {

        }
    }
}