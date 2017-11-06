using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 targetPosition;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

		if(Input.GetKey("a"))
        {
            targetPosition.x -= 0.1f;
        }

        if(Input.GetKey("d"))
        {
            targetPosition.x += 0.1f;
        }

        if (Input.GetKey("w"))
        {
            targetPosition.z += 0.1f;
        }

        if (Input.GetKey("s"))
        {
            targetPosition.z -= 0.1f;
        }

        gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);

        Camera.main.GetComponent<Camera>().transform.Translate(Vector3.forward * 1f * Input.mouseScrollDelta.y);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
