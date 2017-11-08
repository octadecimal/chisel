using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{
    public Text CellTypeLabel;
    public Text CellPositionLabel;
    public Text CellResourcesLabel;

	void Start ()
    {
        // Assertions
        Debug.Assert(CellTypeLabel != null, "CellTypeLabel not found.");
        Debug.Assert(CellPositionLabel != null, "CellPositionLabel not found.");
        Debug.Assert(CellResourcesLabel != null, "CellResourcesLabel not found.");

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, HandleCellSelected);
        NotificationManager.Subscribe(CellNotifications.Deselected, HandleCellDeselected);
    }

    void Update ()
    {
		
	}

    public void HandleCellSelected(object data)
    {
        CellController cell = (CellController) data;

        // Show cell type
        string cellType;
        switch(cell.Type)
        {
            case 1:
                cellType = "Water";
                break;
            case 2:
                cellType = "Desert";
                break;
            case 3:
                cellType = "Field";
                break;
            case 4:
                cellType = "Mountain";
                break;
            default:
                cellType = "Unknown";
                break;
        }

        CellTypeLabel.text = cellType;

        CellPositionLabel.text = cell.X + "," + cell.Z;

        if(cell.Building != null)
        {
            CellResourcesLabel.text = "";
            foreach(int key in cell.Building.Resources.Keys)
            {
                CellResourcesLabel.text += key + " x" + cell.Building.Resources[key] + "\n";
            }
        }
    }

    public void HandleCellDeselected(object data)
    {
        CellTypeLabel.text = "";
        CellPositionLabel.text = "";
        CellResourcesLabel.text = "";
    }
}
