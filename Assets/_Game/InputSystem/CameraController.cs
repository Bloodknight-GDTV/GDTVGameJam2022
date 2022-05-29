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
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        // set mouse movement value
        float mouseX = Input.GetAxis("Mouse X") * ViewSensitivity * Time.deltaTime;
        float cameraX = Input.GetAxis("CameraX") * ViewSensitivity * Time.deltaTime;

        // Rotate camera around the Y axis
        parent.Rotate(Vector3.up, mouseX);
        parent.Rotate(Vector3.up, cameraX);
    }
}
