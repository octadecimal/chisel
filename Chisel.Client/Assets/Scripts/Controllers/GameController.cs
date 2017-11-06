using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameObject grid;
    GameObject tooltip;
    GameObject toolbar;

    HexGridController gridController;
    CameraController cameraController;
    TooltipController tooltipController;
    ToolbarController toolbarController;

    TaskManager taskManager;
    
	// Use this for initialization
	void Start ()
    {
        // Get game objects
        grid = GameObject.Find("HexGrid");
        tooltip = GameObject.Find("Tooltip");
        toolbar = GameObject.Find("Toolbar");

        // Get components
        taskManager = GetComponent<TaskManager>();

        // Assert game objects found
        Debug.Assert(grid != null, "Grid game object not found.");
        Debug.Assert(tooltip != null, "Tooltip game object not found.");
        Debug.Assert(toolbar != null, "Toolbar game object not found.");
        Debug.Assert(taskManager != null, "Task manager not found.");

        // Get controllers
        gridController = grid.GetComponent<HexGridController>();
        tooltipController = tooltip.GetComponent<TooltipController>();

        // Events
        gridController.OnCellClicked += HandleCellClicked;
        gridController.OnCellSelected += HandleCellSelected;
        gridController.OnCellDeselected += HandleCellDeselected;

        // Notifications
        NotificationManager.Subscribe(ToolbarNotifications.Explore, HandleExploreClicked);
        NotificationManager.Subscribe(TaskNotifications.Finished, HandleTaskFinished);
    }

    private void HandleTaskFinished(object data)
    {
        Task task = (Task)data;

        Debug.Log("Task finished!");
    }

    // Update is called once per frame
    void Update ()
    {

    }

    private void HandleCellClicked(HexCellController cell)
    {
        //cameraController.SetTargetPosition(cell.transform.position);
        NotificationManager.Broadcast(CellNotifications.CellClicked, cell);
    }

    private void HandleCellSelected(HexCellController cell)
    {
        //tooltipController.HandleCellSelected(cell);
        NotificationManager.Broadcast(CellNotifications.CellSelected, cell);
    }

    private void HandleCellDeselected()
    {
        //tooltipController.HandleCellDeselected();
        NotificationManager.Broadcast(CellNotifications.CellDeselected);
    }

    private void HandleExploreClicked(object data)
    {
        taskManager.AddTask(TaskTypes.Explore, 3);
    }
}