using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarController : MonoBehaviour
{
    public Button ExploreButton;
    public Button BuildKeepButton;

	void Start ()
    {
        // Assertions
        Debug.Assert(ExploreButton != null, "ExploreButton not found.");
        Debug.Assert(BuildKeepButton != null, "BuildKeepButton not found.");

        // Events
        ExploreButton.onClick.AddListener(HandleExploreClicked);
        BuildKeepButton.onClick.AddListener(HandleBuildKeepButtonClicked);

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, HandleCellSelected);
	}

    private void HandleExploreClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.Explore);
    }

    private void HandleBuildKeepButtonClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.Explore);
    }

    private void HandleCellSelected(object data)
    {
        
    }
}
