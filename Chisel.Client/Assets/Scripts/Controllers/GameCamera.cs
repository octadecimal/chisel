using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameCamera : MonoBehaviour
{
    public Camera Camera;

    Vector3 targetPosition;

    // Use this for initialization
    void Start () {
		
	}

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUIObject())
            {
                /*RaycastHit hit;
                Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.GetComponent<Cell>() != null)
                    {
                        NotificationManager.Broadcast(CellNotifications.Clicked, hit.collider.GetComponent<Cell>());
                    }
                }
                else
                {
                    NotificationManager.Broadcast(InputNotifications.BackgroundClicked);
                }*/
            }
        }

        if (Input.GetKey("a"))
        {
            targetPosition.x -= 5f * Time.deltaTime;
        }

        if(Input.GetKey("d"))
        {
            targetPosition.x += 5f * Time.deltaTime;
        }

        if (Input.GetKey("w"))
        {
            targetPosition.z += 5f * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            targetPosition.z -= 5f * Time.deltaTime;
        }

        //gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);

        //Camera.transform.Translate(Vector3.forward * 1f * Input.mouseScrollDelta.y);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
