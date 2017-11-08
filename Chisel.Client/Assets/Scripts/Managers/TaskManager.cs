using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // Reference to the game, allowing our tasks to have direct access
    public Game Game;

    // Reference to the game model
    public GameModel GameModel;

    // Task type lookup table
    Type[] taskTypes;
    

    // CORE
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    public void Start ()
    {
        taskTypes = new Type[8];
        taskTypes[(int)TaskTypes.Explore] = typeof(ExploreTask);
        taskTypes[(int)TaskTypes.Build] = typeof(BuildTask);
        taskTypes[(int)TaskTypes.Produce] = typeof(ProduceTask);
        taskTypes[(int)TaskTypes.Transport] = typeof(TransportTask);
    }

    /// <summary>
    /// Update routine.
    /// </summary>
    public void Update()
    {
        UpdateTasks(DateTime.Now.Ticks);
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Builds the task manager.
    /// </summary>
    /// <param name="gameModel"></param>
    public void Build(GameModel gameModel)
    {
        // Save game model
        GameModel = gameModel;

        // Debug
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

        // Fast-forward running tasks to present time
        FastForwardRunningTasks(gameModel.Time);
    }

    /// <summary>
    /// Adds a task.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="seconds"></param>
    /// <param name="entityId"></param>
    /// <param name="value"></param>
    public void AddTask(TaskTypes type, long seconds, int entityId, int value = 0)
    {
        // Get current time
        DateTime now = DateTime.Now;

        // Create task
        Task task = CreateTask(type);

        // Set cell properties
        task.Game = Game;
        task.EntityId = entityId;
        task.Value = value;

        // Derive start and finish times
        task.StartTime = now.Ticks;
        task.FinishTime = TimeTools.AddSeconds(task.StartTime, seconds);

        // Save task
        GameModel.RunningTasks.Add(task);

        // Start task
        task.Start();

        // Debug
        Debug.Log("Task added: task=" + task + " entity=" + entityId + " value=" + value + " startTime=" + task.StartTime + " finishTime=" + task.FinishTime);

        // Broadcast
        NotificationManager.Broadcast(TaskNotifications.Added, task);
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    /// <summary>
    /// Executes a task.
    /// </summary>
    /// <param name="task"></param>
    public void ExecuteTask(Task task)
    {
        Debug.Log("Executing task: " + task + " entity=" + task.EntityId + " value=" + task.Value);

        // Remove from finished tasks list
        GameModel.FinishedTasks.Remove(task);

        // Execute task
        task.Execute();

        // Broadcast
        NotificationManager.Broadcast(TaskNotifications.Executed, task);
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }


    // PRIVATE
    // ========================================================================

    /// <summary>
    /// Creates a task by type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Task CreateTask(TaskTypes type)
    {
        return (Task)Activator.CreateInstance(taskTypes[(int)type]);
    }

    /// <summary>
    /// Updates all running tasks.
    /// </summary>
    /// <param name="time"></param>
    private void UpdateTasks(long time)
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

    /// <summary>
    /// Repeats a task.
    /// </summary>
    /// <param name="task"></param>
    private void RepeatTask(Task task)
    {
        Debug.Log("Repeating task: " + task + " entity=" + task.EntityId + " value=" + task.Value);

        // Get current time
        DateTime now = DateTime.Now;

        // Derive task duration
        long taskTime = task.FinishTime - task.StartTime;

        // Derive new start and finish times
        task.StartTime += taskTime;
        task.FinishTime += taskTime;

        // Broadcast
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }

    /// <summary>
    /// Fast-forwards all running tasks to the passed time.
    /// </summary>
    /// <param name="time"></param>
    private void FastForwardRunningTasks(long time)
    {
        // Get current time
        long now = DateTime.Now.Ticks;

        // Loop through each game second until we're caught up
        while (time < now)
        {
            // Increment 1 second
            time = TimeTools.AddSeconds(time, 1);

            // Update at time
            UpdateTasks(time);
        }

        // Debug
        Debug.Log("Fast fowarded. (" + ((float)(DateTime.Now.Ticks - now) / 100f) + "ms)");

        // Invalidate
        NotificationManager.Broadcast(GameNotifications.Invalidated);
    }
}
