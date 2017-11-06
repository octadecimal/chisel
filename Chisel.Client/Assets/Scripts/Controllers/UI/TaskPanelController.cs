using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanelController : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Content;

	void Start ()
    {
        NotificationManager.Subscribe(TaskNotifications.Added, HandleTaskAdded);
	}

    private void AddTaskStatus(Task task)
    {
        GameObject prefab = Instantiate(Prefab, Content.transform);
        TaskStatusController prefabController = prefab.GetComponent<TaskStatusController>();

        prefabController.SetTask(task);
    }

    private void HandleTaskAdded(object data)
    {
        Task task = (Task)data;

        AddTaskStatus(task);
    }
}
