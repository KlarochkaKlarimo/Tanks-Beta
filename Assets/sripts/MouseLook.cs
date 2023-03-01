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
    [SerializeField] private Transform _target;
    [SerializeField] private float _verticalSpeed;

    public float _horizontalSpeed = 1f;
    private float xRotation = 0f;
    private float yRotation = 0f;

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
        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0);


        Vector3 targetDirection = new Vector3(_target.position.x, playerBody.position.y, _target.position.z) /*_taget.position*/ - playerBody.position;

        // The step size is equal to speed times frame time.
        float singleStep = _horizontalSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(playerBody.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(playerBody.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        playerBody.rotation = Quaternion.LookRotation(newDirection);


        if (Input.GetAxis("Mouse Y") != 0)
        {
            var roundedAxisY = GetAxis(Input.GetAxis("Mouse Y"));
            mouseY = roundedAxisY * _verticalSpeed * Time.deltaTime;
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
