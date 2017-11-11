using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Chisel
{
    /// <summary>
    /// The TaskManager manages all tasks within the game. All active tasks are 
    /// stored in the game model, allowing for easy saving and serialization.
    /// 
    /// 
    /// A Task has 2 important parameters: `AutoComplete` and `Repeating`
    /// 
    /// `AutoComplete`: If true, when the task FinishTime has been reached, the 
    /// task will automatically complete. If false, the task will not complete 
    /// (allowing for later user interaction to trigger the completion), and will 
    /// be moved into the `finishedTasks` list.
    /// 
    /// `Repeat`: If a task is set to AutoComplete and Repeat, upon its completion 
    /// itwill immediately re-enqueue itself. This is useful for tasks that 
    /// produce or gather items repeatedly, whereas you would not want a task like
    /// constructing a building to repeat.
    /// 
    /// 
    /// A Task has 3 states: STARTING, RUNNING, and COMPLETED.
    /// 
    /// STARTING: Task has been added to the running task list, but has not yet 
    /// started due to its ValidateStart() not yet returning true.
    /// 
    /// RUNNING: Task has successfully been started, but is awaiting the for the
    /// task's finish time to be reached.
    /// 
    /// COMPLETED: Task has been successfully started and completed. This state will
    /// only be reached if the Task is not set to auto-complete.
    /// 
    /// </summary>
    public class TaskManager : MonoBehaviour
    {
        GameModel gameModel;


        // CORE
        // ========================================================================

        public void Configure(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void Update()
        {
            UpdateTasks(DateTime.Now.Ticks);
        }


        // PUBLIC
        // ========================================================================

        public void AddTask(Task task, TaskValues values)
        {
            // Initial task state
            task.State = TaskState.Starting;

            // Add as running task
            gameModel.RunningTasks.Add(task, values);

            // Notify
            NotificationManager.Broadcast(TaskNotifications.Added, task);
        }


        // PRIVATE
        // ========================================================================

        /// <summary>
        /// Updates all tasks at the current passed time.
        /// </summary>
        /// <param name="time"></param>
        private void UpdateTasks(long time)
        {
            // Handle task
            foreach (Task task in gameModel.RunningTasks)
            {
                // Task can start
                if (task.State == TaskState.Starting && task.ValidateStart())
                {
                    // Start
                    StartTask(task);

                    // Broadcast task
                    NotificationManager.Broadcast(TaskNotificaions.Started, task);
                }

                // Task can complete
                if (task.State == TaskState.Running && time >= task.FinishTime && task.ValidateComplete())
                {
                    // Task is set to auto-complete
                    if (task.AutoComplete)
                    {
                        // Complete task
                        CompleteTask(task);
                    }
                    else
                    {
                        // Defer for user to later manually complete task
                        gameModel.RunningTasks.Remove(task);
                        gameModel.FinishedTasks.Add(task);

                        // Broadcast task
                        NotificationManager.Broadcast(TaskNotificaions.ReadyToComplete, task);
                    }
                }
            }
        }

        /// <summary>
        /// Starts a task, flagging it as started and now running.
        /// </summary>
        /// <param name="task"></param>
        private void StartTask(Task task)
        {
            DateTime now = DateTime.Now;

            // Update task state
            task.State = TaskState.Running;

            // Set task times
            task.StartTime = now.Ticks;
            task.FinishTime = TimeUtility.AddSeconds(task.StartTime, seconds);

            // Start task
            task.Start();

            // Broadcast task
            NotificationManager.Broadcast(TaskNotificaions.TaskStarted, task);
        }

        /// <summary>
        /// Completes a task.
        /// </summary>
        /// <param name="task"></param>
        private void CompleteTask(Task task)
        {
            // Remove from finished tasks list
            gameModel.FinishedTasks.Remove(task);

            // Update task state
            task.State = TaskState.Completed;

            // Complete
            task.Complete();

            // Broadcast task
            NotificationManager.Broadcast(TaskNotificaions.TaskCompleted, task);

            // Repeat task
            if (task.Repeating)
            {
                // Repeat
                RepeatTask(task);
            }
        }

        /// <summary>
        /// Repeats an input task, effecitvely resetting it.
        /// </summary>
        /// <param name="task"></param>
        private void RepeatTask(Task task)
        {
            // Get time
            DateTime now = DateTime.Now;

            // Restart task state
            task.State = TaskState.Starting;

            // Set task times
            task.StartTime = task.FinishTime;
            task.FinishTime = TimeUtility.AddSeconds(task.StartTime, seconds);

            // Broadcast task
            NotificationManager.Broadcast(TaskNotificaions.Repeating, task);
        }

        /// <summary>
        /// Fast-forwards all tasks from the passed time up to the current iem.
        /// </summary>
        /// <param name="time"></param>
        private void FastForward(long time)
        {
            // Get current time
            long now = DateTime.Now.Ticks;

            // Loop through each game second until we're caught up
            while (time < now)
            {
                // Increment 1 second
                time = TimeUtility.AddSeconds(time, 1);

                // Update at time
                UpdateTasks(time);
            }
        }
    }
}