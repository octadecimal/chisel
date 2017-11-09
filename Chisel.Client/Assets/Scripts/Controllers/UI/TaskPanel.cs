using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Content;


    // START
    // ========================================================================

    void Start()
    {
        NotificationManager.Subscribe(TaskNotifications.Added, HandleTaskAdded);
    }


    // PUBLIC
    // ========================================================================

    public void Build(GameModel gameModel)
    {
        foreach (Task task in gameModel.RunningTasks)
        {
            AddTaskStatus(task);
        }

        foreach (Task task in gameModel.FinishedTasks)
        {
            AddTaskStatus(task);
        }
    }


    // PRIVATE
    // ========================================================================

    private void AddTaskStatus(Task task)
    {
        if(!task.Repeating)
        { 
            GameObject prefab = Instantiate(Prefab, Content.transform);
            TaskStatus prefabController = prefab.GetComponent<TaskStatus>();

            prefabController.SetTask(task);
        }
    }


    // NOTIFICATIONS
    // ========================================================================

    private void HandleTaskAdded(object data)
    {
        Task task = (Task)data;

        AddTaskStatus(task);
    }
}
