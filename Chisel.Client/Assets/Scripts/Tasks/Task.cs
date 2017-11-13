using System;

namespace Chisel
{
    /// <summary>
    /// 
    /// A task is responsible for performing a single, specialized action
    /// that typically takes a set amount of time to complete and typically also 
    /// has a set of specific conditions that must exist before the task may start.
    /// 
    /// A Task's lifecycle begins when it is added to the TaskManager, though the 
    /// task itself will not immediately start. Its state will initially be set to 
    /// `TaskState.STARTING`, effectively in a state where it exists idly and is 
    /// waiting to start.
    /// 
    /// Every update of the TaskManager, it will invoke the newly added Task's 
    /// ValidateStart() method. We may optionally override this method in our Task 
    /// subclasses to provide specific requirements before a Task may start.
    /// 
    /// For example, we may have a task that is responsible for producing an item 
    /// that requires a known number of input materials. The ValidateStart() 
    /// method can be used to check if all of the required materials exist, and 
    /// return the result as a simple boolean.
    /// 
    /// Once a Task's ValidateStart() returns true:
    /// - The Task's Start() method will be invoked. 
    /// - The Task's state will be set to `TaskState.RUNNING`
    /// - The Task's StartTime and FinishTime will be set relative to the time of
    ///   when the Task was started (now).
    /// 
    /// Once a Task is in the RUNNING state:
    /// - The Task will remain in RUNNING state until the Task's FinishTime time 
    ///   is reached.
    /// 
    /// Once a Task's `FinishTime` has been reached:
    /// - If the Task's AutoComplete property is set to true, its Complete()
    ///   method will immediately and automatically be invoked.
    /// - Conversely, if the Task's AutoComplete property is set to false, it will 
    ///   be moved into the TaskManager's `FinishedTasks` list and the Task's state
    ///   will be set to `TaskState.Completed`, allowing the user to explicitly 
    ///   invoke the Task's Complete() at their own discretion at any time.
    /// - If the Task's AutoComplete and Repeating properties are set to true, the 
    ///   TaskManager will continue tracking the task and restart it as if it were 
    ///   newly added, using the same parameters (unless overridden by the 
    ///   programmer) and repeating the process over again.
    /// 
    /// The ValidateStart() method is where we should check if the necessary
    /// conditions exist in order to start this task. For example, ensuring we
    /// have a required amount of resources exist.
    /// 
    /// The Start() method should be where we consume or act upon the conditions
    /// ValidateStart() checked for. For example, we now consume the resources
    /// we checked for in ValidateStart().
    /// 
    /// The Complete() method should thus be where we finally act on the 
    /// 
    /// For example, in a transport task, the ValidateStart() method would check if
    /// our source entity has the required amount of resources. The Start() method 
    /// would then consume (remove) the items that we checked for in ValidateStart()
    /// from our source entity. The Complete() method would then be used to add our 
    /// previously consumed items to our destination entity.
    /// 
    /// </summary>

    [Serializable]
    public class Task
    {
        /// <summary>
        /// Task state.
        /// </summary>
        public TaskState State;

        /// <summary>
        /// If true, Task will automatically invoke its Complete() method when the 
        /// task's FinishTime is reached.
        /// </summary>
        public bool AutoComplete = true;

        /// <summary>
        /// If true, Task will automatically be re-run after its Complete() has 
        /// been invoked byreaching the task's FinishTime.
        /// </summary>
        public bool Repeating = false;

        /// <summary>
        /// Task start time.
        /// </summary>
        public long StartTime;

        /// <summary>
        /// Task finish time.
        /// </summary>
        public long FinishTime;

        /// <summary>
        /// Time this task should take to completed in seconds. Typically this 
        /// will be derived and set by the Task itself in the Start method.
        /// </summary>
        public long TaskTime;

        /// <summary>
        /// Game reference.
        /// </summary>
        [NonSerialized]
        protected Game Game;


        // PUBLIC
        // ========================================================================

        /// <summary>
        /// Configure routine.
        /// </summary>
        /// <param name="game"></param>
        public void Configure(Game game)
        {
            Game = game;
        }

        /// <summary>
        /// Start validation.
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateStart()
        {
            return true;
        }

        /// <summary>
        /// Start routine.
        /// </summary>
        public virtual void Start()
        {
            return;
        }

        /// <summary>
        /// Complete validation.
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidateComplete()
        {
            return true;
        }

        /// <summary>
        /// Complete routine.
        /// </summary>
        public virtual void Complete()
        {
            return;
        }
    }
}