using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float RotationSensitivity = 1f;

    public Transform playerBody;

    private float xRotation = 0f;
    private float mouseX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            mouseX = Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f) * RotationSensitivity * Time.deltaTime;
        }
       // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * -1;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(0, xRotation, 0);
        Debug.Log(Input.GetAxis("Mouse X"));
        if (Math.Abs (transform.rotation.x - playerBody.transform.rotation.x )>0.0001)
        {
            playerBody.Rotate(Vector3.up * mouseX );
        }
    }
}
