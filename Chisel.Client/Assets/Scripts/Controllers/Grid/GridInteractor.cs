using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    public class GridInteractor : MonoBehaviour
    {
        /// <summary>
        /// Game model reference.
        /// </summary>
        GameModel gameModel;

        /// <summary>
        /// Grid reference.
        /// </summary>
        Grid Grid;


        // CORE
        // ========================================================================

        /// <summary>
        /// Awake routine.
        /// </summary>
        public void Awake()
        {
            Debug.Log("[awake] " + this);

            Grid = GetComponent<Grid>();
        }

        /// <summary>
        /// Configure routine.
        /// </summary>
        /// <param name="gameModel"></param>
        public void Configure(GameModel gameModel)
        {
            Debug.Log("[configure] " + this);

            this.gameModel = gameModel;
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