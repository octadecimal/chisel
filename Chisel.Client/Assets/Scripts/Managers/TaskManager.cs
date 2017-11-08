using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameController Game;
    public GameModel GameModel;

    Type[] taskTypes;


    public void Start ()
    {
        taskTypes = new Type[8];
        taskTypes[(int)TaskTypes.Explore] = typeof(ExploreTask);
        taskTypes[(int)TaskTypes.Build] = typeof(BuildTask);
        taskTypes[(int)TaskTypes.Produce] = typeof(ProduceTask);
        taskTypes[(int)TaskTypes.Transport] = typeof(TransportTask);
    }

    public void Build(GameModel gameModel)
    {
        GameModel = gameModel;

        DateTime now = DateTime.Now;

        Debug.Log("Running tasks: " + GameModel.RunningTasks.Count);
        Debug.Log("Finished tasks: " + GameModel.FinishedTasks.Count);

        // Handle running tasks
        foreach (Task task in GameModel.RunningTasks)
        {
            task.Game = Game;
        }

        // Handle finished tasks
        foreach (Task task in GameModel.FinishedTasks)
        {
            task.Game = Game;
        }

        // Fast-forward running tasks
        FastForwardRunningTasks(gameModel.Time);
    }

    public void FastForwardRunningTasks(long time)
    {
        // Get current time
        long now = DateTime.Now.Ticks;

        // Loop through each game second until we're caught up
        while(time < now)
        {
            // Increment 1 second
            time = TimeTools.AddSeconds(time, 1);

            // Update at time
            UpdateTasks(time);
        }

        Debug.Log("Fast fowarded. (" + ((float)(DateTime.Now.Ticks - now) / 100f)+"ms)");

        // Invalidate
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    public void Update()
    {
        UpdateTasks(DateTime.Now.Ticks);
    }

    public void UpdateTasks(long time)
    {
        Task task;

        // Update running tasks
        for (int i = 0; i < GameModel.RunningTasks.Count; i++)
        {
            // Get task
            task = GameModel.RunningTasks[i];

            // Check if task completed
            if (time >= task.FinishTime)
            {
                // Remove from running tasks and add to finished tasks
                if (!task.Repeating)
                {
                    GameModel.RunningTasks.Remove(task);
                    GameModel.FinishedTasks.Add(task);
                }

                // Broadcast
                NotificationManager.Broadcast(TaskNotifications.Finished, task);

                // Execute task if auto-execute is enabled
                if (task.AutoExecute)
                {
                    ExecuteTask(task);
                }

                // Repeat task
                if (task.Repeating)
                {
                    RepeatTask(task);
                }
            }
        }
    }

    public void AddTask(TaskTypes type, long seconds, int cellId, int value = 0)
    {
        // Get current time
        DateTime now = DateTime.Now;

        // Create task
        Task task = CreateTask(type);

        // Set cell properties
        task.Game = Game;
        task.EntityId = cellId;
        task.Value = value;

        // Derive start and finish times
        task.StartTime = now.Ticks;
        task.FinishTime = TimeTools.AddSeconds(task.StartTime, seconds);

        // Save task
        GameModel.RunningTasks.Add(task);

        Debug.Log("Task added: task="+task+" startTime=" +task.StartTime+" finishTime="+task.FinishTime);

        // Broadcast
        NotificationManager.Broadcast(TaskNotifications.Added, task);
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    private void RepeatTask(Task task)
    {
        //Debug.Log("Repeating task: " + task + " entity=" + task.EntityId + " value=" + task.Value);

        // Get current time
        DateTime now = DateTime.Now;

        // Derive task duration
        long taskTime = task.FinishTime - task.StartTime;

        // Derive new start and finish times
        task.StartTime += taskTime;
        task.FinishTime += taskTime;

        // Execute task
        ExecuteTask(task);
    }

    public void ExecuteTask(Task task)
    {
        //Debug.Log("Executing task: " + task + " entity=" + task.EntityId + " value=" + task.Value);

        // Remove from finished tasks list
        GameModel.FinishedTasks.Remove(task);

        // Execute task
        task.Execute();

        // Broadcast
        NotificationManager.Broadcast(TaskNotifications.Executed, task);
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    private Task CreateTask(TaskTypes type)
    {
        Task task = (Task)Activator.CreateInstance(taskTypes[(int)type]);

        return task;
    }
}
