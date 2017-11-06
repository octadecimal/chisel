using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Game model
    public GameModel GameModel;

    // Game objects
    public GameObject GridObject;
    public GameObject TooltipObject;
    public GameObject ToolbarObject;

    // Object controllers
    public GridController Grid;
    public CameraController Camera;
    public TooltipController Tooltip;
    public ToolbarController Toolbar;

    // Managers
    public TaskManager Tasks;

    // Flags
    private bool invalidated;


    /// <summary>
    /// Start routine. Game entry point.
    /// </summary>
	void Start ()
    {
        // Get game objects
        GridObject = GameObject.Find("Grid");
        TooltipObject = GameObject.Find("Tooltip");
        ToolbarObject = GameObject.Find("Toolbar");

        // Assert game objects found
        Debug.Assert(GridObject != null, "Grid game object not found.");
        Debug.Assert(TooltipObject != null, "Tooltip game object not found.");
        Debug.Assert(ToolbarObject != null, "Toolbar game object not found.");

        // Get controllers
        Grid = GridObject.GetComponent<GridController>();
        Tooltip = TooltipObject.GetComponent<TooltipController>();
        Tasks = GetComponent<TaskManager>();

        // Assert controllers found
        Debug.Assert(Grid != null, "Grid controller not found.");
        Debug.Assert(Tooltip != null, "Tooltip controller not found.");
        Debug.Assert(Tasks != null, "Tasks manager not found.");

        // Set game reference on our task manager so it can be passed to tasks
        Tasks.Game = this;

        // Notifications
        NotificationManager.Subscribe(GameNotifications.Invalidated, HandleGridInvalidated);
        NotificationManager.Subscribe(CellNotifications.Created, HandleCellCreated);
        NotificationManager.Subscribe(ToolbarNotifications.Explore, HandleExploreClicked);

        // Load game
        //DeleteSave();
        Load();

        // Build components
        Tasks.Build(GameModel);
        Grid.Build(GameModel);
    }


    // PUBLIC
    // ========================================================================

    public void Update()
    {
        if(invalidated)
        {
            Save();
            invalidated = false;
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

            GameModel = (GameModel)formatter.Deserialize(file);

            file.Close();
        }
        else
        {
            Debug.Log("Creating new game: " + path);

            GameModel = new GameModel();
            GameModel.CreateNewGame();

            Save();
        }
    }

    /// <summary>
    /// Saves the game.
    /// </summary>
    public void Save()
    {
        string path = Application.persistentDataPath + "/save.bin";

        Debug.Log("Saving: " + path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        formatter.Serialize(file, GameModel);
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


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Grid: Invalidated.
    /// </summary>
    /// <param name="data"></param>
    private void HandleGridInvalidated(object data)
    {
        invalidated = true;
        //Save();
    }

    /// <summary>
    /// Cell: Created.
    /// </summary>
    /// <param name="data"></param>
    private void HandleCellCreated(object data)
    {
        CellController cell = (CellController)data;

        GameModel.AddCell(cell);
    }

    /// <summary>
    /// Toolbar: Explore clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleExploreClicked(object data)
    {
        if(Grid.SelectedCell != null)
        { 
            Tasks.AddTask(TaskTypes.Explore, 10, Grid.SelectedCell.Id);
        }
    }
}