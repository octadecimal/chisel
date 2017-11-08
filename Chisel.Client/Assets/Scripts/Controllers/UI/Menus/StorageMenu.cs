using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StorageMenu : Menu
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

        if (building != null)
        {
            int index = 0;

            foreach(int resourceType in building.Resources.Keys)
            {
                items[index].GetComponent<StorageMenuItem>().TextName.text = LocalizationManager.instance.GetResourceType(resourceType);
                items[index].GetComponent<StorageMenuItem>().TextQuantity.text = building.Resources[resourceType].ToString();

                index++;
            }
        }
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

        if(building != null)
        { 
            ShowItems(building.Resources.Count);
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
        Cell cell = (Cell)data;
        Building building = cell.Building;

        if(cell == cell.Grid.SelectedCell)
        { 
            Clear();

            if (building != null)
            {
                ShowItems(building.Resources.Count);
            }
        }
    }
}
