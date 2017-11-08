using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public Game Game;

    Cell outputCell;

    /// <summary>
    /// Start routine.
    /// </summary>
    public void Start()
    {
        NotificationManager.Subscribe(ToolbarNotifications.OutputCreate, HandleOutputClicked);
        NotificationManager.Subscribe(ToolbarNotifications.OutputAccept, HandleOutputAcceptClicked);
        NotificationManager.Subscribe(ToolbarNotifications.OutputCancel, HandleOutputCancel);
        NotificationManager.Subscribe(InputNotifications.BackgroundClicked, HandleBackgroundClicked);
    }

    /// <summary>
    /// Toolbar: Output clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleOutputClicked(object data)
    {
        Debug.Log("Output clicked.");

        if (Game.Grid.SelectedCell != null)
        {
            outputCell = Game.Grid.SelectedCell;
            NotificationManager.Broadcast("arrow", outputCell);
        }
        else
        {
            Debug.Log("No cell selected.");
        }
    }

    /// <summary>
    /// Output select clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleOutputAcceptClicked(object data)
    {
        Cell inputCell = Game.Grid.SelectedCell;

        if (inputCell.Building != null && outputCell != null)
        {
            Debug.Log("Creating connection: " + outputCell.Id + " -> " + inputCell.Id);

            outputCell.Building.Output = inputCell.Building.Id;
            inputCell.Building.Inputs.Add(outputCell.Building.Id);

            ConnectionModel connection = new ConnectionModel(outputCell.Id, inputCell.Id);

            NotificationManager.Broadcast(ConnectionNotifications.Created, connection);
            NotificationManager.Broadcast(GameNotifications.Invalidated);
        }
        else
        {
            Debug.Log("Building doesn't exist at input cell.");
        }
    }

    /// <summary>
    /// Output cancel clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleOutputCancel(object data)
    {
        outputCell = null;
    }

    /// <summary>
    /// Background clicked.
    /// </summary>
    /// <param name="data"></param>
    private void HandleBackgroundClicked(object data)
    {
        outputCell = null;
    }
}
