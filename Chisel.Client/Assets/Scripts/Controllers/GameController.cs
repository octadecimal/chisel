using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject GridObject;
    public GameObject TooltipObject;
    public GameObject ToolbarObject;

    public HexGridController Grid;
    public CameraController Camera;
    public TooltipController Tooltip;
    public ToolbarController Toolbar;

    public TaskManager Tasks;
    
	// Use this for initialization
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
        NotificationManager.Subscribe(CellNotifications.Clicked, HandleCellClicked);
        NotificationManager.Subscribe(CellNotifications.Selected, HandleCellSelected);
        NotificationManager.Subscribe(CellNotifications.Deselected, HandleCellDeselected);
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

    private void HandleCellClicked(object data)
    {
        HexCellController cell = (HexCellController)data;
    }

    private void HandleCellSelected(object data)
    {
        HexCellController cell = (HexCellController)data;
    }

    private void HandleCellDeselected(object data)
    {
        HexCellController cell = (HexCellController)data;
    }

    private void HandleExploreClicked(object data)
    {
        if(Grid.SelectedCell != null)
        { 
            Tasks.AddTask(TaskTypes.Explore, 3, Grid.SelectedCell.Id);
        }
    }
}