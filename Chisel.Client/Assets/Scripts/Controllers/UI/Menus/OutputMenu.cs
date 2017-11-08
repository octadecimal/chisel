using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OutputMenu : Menu
{
    // Storage
    private Cell selectedCell;


    // CORE
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
        NotificationManager.Subscribe(CellNotifications.Deselected, OnCellDeselected);
        NotificationManager.Subscribe(CellNotifications.Updated, OnCellUpdated);
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Updates the menu items.
    /// </summary>
    public override void UpdateItems()
    {
        Building building = selectedCell.Building;

        if(building == null)
        {
            MenuItem item = items[0].GetComponent<MenuItem>();
            item.TextCaption.text = "Create connection";
            item.Button.onClick.AddListener(OnCreateConnectionClicked);
        }
        else
        {

        }
    }


    // EVENTS
    // ========================================================================

    private void OnCreateConnectionClicked()
    {
        Debug.Log("Connection create clicked!");
        NotificationManager.Broadcast(ToolbarNotifications.Output);
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
        Building building = selectedCell.Building;

        Clear();

        // Cell does not have a building
        if(building == null)
        {
            ShowItems(1);
        }
        else
        {
            ShowItems(3);
        }
    }

    /// <summary>
    /// Cell: Deselected
    /// </summary>
    /// <param name="data"></param>
    private void OnCellDeselected(object data)
    {
        selectedCell = null;

        Clear();
    }

    /// <summary>
    /// Cell: Updated
    /// </summary>
    /// <param name="data"></param>
    private void OnCellUpdated(object data)
    {
        /*Cell cell = (Cell)data;
        Building building = cell.Building;

        if (cell == cell.Grid.SelectedCell)
        {
            Clear();

            // DO STUFF
        }*/
    }

}
