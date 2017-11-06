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
        taskTypes[TaskTypes.Explore] = typeof(ExploreTask);
        taskTypes[TaskTypes.Build] = typeof(BuildTask);
    }

    public void Build(GameModel gameModel)
    {
        GameModel = gameModel;

        Debug.Log("Running tasks: " + GameModel.RunningTasks.Count);
        Debug.Log("Finished tasks: " + GameModel.FinishedTasks.Count);

        foreach (Task task in GameModel.RunningTasks)
        {
            task.Game = Game;
        }

        foreach (Task task in GameModel.FinishedTasks)
        {
            task.Game = Game;
        }
    }

    public void Update()
    {
        Task task;

        // Get current time
        DateTime now = DateTime.Now;

        // Update running tasks
        for(int i = 0; i < GameModel.RunningTasks.Count; i++)
        {
            // Get task
            task = GameModel.RunningTasks[i];

            // Check if task completed
            if(now.Ticks >= task.FinishTime)
            {
                // Remove from running tasks and add to finished tasks
                GameModel.RunningTasks.Remove(task);
                GameModel.FinishedTasks.Add(task);

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

    public void AddTask(int type, double seconds, int cellId, int value = 0)
    {
        // Get current time
        DateTime now = DateTime.Now;

        // Create task
        Task task = CreateTask(type);

        // Set cell properties
        task.Game = Game;
        task.CellId = cellId;
        task.Value = value;

        // Derive start and finish times
        task.StartTime = now.Ticks;
        task.FinishTime = now.AddSeconds(seconds).Ticks;

        // Save task
        GameModel.RunningTasks.Add(task);

        Debug.Log("Task added: task="+task+" startTime=" +task.StartTime+" finishTime="+task.FinishTime);

        // Invalidate game state
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    public void ExecuteTask(Task task)
    {
        Debug.Log(this + " Executing task: " + task + " cell=" + task.CellId + " value=" + task.Value);

        // Remove from finished tasks list
        GameModel.FinishedTasks.Remove(task);

        // Execute task
        task.Execute();
    }

    private Task CreateTask(int type)
    {
        Task task = (Task)Activator.CreateInstance(taskTypes[type]);

        return task;
    }
}
