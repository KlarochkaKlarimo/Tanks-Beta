using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public bool isTest;
    public float mouseSensitivity = 100f;
    public float RotationSensitivity = 1f;
    public float _roundedAxis;
    public float _roundedAxisCamera;
    public float _roundedAxisTower;
    public Transform playerBody;
    public Transform playerGun;

    private float xRotation = 0f;
    public float yRotation = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTest)
        {
            return;
        }

        xRotation -= Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * -1;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -10, 10);
        // -=Mathf.Clamp( Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime ,-10,10);
        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0);
        if (Input.GetAxis("Mouse X") != 0)
        {
            var roundedAxisX = GetAxis(Input.GetAxis("Mouse X"));
            var roundedAxisY = GetAxis(Input.GetAxis("Mouse Y"));
            mouseX = roundedAxisX * RotationSensitivity * Time.deltaTime;
            mouseY = roundedAxisY * RotationSensitivity * Time.deltaTime;
        }
        if (Math.Abs(transform.localEulerAngles.y - playerBody.transform.localEulerAngles.y )>1)
        {
            playerBody.Rotate(Vector3.up * mouseX );
            //ус
        }
        if (Math.Abs(transform.localEulerAngles.x - playerGun.transform.localEulerAngles.x) > 1)
        {
            playerGun.Rotate(Vector3.left * mouseY);
        }
    }
    private float GetAxis(float input)
    {
        if (input > 0)
        {
            return 1;
        }
        return -1;
    }
}
