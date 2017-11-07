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
    }

    public void Build(GameModel gameModel)
    {
        GameModel = gameModel;

        DateTime now = DateTime.Now;

        Debug.Log("Running tasks: " + GameModel.RunningTasks.Count);
        Debug.Log("Finished tasks: " + GameModel.FinishedTasks.Count);

        foreach (Task task in GameModel.RunningTasks)
        {
            task.Game = Game;

            if(task.Repeating)
            {
                if(now.Ticks > task.FinishTime)
                {
                    long taskTime = task.FinishTime - task.StartTime;
                    long ticksPassed = now.Ticks - task.FinishTime;
                    long secondsPassed = ticksPassed / 10000000;
                    double runsToDo = 1 + (secondsPassed / (taskTime / 10000000));

                    Debug.Log("RESUMING RUNS: " + runsToDo);
                }
            }
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
                if (!task.Repeating)
                {
                    GameModel.RunningTasks.Remove(task);
                    GameModel.FinishedTasks.Add(task);
                }

                // Broadcast
                NotificationManager.Broadcast(TaskNotifications.Finished, task);

                // Execute task if auto-execute is enabled
                if(task.AutoExecute)
                {
                    ExecuteTask(task);
                }

                // Repeat task
                if(task.Repeating)
                {
                    RepeatTask(task);
                }
            }
        }
    }

    public void AddTask(TaskTypes type, double seconds, int cellId, int value = 0)
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

        // Broadcast
        NotificationManager.Broadcast(TaskNotifications.Added, task);
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    private void RepeatTask(Task task)
    {
        Debug.Log(this + " Repeating task: " + task + " cell=" + task.CellId + " value=" + task.Value);

        // Get current time
        DateTime now = DateTime.Now;

        // Derive task duration
        long taskTime = task.FinishTime - task.StartTime;

        // Derive new start and finish times
        task.StartTime = now.Ticks;
        task.FinishTime = now.AddSeconds(new DateTime(taskTime).Second).Ticks;

        // Broadcast
        //NotificationManager.Broadcast(TaskNotifications.Added, task);
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    public void ExecuteTask(Task task)
    {
        Debug.Log(this + " Executing task: " + task + " cell=" + task.CellId + " value=" + task.Value);

        // Remove from finished tasks list
        GameModel.FinishedTasks.Remove(task);

        // Execute task
        task.Execute();

        // Broadcast
        NotificationManager.Broadcast(TaskNotifications.Executed, task);
    }

    private Task CreateTask(TaskTypes type)
    {
        Task task = (Task)Activator.CreateInstance(taskTypes[(int)type]);

        return task;
    }
}
