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
        /// Selected cell highlight prefab.
        /// </summary>
        public GameObject CellHighlightPrefab;

        /// <summary>
        /// Selected cell.
        /// </summary>
        public Cell SelectedCell { get; private set; }

        /// <summary>
        /// Game model reference.
        /// </summary>
        GameModel gameModel;

        /// <summary>
        /// Grid reference.
        /// </summary>
        Grid Grid;

        /// <summary>
        /// Cell highlight.
        /// </summary>
        GameObject cellHighlight;


        // CORE
        // ========================================================================

        /// <summary>
        /// Awake routine.
        /// </summary>
        public void Awake()
        {
            // Get componenets
            Grid = GetComponent<Grid>();

            // Notifications
            NotificationManager.Subscribe(InputNotifications.BackgroundClicked, OnBackgroundClicked);
            NotificationManager.Subscribe(InputNotifications.CellClicked, OnCellClicked);
        }

        /// <summary>
        /// Start routine.
        /// </summary>
        public void Start()
        {
            cellHighlight = Instantiate(CellHighlightPrefab, this.transform);
            cellHighlight.SetActive(false);
        }

        /// <summary>
        /// Configure routine.
        /// </summary>
        /// <param name="gameModel"></param>
        public void Configure(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }


        // PUBLIC
        // ========================================================================

        /// <summary>
        /// Selects a cell.
        /// </summary>
        /// <param name="cell"></param>
        public void SelectCell(Cell cell)
        {
            SelectedCell = cell;
            cellHighlight.SetActive(true);

            cellHighlight.transform.position = cell.transform.position;

            NotificationManager.Broadcast(CellNotifications.Selected, cell);
        }

        /// <summary>
        /// Deselects a cell.
        /// </summary>
        public void DeselectCell()
        {
            SelectedCell = null;
            cellHighlight.SetActive(false);

            NotificationManager.Broadcast(CellNotifications.Deselected);
        }


        // NOTIFICATIONS
        // ========================================================================

        /// <summary>
        /// Background: Clicked
        /// </summary>
        /// <param name="data"></param>
        private void OnBackgroundClicked(object data)
        {
            DeselectCell();
        }
        
        /// <summary>
        /// Cell: Clicked
        /// </summary>
        /// <param name="data"></param>
        private void OnCellClicked(object data)
        {
            Cell cell = (Cell)data;

            if (SelectedCell != null && cell.X == SelectedCell.X && cell.Z == SelectedCell.Z)
            {
                DeselectCell();
            }
            else
            {
                SelectCell(cell);
            }
        }
    }
}