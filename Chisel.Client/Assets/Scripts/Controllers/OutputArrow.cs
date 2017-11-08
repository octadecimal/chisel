using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OutputArrow : MonoBehaviour
{
    public Grid Grid;

    Vector3 target = new Vector3();

    public void Start()
    {
        NotificationManager.Subscribe("arrow", OnOutputClicked);
        NotificationManager.Subscribe(ToolbarNotifications.OutputAccept, OnOutputAccept);
        NotificationManager.Subscribe(ToolbarNotifications.OutputCancel, OnOutputCancel);
        NotificationManager.Subscribe(CellNotifications.Clicked, OnCellClicked);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Grid.SelectedCell != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, Grid.SelectedCell.transform.position);
            gameObject.transform.localScale = new Vector3(1f, 1f, distance * 10);

            if ((gameObject.transform.position - Grid.SelectedCell.transform.position).magnitude > 0)
            {
                gameObject.transform.localRotation = Quaternion.LookRotation(gameObject.transform.position - Grid.SelectedCell.transform.position);
            }
        }
    }

    private void OnOutputClicked(object data)
    {
        Debug.Log("Moving arrow");

        Cell selectedCell = (Cell)data;

        gameObject.transform.parent = selectedCell.transform;
        gameObject.transform.position = selectedCell.transform.position;
        //gameObject.transform.Translate(0, 1f, 0);

        gameObject.SetActive(true);
    }

    private void OnOutputAccept(object data)
    {
        gameObject.SetActive(false);
    }

    private void OnOutputCancel(object data)
    {
        gameObject.SetActive(false);
    }

    private void OnCellClicked(object data)
    {
        /*CellController cell = (CellController)data;

        bool hasConnection = cell.Building != null && (cell.Building.Output != null || cell.Building.Inputs.Count > 0);

        if(hasConnection)
        {
            Debug.Log("Has connection: " + cell);

            gameObject.SetActive(true);

            gameObject.transform.parent = cell.transform;
            gameObject.transform.position = cell.transform.position;

            float distance = Vector3.Distance(gameObject.transform.position, cell.Building.Output.transform.position);
            gameObject.transform.localScale = new Vector3(1f, 1f, distance * 10);

            if ((gameObject.transform.position - cell.Building.Output.transform.position).magnitude > 0)
            {
                gameObject.transform.localRotation = Quaternion.LookRotation(gameObject.transform.position - cell.Building.Output.transform.position);
            }
        }
        else
        {
            Debug.Log("No connection.");
        }*/
    }
}
