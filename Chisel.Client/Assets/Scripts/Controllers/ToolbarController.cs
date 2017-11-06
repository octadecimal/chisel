using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarController : MonoBehaviour
{
    public Button ExploreButton;

	void Start ()
    {
        // Assertions
        Debug.Assert(ExploreButton != null, "Explore button not found.");

        // Events
        ExploreButton.onClick.AddListener(HandleExploreClicked);

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, HandleCellSelected);
	}

    private void HandleExploreClicked()
    {
        NotificationManager.Broadcast(ToolbarNotifications.Explore);
    }

    private void HandleCellSelected(object data)
    {
        
    }
}
