using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarController : MonoBehaviour
{
    public Button ExploreButton;
    public Button BuildKeepButton;
    public Button BuildFarmButton;
    public Button OutputButton;
    public Button OutputAcceptButton;
    public Button OutputCancelButton;


    // START
    // ========================================================================

    void Start ()
    {
        // Assertions
        Debug.Assert(ExploreButton != null, "ExploreButton not found.");
        Debug.Assert(BuildKeepButton != null, "BuildKeepButton not found.");
        Debug.Assert(BuildFarmButton != null, "BuildFarmButton not found.");
        Debug.Assert(OutputButton != null, "OutputButton not found.");

        // Events
        ExploreButton.onClick.AddListener(HandleExploreButtonClicked);
        BuildKeepButton.onClick.AddListener(HandleBuildKeepButtonClicked);
        BuildFarmButton.onClick.AddListener(HandleBuildFarmButtonClicked);
        OutputButton.onClick.AddListener(HandleOutputButtonClicked);
        OutputAcceptButton.onClick.AddListener(HandleOutputAcceptButtonClicked);
        OutputCancelButton.onClick.AddListener(HandleOutputCancelButtonClicked);

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, HandleCellSelected);
    }


    // EVENTS
    // ========================================================================

    /// <summary>
    /// Explore
    /// </summary>
    private void HandleExploreButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.Explore);
    }

    /// <summary>
    /// Build Keep
    /// </summary>
    private void HandleBuildKeepButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.BuildKeep);
    }

    private void HandleBuildFarmButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.BuildFarm);
    }

    /// <summary>
    /// Output
    /// </summary>
    private void HandleOutputButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.Output);
    }

    /// <summary>
    /// Output accept
    /// </summary>
    private void HandleOutputAcceptButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.OutputAccept);
    }

    /// <summary>
    /// Output cancel
    /// </summary>
    private void HandleOutputCancelButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.OutputCancel);
    }


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell selected.
    /// </summary>
    /// <param name="data"></param>
    private void HandleCellSelected(object data)
    {
        
    }
}
