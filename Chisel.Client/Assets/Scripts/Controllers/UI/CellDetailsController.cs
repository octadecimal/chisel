using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CellDetailsController : MonoBehaviour
{
    // Textfields
    public Text TextBuilding;
    public Text TextType;
    public Text TextPosition;


    // START
    // ========================================================================

    private void Start()
    {
        // Assertions
        Debug.Assert(TextBuilding != null, "TextBuilding was not found.");
        Debug.Assert(TextType != null, "TextType was not found.");
        Debug.Assert(TextPosition != null, "TextPosition was not found.");

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Updated, OnCellUpdated);
        NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
        NotificationManager.Subscribe(CellNotifications.Deselected, OnCellDeselected);

        // Initial state
        ClearText();
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Clears the text.
    /// </summary>
    public void ClearText()
    {
        TextBuilding.text = "";
        TextType.text = "";
        TextPosition.text = "";
    }

    /// <summary>
    /// Sets the text for the passed cell.
    /// </summary>
    /// <param name="cell"></param>
    public void SetCellText(CellController cell)
    {
        TextBuilding.text = (cell.Building != null) ? cell.Building.Type.ToString() : "(none)";
        TextType.text = cell.Type.ToString();
        TextPosition.text = cell.X + "," + cell.Z;
    }


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell: Updated
    /// </summary>
    /// <param name="data"></param>
    private void OnCellUpdated(object data)
    {
        CellController cell = (CellController)data;
        
        SetCellText(cell.Grid.SelectedCell);
    }

    /// <summary>
    /// Cell: Selected
    /// </summary>
    /// <param name="data"></param>
    private void OnCellSelected(object data)
    {
        CellController cell = (CellController)data;

        SetCellText(cell.Grid.SelectedCell);
    }

    /// <summary>
    /// Cell: Deselected
    /// </summary>
    /// <param name="data"></param>
    private void OnCellDeselected(object data)
    {
        ClearText();
    }
}
