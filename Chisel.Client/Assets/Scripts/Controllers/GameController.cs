using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Game objects
    public GameObject GridObject;
    public GameObject TooltipObject;
    public GameObject ToolbarObject;

    // Object controllers
    public HexGridController Grid;
    public CameraController Camera;
    public TooltipController Tooltip;
    public ToolbarController Toolbar;

    // Managers
    public TaskManager Tasks;


    /// <summary>
    /// Start routine. Game entry point.
    /// </summary>
	void Start ()
    {
        // Get game objects
        GridObject = GameObject.Find("HexGrid");
        TooltipObject = GameObject.Find("Tooltip");
        ToolbarObject = GameObject.Find("Toolbar");

        // Assert game objects found
        Debug.Assert(GridObject != null, "Grid game object not found.");
        Debug.Assert(TooltipObject != null, "Tooltip game object not found.");
        Debug.Assert(ToolbarObject != null, "Toolbar game object not found.");

        // Get controllers
        Grid = GridObject.GetComponent<HexGridController>();
        Tooltip = TooltipObject.GetComponent<TooltipController>();
        Tasks = GetComponent<TaskManager>();

        // Assert controllers found
        Debug.Assert(Grid != null, "Grid controller not found.");
        Debug.Assert(Tooltip != null, "Tooltip controller not found.");
        Debug.Assert(Tasks != null, "Tasks manager not found.");

        // Set game reference on our task manager so it can be passed to tasks
        Tasks.Game = this;

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Created, HandleCellCreated);
        NotificationManager.Subscribe(ToolbarNotifications.Explore, HandleExploreClicked);
    }

    /// <summary>
    /// Grid: Cell created.
    /// </summary>
    /// <param name="data"></param>
    private void HandleCellCreated(object data)
    {
        HexCellController cell = (HexCellController)data;
    }

    /// <summary>
    /// Toolbar: Explore clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleExploreClicked(object data)
    {
        if(Grid.SelectedCell != null)
        { 
            Tasks.AddTask(TaskTypes.Explore, 3, Grid.SelectedCell.Id);
        }
    }
}