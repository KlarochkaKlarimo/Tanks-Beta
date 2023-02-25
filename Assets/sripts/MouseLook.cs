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
    [SerializeField] private Transform rayStartPoint;

    public float RotationSensitivity = 1f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public void SetIsTest(bool isTest)
    {
        _isTest = isTest;
    }

    // Start is called before the first frame update
    void Start()
    {
        rayStartPoint = transform;
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
        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0);


        var ray = new Ray(rayStartPoint.position, rayStartPoint.forward);
        var target = ray.origin + ray.direction * 1000f;
        Debug.DrawRay(transform.position, target, Color.red);
        Quaternion endRotation = Quaternion.LookRotation(target - transform.position);
        playerBody.rotation = Quaternion.Slerp(playerBody.rotation, endRotation, Time.deltaTime * RotationSensitivity);
        playerBody.rotation = Quaternion.Euler(0f, playerBody.eulerAngles.y, 0f);

        if (Input.GetAxis("Mouse Y") != 0)
        {
            var roundedAxisY = GetAxis(Input.GetAxis("Mouse Y"));
            mouseY = roundedAxisY * RotationSensitivity * Time.deltaTime;
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
