using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Game model
    public GameModel Model;

    // Game objects
    public GameObject CameraObject;
    public GameObject GridObject;
    public GameObject TooltipObject;
    public GameObject ToolbarObject;
    public GameObject TaskPanelObject;

    // Object controllers
    public CameraController Camera;
    public GridController Grid;
    public TooltipController Tooltip;
    public ToolbarController Toolbar;
    public TaskPanelController TaskPanel;

    // Managers
    public TaskManager Tasks;

    // Flags
    private bool invalidated;
    private long invalidationTime;


    // START
    // ========================================================================

    /// <summary>
    /// Start routine. Game entry point.
    /// </summary>
	void Start()
    {
        // Get game objects
        CameraObject = GameObject.Find("Camera");
        GridObject = GameObject.Find("Grid");
        TooltipObject = GameObject.Find("Tooltip");
        ToolbarObject = GameObject.Find("Toolbar");
        TaskPanelObject = GameObject.Find("TaskPanel");

        // Assert game objects found
        Debug.Assert(CameraObject != null, "Camera game object not found.");
        Debug.Assert(GridObject != null, "Grid game object not found.");
        Debug.Assert(TooltipObject != null, "Tooltip game object not found.");
        Debug.Assert(ToolbarObject != null, "Toolbar game object not found.");
        Debug.Assert(TaskPanelObject != null, "Task panel game object not found.");

        // Get controllers
        Camera = CameraObject.GetComponent<CameraController>();
        Grid = GridObject.GetComponent<GridController>();
        Tooltip = TooltipObject.GetComponent<TooltipController>();
        TaskPanel = TaskPanelObject.GetComponent<TaskPanelController>();
        Tasks = GetComponent<TaskManager>();

        // Assert controllers found
        Debug.Assert(Camera != null, "Camera controller not found.");
        Debug.Assert(Grid != null, "Grid controller not found.");
        Debug.Assert(Tooltip != null, "Tooltip controller not found.");
        Debug.Assert(TaskPanel != null, "Task panel controller not found.");
        Debug.Assert(Tasks != null, "Tasks manager not found.");

        // Set game reference on our task manager so it can be passed to tasks
        Tasks.Game = this;

        // Notifications
        NotificationManager.Subscribe(GameNotifications.Invalidated, HandleInvalidate);
        NotificationManager.Subscribe(CellNotifications.Created, HandleCellCreated);
        NotificationManager.Subscribe(ToolbarNotifications.Explore, HandleExploreClicked);
        NotificationManager.Subscribe(ToolbarNotifications.BuildKeep, HandleBuildKeepClicked);
        NotificationManager.Subscribe(ToolbarNotifications.BuildFarm, HandleBuildFarmClicked);
        NotificationManager.Subscribe(TaskPanelNotifications.CompleteClicked, HandleTaskStatusCompleteClicked);
        NotificationManager.Subscribe(ConnectionNotifications.Created, HandleConnectionCreated);

        // Load game
        DeleteSave();
        Load();

        // Build components
        
        Grid.Build(Model);
        TaskPanel.Build(Model);
        Tasks.Build(Model);
    }

    /// <summary>
    /// Application quit routine.
    /// </summary>
    public void OnApplicationQuit()
    {
        Save();
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Update routine.
    /// </summary>
    public void Update()
    {
        // Broadcast game update
        NotificationManager.Broadcast(GameNotifications.Update, Time.deltaTime);

        // Handle invalidations and save
        if(invalidated)
        {
            if (DateTime.Now.Ticks >= invalidationTime)
            {
                Save();
                invalidated = false;
                invalidationTime = 0;
            }
        }
    }

    /// <summary>
    /// Loads the game.
    /// </summary>
    public void Load()
    {
        string path = Application.persistentDataPath + "/save.bin";

        if (File.Exists(path))
        {
            Debug.Log("Loading: " + path);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            Model = (GameModel)formatter.Deserialize(file);

            file.Close();
        }
        else
        {
            Debug.Log("Creating new game: " + path);

            Model = new GameModel();
            Model.CreateNewGame();

            Save();
        }
    }

    /// <summary>
    /// Saves the game.
    /// </summary>
    public void Save()
    {
        Model.Time = DateTime.Now.Ticks;

        string path = Application.persistentDataPath + "/save.bin";

        Debug.Log("Saving: " + path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        formatter.Serialize(file, Model);
        file.Close();
    }

    /// <summary>
    /// Deletes the saved game.
    /// </summary>
    public void DeleteSave()
    {
        string path = Application.persistentDataPath + "/save.bin";
        Debug.Log("Deleting save: " + path);
        File.Delete(path);
    }


    // GAME NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Game: Invalidated.
    /// </summary>
    /// <param name="data"></param>
    private void HandleInvalidate(object data)
    {
        if(invalidationTime == 0)
        { 
            invalidationTime = TimeTools.AddSeconds(DateTime.Now.Ticks, 60);
            invalidated = true;
        }
    }


    // GRID NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell: Created.
    /// </summary>
    /// <param name="data"></param>
    private void HandleCellCreated(object data)
    {
        CellController cell = (CellController)data;

        Model.AddCell(cell);
    }


    // TOOLBAR NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Toolbar: Explore clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleExploreClicked(object data)
    {
        if (Grid.SelectedCell != null)
        {
            Tasks.AddTask(TaskTypes.Explore, 3, Grid.SelectedCell.Id);
        }
    }

    /// <summary>
    /// Toolbar: Build Keep clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleBuildKeepClicked(object data)
    {
        if (Grid.SelectedCell != null)
        {
            Tasks.AddTask(TaskTypes.Build, 3, Grid.SelectedCell.Id, BuildingTypes.Keep);
        }
    }

    /// <summary>
    /// Toolbar: Build Keep clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleBuildFarmClicked(object data)
    {
        if (Grid.SelectedCell != null)
        {
            Tasks.AddTask(TaskTypes.Build, 3, Grid.SelectedCell.Id, BuildingTypes.Farm);
        }
    }


    // TASKPANEL NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// TaskPanel: Complete clicked on a task status.
    /// </summary>
    /// <param name="data"></param>
    private void HandleTaskStatusCompleteClicked(object data)
    {
        TaskStatusController taskStatus = (TaskStatusController)data;

        Tasks.ExecuteTask(taskStatus.Task);

        taskStatus.Dispose();
    }


    // CONNECTION NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Connection: Created.
    /// </summary>
    /// <param name="data"></param>
    private void HandleConnectionCreated(object data)
    {
        ConnectionModel connectionModel = (ConnectionModel)data;

        Model.AddConnection(connectionModel);

        Tasks.AddTask(TaskTypes.Transport, 10, connectionModel.Output, connectionModel.Input);
    }
}