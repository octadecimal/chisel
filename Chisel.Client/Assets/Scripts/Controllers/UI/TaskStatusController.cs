using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskStatusController : MonoBehaviour
{
    public Task Task;

    public GameObject Progress;
    public GameObject ProgressContainer;
    public GameObject CompleteButton;
    public Text Caption;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	void Update ()
    {
        long timeRemaining = Task.FinishTime - DateTime.Now.Ticks;

        float progress = 1-(timeRemaining / (float)(Task.FinishTime - Task.StartTime));

        if(progress >= 1)
        {
            if(Task.AutoExecute)
            {
                transform.parent = null;
                Destroy(this);
            }
            else
            {
                ProgressContainer.SetActive(false);
                CompleteButton.SetActive(true);
            }
        }
        else
        {
            Caption.text = new DateTime(timeRemaining).Second.ToString();
            SetProgress(progress);
        }
    }

    public void SetTask(Task task)
    {
        Task = task;
    }

    void SetProgress(float value)
    {
        float amount = Mathf.Min(1f, value) * 170f;

        Debug.Log(amount);

        RectTransform rect = Progress.GetComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, amount);
    }
}
