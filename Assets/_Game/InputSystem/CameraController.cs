using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float ViewSensitivity = 0f;

    private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        // set mouse movement value
        float mouseX = Input.GetAxis("Mouse X") * ViewSensitivity;// * Time.deltaTime;
        float cameraX = Input.GetAxis("CameraX") * ViewSensitivity;// * Time.deltaTime;

        // Rotate camera around the Y axis


        if (Application.isPlaying)
        {
            Debug.Log("Application.isPlaying");
        }


        // THIS horrendous code is required because the editor runs at a very 
        // different rate to the rest of the outside world
        if (Application.isEditor)
        {
            parent.Rotate(Vector3.up, mouseX * Time.deltaTime);
            parent.Rotate(Vector3.up, cameraX * Time.deltaTime);
        }
        else
        {
            parent.Rotate(Vector3.up, mouseX / 8 * Time.deltaTime);
            parent.Rotate(Vector3.up, cameraX * Time.deltaTime);
        }
    }
}
