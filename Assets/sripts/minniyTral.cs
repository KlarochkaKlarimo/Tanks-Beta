using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minniyTral : MonoBehaviour
{   
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;

    void Update()
    {
        if (Input.GetKey(KeyCode.PageDown))
        {
            _rotation = Vector3.right;
            transform.Rotate(_rotation * _speed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.PageUp))
        {
            _rotation = Vector3.left;
            transform.Rotate(_rotation * _speed * Time.deltaTime);
        }
    }
}
