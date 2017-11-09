using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputManager : MonoBehaviour
{
    public GameCamera GameCamera;
    public float DragSpeed = 0.1f;

    Vector3 targetPosition;

	// Use this for initialization
	void Start ()
    {
        targetPosition = new Vector3();
	}

    private void FixedUpdate()
    {
        // Not touching ui
        if(!IsPointerOverUIObject())
        { 
            // Single touch
            if (Input.touchCount == 1)
            {
                // Began
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    RaycastHit hit;
                    Ray ray = GameCamera.Camera.ScreenPointToRay(Input.GetTouch(0).position);

                    // Object hit
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Cell
                        if (hit.collider.GetComponent<Cell>() != null)
                        {
                            NotificationManager.Broadcast(CellNotifications.Clicked, hit.collider.GetComponent<Cell>());
                        }
                    }
                    else
                    {
                        // Background
                        NotificationManager.Broadcast(InputNotifications.BackgroundClicked);
                    }
                }

                // Moved
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 delta = Input.GetTouch(0).deltaPosition * Time.fixedDeltaTime;

                    targetPosition.x += (-delta.x * Mathf.Abs(Mathf.Max(1, delta.x))) * DragSpeed;
                    targetPosition.z += (-delta.y * Mathf.Abs(Mathf.Max(1, delta.y))) * DragSpeed;
                }
            }

            // Double touch
            if(Input.touchCount == 2)
            {
                // Save touches
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                // Get previous position
                Vector2 touch0previous = touch0.position - touch0.deltaPosition;
                Vector2 touch1previous = touch1.position - touch1.deltaPosition;

                // Magnitude
                float deltaPrevious = (touch0previous - touch1previous).magnitude;
                float deltaCurrent = (touch0.position - touch1.position).magnitude;

                // Delta
                float delta = deltaPrevious - deltaCurrent;

                GameCamera.Camera.fieldOfView += delta * 0.1f;

                GameCamera.Camera.fieldOfView = Mathf.Clamp(GameCamera.Camera.fieldOfView, 0.1f, 179.9f);
            }
        }

        // Lerp to target
        GameCamera.transform.position = Vector3.Lerp(GameCamera.transform.position, targetPosition, 0.2f);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
