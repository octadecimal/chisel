using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    List<Task> runningTasks;
    List<Task> finishedTasks;

    void Start ()
    {
        runningTasks = new List<Task>();
        finishedTasks = new List<Task>();
    }

    void Update()
    {
        Task task;

        // Get current time
        DateTime now = DateTime.Now;

        // Update running tasks
        for(int i = 0; i < runningTasks.Count; i++)
        {
            // Get task
            task = runningTasks[i];

            // Check if task completed
            if(now.Ticks >= task.FinishTime)
            {
                // Remove from running tasks and add to finished tasks
                runningTasks.Remove(task);
                finishedTasks.Add(task);

                // Broadcast
                NotificationManager.Broadcast(TaskNotifications.Finished, task);
            }
        }
    }

    public void AddTask(int type, double seconds)
    {
        // Get current time
        DateTime now = DateTime.Now;

        // Create task
        Task task = new Task();

        // Derive start and finish times
        task.StartTime = now.Ticks;
        task.FinishTime = now.AddSeconds(seconds).Ticks;

        // Save task
        runningTasks.Add(task);

        Debug.Log("Task added: startTime=" +task.StartTime+" finishTime="+task.FinishTime);
    }
}
