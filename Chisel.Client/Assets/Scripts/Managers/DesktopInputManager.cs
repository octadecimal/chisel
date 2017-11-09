using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopInputManager : MonoBehaviour
{
    public GameCamera GameCamera;
    public float DragSpeed = 0.1f;

    Vector3 lastMousePosition;
    Vector3 targetPosition;

    // Use this for initialization
    void Start ()
    {
        lastMousePosition = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update()
    {
        // Ignore if over a UI object
        if (!IsPointerOverUIObject())
        {
            // Mouse 0
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = GameCamera.Camera.ScreenPointToRay(Input.mousePosition);

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
                }
            }

            // Mouse 1
            if (Input.GetMouseButton(1))
            {
                Vector3 delta = (Input.mousePosition - lastMousePosition) * Time.fixedDeltaTime;

                targetPosition.x += (-delta.x * Mathf.Abs(Mathf.Max(1, delta.x))) * DragSpeed;
                targetPosition.z += (-delta.y * Mathf.Abs(Mathf.Max(1, delta.y))) * DragSpeed;
            }
        }

        // Lerp to target
        GameCamera.transform.position = Vector3.Lerp(GameCamera.transform.position, targetPosition, 0.15f);

        // Save mouse position
        lastMousePosition = Input.mousePosition;
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
