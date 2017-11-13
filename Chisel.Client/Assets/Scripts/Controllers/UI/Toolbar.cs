using Chisel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chisel
{
    public class Toolbar : MonoBehaviour
    {
        public Button ButtonExplore;

        Cell selectedCell;


        // CORE
        // ========================================================================

        /// <summary>
        /// Start routine.
        /// </summary>
        void Start()
        {
            // Assert
            Debug.Assert(ButtonExplore != null, "ButtonExplore not found.");

            // Events
            ButtonExplore.onClick.AddListener(OnButtonExploreClicked);

            // Notifications
            NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
            NotificationManager.Subscribe(CellNotifications.Deselected, OnCellDeselected);

            // Initial state
            Hide();
        }


        // PRIVATE
        // ========================================================================

        void Show()
        {
            gameObject.SetActive(true);

            UpdateButtons();
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        void UpdateButtons()
        {
            // Expore
            
        }


        // EVENTS
        // ========================================================================

        /// <summary>
        /// Button: Explore
        /// </summary>
        private void OnButtonExploreClicked()
        {
            NotificationManager.Broadcast(ToolbarNotifications.Explore, selectedCell);
        }


        // NOTIFICATIONS
        // ========================================================================

        /// <summary>
        /// Cell: Selected
        /// </summary>
        /// <param name="data"></param>
        private void OnCellSelected(object data)
        {
            selectedCell = (Cell)data;

            Show();
        }

        /// <summary>
        /// Cell: Deselected
        /// </summary>
        /// <param name="data"></param>
        private void OnCellDeselected(object data)
        {
            selectedCell = null;

            Hide();
        }
    }
}