using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameController Game;

    Type[] taskTypes;
    List<Task> runningTasks;
    List<Task> finishedTasks;

    void Start ()
    {
        runningTasks = new List<Task>();
        finishedTasks = new List<Task>();

        taskTypes = new Type[8];
        taskTypes[TaskTypes.Explore] = typeof(ExploreTask);
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

                // Execute task if auto-execute is enabled
                if(task.AutoExecute)
                {
                    ExecuteTask(task);
                }
            }
        }
    }

    public void AddTask(int type, double seconds, int cellId)
    {
        // Get current time
        DateTime now = DateTime.Now;

        // Create task
        Task task = CreateTask(type);

        // Set cell properties
        task.CellId = cellId;
        task.Game = Game;

        // Derive start and finish times
        task.StartTime = now.Ticks;
        task.FinishTime = now.AddSeconds(seconds).Ticks;

        // Save task
        runningTasks.Add(task);

        Debug.Log("Task added: startTime=" +task.StartTime+" finishTime="+task.FinishTime);
    }

    public void ExecuteTask(Task task)
    {
        // Remove from finished tasks list
        finishedTasks.Remove(task);

        // Execute task
        task.Execute();
    }

    private Task CreateTask(int type)
    {
        Task task = (Task)Activator.CreateInstance(taskTypes[type]);

        return task;
    }
}
