using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chisel
{
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// Main game camera.
        /// </summary>
        public MainCamera MainCamera;

        /// <summary>
        /// Drag speed.
        /// </summary>
        public float DragSpeed = 0.1f;

        /// <summary>
        /// Position of mouse on last frame.
        /// </summary>
        Vector3 lastMousePosition;

        /// <summary>
        /// Target camera position.
        /// </summary>
        Vector3 targetPosition;


        // CORE
        // ========================================================================

        /// <summary>
        /// Start routine.
        /// </summary>
        public void Start()
        {
            lastMousePosition = Input.mousePosition;
        }

        /// <summary>
        /// Configure routine.
        /// </summary>
        /// <param name="model"></param>
        public void Configure(GameModel model)
        {
            
        }

        /// <summary>
        /// Update routine.
        /// </summary>
        public void FixedUpdate()
        {
            // Ignore over UI objects
            if(!IsPointerOverUIObject())
            {
                // Mouse 1
                if (Input.GetMouseButtonDown(0))
                {
                    CastInputHitRay();
                }

                // Mouse 2
                if(Input.GetMouseButtonDown(1))
                {
                    ClickDrag();
                }

                // Touch 1
                if(Input.touchCount == 1)
                {
                    // Touch began
                    if(Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        CastInputHitRay();
                    }

                    // Touch moved
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        TouchDrag();
                    }
                }

                // Touch 2
                if(Input.touchCount == 2)
                {
                    Pinch();
                }
            }

            // Lerp to target
            //MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetPosition, 0.15f);

            // Save mouse position
            lastMousePosition = Input.mousePosition;
        }


        // PRIVATE
        // ========================================================================

        private void ClickDrag()
        {
            Vector3 delta = (Input.mousePosition - lastMousePosition) * Time.fixedDeltaTime;

            Drag(delta);
        }

        private void TouchDrag()
        {
            Vector2 delta = Input.GetTouch(0).deltaPosition * Time.fixedDeltaTime;

            Drag(delta);
        }

        private void Drag(Vector3 delta)
        {
            targetPosition.x += (-delta.x * Mathf.Abs(Mathf.Max(1, delta.x))) * DragSpeed;
            targetPosition.z += (-delta.y * Mathf.Abs(Mathf.Max(1, delta.y))) * DragSpeed;
        }

        private void Pinch()
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

            // Zoom
            MainCamera.Camera.fieldOfView += delta * 0.1f;
            MainCamera.Camera.fieldOfView = Mathf.Clamp(MainCamera.Camera.fieldOfView, 0.1f, 179.9f);
        }

        private void CastInputHitRay()
        {
            RaycastHit hit;

            Ray ray = MainCamera.Camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Cell
                if (hit.collider.GetComponent<Cell>() != null)
                {
                    NotificationManager.Broadcast(InputNotifications.CellClicked, hit.collider.GetComponent<Cell>());
                }
            }
            else
            {
                // Background
                NotificationManager.Broadcast(InputNotifications.BackgroundClicked);
            }
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
}
