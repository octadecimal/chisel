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
    public int CellId;
    public bool AutoExecute = true;

    public Task()
    {

    }

    public virtual void Execute()
    {
        
    }
}
