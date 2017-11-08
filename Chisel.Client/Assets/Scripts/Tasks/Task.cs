using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Task
{
    [NonSerialized]
    public GameController Game;

    /// <summary>
    /// If true, this task will auto-execute on completion.
    /// </summary>
    public bool AutoExecute;

    /// <summary>
    /// If true, this task will run again after execution.
    /// </summary>
    public bool Repeating;

    /// <summary>
    /// Task start time.
    /// </summary>
    public long StartTime;

    /// <summary>
    /// Task finish time.
    /// </summary>
    public long FinishTime;

    /// <summary>
    /// Id of entity associated with this task.
    /// </summary>
    public int EntityId;

    /// <summary>
    /// Optional value that can be set.
    /// </summary>
    public int Value;


    /// <summary>
    /// Constructor.
    /// </summary>
    public Task()
    {

    }

    /// <summary>
    /// Called when a task is first added.
    /// </summary>
    public virtual void Start()
    {
        
    }

    /// <summary>
    /// Called when a task is finished.
    /// </summary>
    public virtual void Execute()
    {
        
    }

    /// <summary>
    /// Utility method to select and move to the cell associated
    /// with this task.
    /// </summary>
    protected void SelectAndMoveToCell()
    {
        Game.Grid.SelectCell(EntityId);
        CellController cell = Game.Grid.GetCell(EntityId);
        Game.Camera.SetTargetPosition(cell.transform.position);
    }
}
