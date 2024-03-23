using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minniyTral : MonoBehaviour
{   
    private Vector3 _rotation;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _speed;

    void Update()
    {       
        if (Input.GetKey(KeyCode.PageDown) && transform.localEulerAngles.x < _maxValue)
        {
            _rotation = Vector3.right;
            transform.Rotate(_rotation * _speed * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.PageUp) && transform.localEulerAngles.x > _minValue)
        {
            _rotation = Vector3.left;
            transform.Rotate(_rotation * _speed * Time.deltaTime);
        }
    }
}
