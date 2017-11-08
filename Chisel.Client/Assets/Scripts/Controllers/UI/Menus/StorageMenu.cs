using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StorageMenu : MonoBehaviour
{
    /// <summary>
    /// Menu item prefab.
    /// </summary>
    public GameObject MenuItemPrefab;

    // Items
    private List<GameObject> items;
    private List<GameObject> itemPool;

    // Storage
    private CellController selectedCell;
    private Vector3 initialPosition;


    // CORE
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    private void Start()
    {
        // Create
        items = new List<GameObject>();
        itemPool = new List<GameObject>();

        // Assertions
        Debug.Assert(MenuItemPrefab != null, "MenuItemPrefab not found.");

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
        NotificationManager.Subscribe(CellNotifications.Deselected, OnCellDeselected);
        NotificationManager.Subscribe(CellNotifications.Updated, OnCellUpdated);

        // Save initial state
        initialPosition = transform.position + Vector3.zero;
    }


    // PUBLIC
    // ========================================================================

    public void ShowBuildingContents(BuildingController building)
    {
        int resourceCount = building.Resources.Count;

        transform.position = initialPosition;

        for(int i = 0; i < resourceCount; i++)
        {
            AddMenuItem();

            if(i > 0)
            { 
                transform.Translate(0, 40f, 0);
            }
        }

        UpdateContents();
    }

    public void UpdateContents()
    {
        BuildingController building = selectedCell.Building;
        int resourceCount = building.Resources.Count;

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

    public void Clear()
    {
        transform.position = initialPosition;

        foreach (GameObject item in items.ToList())
        {
            items.Remove(item);
            itemPool.Add(item);
            item.SetActive(false);
        }
    }


    // PRIVATE
    // ========================================================================

    private void AddMenuItem()
    {
        GameObject item;

        if(itemPool.Count > 0)
        {
            item = itemPool.Last();
            itemPool.Remove(item);
        }
        else
        {
            item = Instantiate(MenuItemPrefab, this.transform);
        }

        item.SetActive(true);
        items.Add(item);
    }


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell: Selected
    /// </summary>
    /// <param name="data"></param>
    private void OnCellSelected(object data)
    {
        selectedCell = (CellController)data;
        BuildingController building = selectedCell.Building;

        Clear();

        if(building != null)
        { 
            ShowBuildingContents(building);
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
        CellController cell = (CellController)data;
        BuildingController building = cell.Building;

        if(cell == cell.Grid.SelectedCell)
        { 
            Clear();

            if (building != null)
            {
                ShowBuildingContents(building);
            }
        }
    }
}
