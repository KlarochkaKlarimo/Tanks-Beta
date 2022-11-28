using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private bool _isTest;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float _roundedAxis;
    [SerializeField] private float _roundedAxisCamera;
    [SerializeField] private float _roundedAxisTower;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform playerGun;

    public float RotationSensitivity = 1f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float angle = 0f;

    public void SetIsTest(bool isTest)
    {
        _isTest = isTest;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTest)
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
            //var clamped = new Vector3(Mathf.Clamp(playerGun.rotation.x, -10, 10), 0);
            //angle += Input.GetAxis("Mouse Y") * RotationSensitivity*Time.deltaTime;
            //angle = Mathf.Clamp(angle, -10f, 10f);
            //playerGun.rotation = Quaternion.Euler(angle, 0.0f, 0.0f);

            //playerGun.Rotate(clamped * mouseY);
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
