using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameModel
{
    public float Seed;
    public List<CellModel> Cells;

	void Start ()
    {

	}

    public void CreateNewGame()
    {
        Seed = 0;

        Cells = new List<CellModel>();
    }
}
