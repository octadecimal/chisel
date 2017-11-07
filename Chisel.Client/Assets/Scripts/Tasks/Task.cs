using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Task
{
    [NonSerialized]
    public GameController Game;

    public long StartTime;
    public long FinishTime;
    public int EntityId;
    public int Value;
    public bool AutoExecute;
    public bool Repeating;

    public Task()
    {

    }

    public virtual void Execute()
    {
        
    }

    protected void SelectAndMoveToCell()
    {
        Game.Grid.SelectCell(EntityId);
        CellController cell = Game.Grid.GetCell(EntityId);
        Game.Camera.SetTargetPosition(cell.transform.position);
    }
}
