using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OutputArrowController : MonoBehaviour
{
    public GridController Grid;

    Vector3 target = new Vector3();

    public void Start()
    {
        NotificationManager.Subscribe("arrow", OnOutputClicked);
        NotificationManager.Subscribe(ToolbarNotifications.OutputAccept, OnOutputAccept);
        NotificationManager.Subscribe(ToolbarNotifications.OutputCancel, OnOutputCancel);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Grid.SelectedCell != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, Grid.SelectedCell.transform.position);
            gameObject.transform.localScale = new Vector3(1f, 1f, distance * 10);
            gameObject.transform.localRotation = Quaternion.LookRotation(gameObject.transform.position - Grid.SelectedCell.transform.position);
        }
    }

    private void OnOutputClicked(object data)
    {
        Debug.Log("Moving arrow");

        CellController selectedCell = (CellController)data;

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
}
