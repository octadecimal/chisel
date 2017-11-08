using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarController : MonoBehaviour
{
    // Buttons
    public Button ButtonInputs;
    public Button ButtonStorage;
    public Button ButtonProduction;
    public Button ButtonOutput;

    // Menus
    public GameObject MenuInputs;
    public GameObject MenuStorage;


    // START
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    void Start ()
    {
        // Assertions
        Debug.Assert(ButtonInputs != null, "ButtonInputs was not found.");
        Debug.Assert(ButtonStorage != null, "ButtonStorage was not found.");
        Debug.Assert(ButtonProduction != null, "ButtonProduction was not found.");
        Debug.Assert(ButtonOutput != null, "ButtonOutput was not found.");
        Debug.Assert(MenuInputs != null, "MenuInputs was not found.");
        Debug.Assert(MenuStorage != null, "MenuStorage was not found.");

        // Events
        ButtonInputs.onClick.AddListener(OnButtonInputsClicked);
        ButtonStorage.onClick.AddListener(OnButtonStorageClicked);
        ButtonProduction.onClick.AddListener(OnButtonProductionClicked);
        ButtonOutput.onClick.AddListener(OnButtonOutputClicked);

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
        NotificationManager.Subscribe(InputNotifications.BackgroundClicked, OnBackgroundClicked);

        // Initial state
        HideAllMenus();
    }


    // PRIVATE
    // ========================================================================

    /// <summary>
    /// Hides all menus.
    /// </summary>
    private void HideAllMenus()
    {
        MenuInputs.SetActive(false);
        MenuStorage.SetActive(false);
    }


    // EVENTS
    // ========================================================================

    /// <summary>
    /// Button: Inputs
    /// </summary>
    private void OnButtonInputsClicked()
    {
        if (!MenuInputs.activeSelf)
        {
            HideAllMenus();
        }

        MenuInputs.SetActive(!MenuInputs.activeSelf);
    }

    /// <summary>
    /// Button: Storage
    /// </summary>
    private void OnButtonStorageClicked()
    {
        if (!MenuStorage.activeSelf)
        {
            HideAllMenus();
        }

        MenuStorage.SetActive(!MenuStorage.activeSelf);
    }

    /// <summary>
    /// Button: Production
    /// </summary>
    private void OnButtonProductionClicked()
    {
        
    }

    /// <summary>
    /// Button: Output
    /// </summary>
    private void OnButtonOutputClicked()
    {
        
    }


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell selected.
    /// </summary>
    /// <param name="data"></param>
    private void OnCellSelected(object data)
    {

    }

    /// <summary>
    /// Background clicked.
    /// </summary>
    /// <param name="data"></param>
    private void OnBackgroundClicked(object data)
    {
        HideAllMenus();
    }
}
