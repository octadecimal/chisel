using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OutputMenu : Menu
{
    // Selection types
    enum SelectionType
    {
        Invalid,
        Valid,
        Existing
    }

    // Items
    public MenuItem ItemAccept;
    public MenuItem ItemCancel;
    public MenuItem ItemBreak;
    public MenuItem ItemCreate;

    // Storage
    private Cell selectedCell;

    // Flags
    private bool isConnecting;
    private SelectionType outputSelectionType;
    private SelectionType inputSelectionType;


    // CORE
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Labels
        ItemAccept.TextCaption.text = "Accept";
        ItemCancel.TextCaption.text = "Cancel";
        ItemBreak.TextCaption.text = "Break";
        ItemCreate.TextCaption.text = "Create";

        // Events
        ItemAccept.Button.onClick.AddListener(OnAcceptClicked);
        ItemCancel.Button.onClick.AddListener(OnCancelClicked);
        ItemBreak.Button.onClick.AddListener(OnBreakClicked);
        ItemCreate.Button.onClick.AddListener(OnCreateClicked);

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
        NotificationManager.Subscribe(CellNotifications.Updated, OnCellUpdated);
        NotificationManager.Subscribe(InputNotifications.BackgroundClicked, OnBackgroundClicked);

        // Initial state
        Reset();
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Resets the state.
    /// </summary>
    public void Reset()
    {
        isConnecting = false;
        outputSelectionType = SelectionType.Invalid;
        inputSelectionType = SelectionType.Invalid;

        gameObject.SetActive(false);

        UpdateButtonStates();
    }

    /// <summary>
    /// Hides all elements.
    /// </summary>
    public void HideAll()
    {
        ItemAccept.gameObject.SetActive(false);
        ItemCancel.gameObject.SetActive(false);
        ItemBreak.gameObject.SetActive(false);
        ItemCreate.gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the button states.
    /// </summary>
    public void UpdateButtonStates()
    {
        // Flag to count number of visible elements
        int visibleCount = 0;

        // Restore position
        transform.position = initialPosition;

        // Hide all
        HideAll();

        if(isConnecting)
        { 
            // Accept
            if (inputSelectionType == SelectionType.Valid)
            {
                ItemAccept.gameObject.SetActive(true);
                visibleCount++;
            }

            // Cancel
            ItemCancel.gameObject.SetActive(true);
            visibleCount++;
        }
        else
        {
            // Break
            if (outputSelectionType == SelectionType.Existing)
            {
                ItemBreak.gameObject.SetActive(true);
                visibleCount++;
            }

            // Create
            if (outputSelectionType == SelectionType.Valid)
            {
                ItemCreate.gameObject.SetActive(true);
                visibleCount++;
            }
        }

        // Size and position
        GetComponent<RectTransform>().sizeDelta = new Vector2(196f, 40f * visibleCount);
        transform.Translate(new Vector3(0, 40 * (visibleCount-1), 0));
    }


    // EVENTS
    // ========================================================================

    /// <summary>
    /// Clicked: Create
    /// </summary>
    private void OnCreateClicked()
    {
        // Flag
        isConnecting = true;

        // Update
        UpdateButtonStates();

        // Broadcast
        NotificationManager.Broadcast(ToolbarNotifications.OutputCreate);
    }

    /// <summary>
    /// Clicked: Cancel
    /// </summary>
    private void OnCancelClicked()
    {
        // Broadcast
        NotificationManager.Broadcast(ToolbarNotifications.OutputCancel);

        // Reset
        Reset();
    }

    /// <summary>
    /// Clicked: Accept
    /// </summary>
    private void OnAcceptClicked()
    {
        // Broadcast
        NotificationManager.Broadcast(ToolbarNotifications.OutputAccept);

        // Reset
        Reset();
    }

    /// <summary>
    /// Clicked: Break
    /// </summary>
    private void OnBreakClicked()
    {
        // Broadcast
        NotificationManager.Broadcast(ToolbarNotifications.OutputBreak);

        // Reset
        Reset();
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

        // Output cell
        if(!isConnecting)
        {
            // Cell doesn't have a building
            if (selectedCell.Building == null)
            {
                outputSelectionType = SelectionType.Invalid;
            }
            // Cell already has an existing output
            else if (selectedCell.Building.Output != -1)
            {
                outputSelectionType = SelectionType.Existing;
            }
            // Cell is valid
            else
            {
                outputSelectionType = SelectionType.Valid;
            }
        }
        // Input cell
        else
        {
            // Cell doesn't have a building
            if (selectedCell.Building == null)
            {
                inputSelectionType = SelectionType.Invalid;
            }
            // Cell is valid
            else
            {
                inputSelectionType = SelectionType.Valid;
            }
        }

        // Update
        UpdateButtonStates();
    }

    /// <summary>
    /// Cell: Updated
    /// </summary>
    /// <param name="data"></param>
    private void OnCellUpdated(object data)
    {
        Cell cell = (Cell)data;

        if(selectedCell != null && cell == selectedCell)
        {
            OnCellSelected(cell);
        }
    }

    /// <summary>
    /// Background: Clicked
    /// </summary>
    /// <param name="data"></param>
    private void OnBackgroundClicked(object data)
    {
        Reset();
    }

}
