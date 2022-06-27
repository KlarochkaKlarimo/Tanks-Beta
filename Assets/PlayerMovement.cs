using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody controller;

    public float speed = 2;
    public float rotationSpeed = 3f;
    private bool isOnGround;
    private Vector3 velocity;

    public void setIsOnGround(bool flag)
    {
        isOnGround = flag;
    }
    private void Start()
    {
        controller = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (isOnGround)
        {
            if (z != 0)
            {
                controller.AddForce(transform.forward * speed);
               // controller.MovePosition(transform.position + (transform.forward * (z * speed * Time.deltaTime)));
                if (x!=0)
                {
                    //transform.rotation = Quaternion.Euler(0f, transform.rotation.x +(Time.deltaTime * rotationSpeed * x), 0f);
                    transform.Rotate(Vector3.up * x * rotationSpeed);
                }
            }
        }
        
    }  
}