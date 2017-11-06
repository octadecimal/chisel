using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour
{
    public GameObject GridObject;
    public GridController Grid;

	// Use this for initialization
	void Start ()
    {
        Grid = GridObject.GetComponent<GridController>();

        CellController cell = Grid.GetCell(0);

        this.transform.position = new Vector3(cell.transform.position.x, .2f, cell.transform.position.z);

        Debug.Log("Starting...");
        //StartCoroutine("Test");
        Test2();

        
	}

    void Test2()
    {
        //yield return new WaitForSeconds(2);

        Debug.Log("Starting movement");

        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        CellController cell = Grid.GetCell(10);

        agent.destination = cell.transform.position;

        //yield return null;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
