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
    public Button CompleteButton;
    public Text Caption;


    // START
    // ========================================================================

    public void Start ()
    {
        CompleteButton.onClick.AddListener(HandleCompleteButtonClicked);
    }

    public void Dispose()
    {
        CompleteButton.onClick.RemoveAllListeners();
        transform.parent = null;
        Destroy(this);
    }


    // PUBLIC
    // ========================================================================

    public void SetTask(Task task)
    {
        Task = task;
    }

    public void Update ()
    {
        long timeRemaining = Task.FinishTime - DateTime.Now.Ticks;

        float progress = 1-(timeRemaining / (float)(Task.FinishTime - Task.StartTime));

        if(progress >= 1)
        {
            if(Task.AutoExecute)
            {
                Dispose();
            }
            else
            {

                ProgressContainer.SetActive(false);
                CompleteButton.gameObject.SetActive(true);
            }
        }
        else
        {
            Caption.text = GetTimeText(new DateTime(timeRemaining));
            SetProgress(progress);
        }
    }


    // PRIVATE
    // ========================================================================

    private void SetProgress(float value)
    {
        float amount = Mathf.Min(1f, value) * 170f;

        RectTransform rect = Progress.GetComponent<RectTransform>();
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, amount);
    }

    private string GetTimeText(DateTime time)
    {
        return String.Format("{0: HH:mm:ss}", time);
    }


    // EVENTS
    // ========================================================================

    private void HandleCompleteButtonClicked()
    {
        NotificationManager.Broadcast(TaskPanelNotifications.CompleteClicked, this);
    }
}