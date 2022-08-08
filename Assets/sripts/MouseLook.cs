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
    private float mouseX;
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
            mouseX = Input.GetAxis("Mouse X") * RotationSensitivity * Time.deltaTime;
        }
       // float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(0, xRotation, 0);
        Debug.Log(transform.rotation.x - playerBody.transform.rotation.x );
        if (Math.Abs (transform.rotation.x - playerBody.transform.rotation.x )>0.0001)
        {
            playerBody.Rotate(Vector3.up * mouseX * -1f);
        }
    }
}
