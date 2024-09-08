using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLetalka : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _LookForTarget;
    private int _pointIndex;

    private void Update()
    {
        if (Vector3.Distance(_points[_pointIndex].position, transform.position) < 1)
        {
            _pointIndex++;

            if (_pointIndex >= _points.Length)
            {
                _pointIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, _points[_pointIndex].position, Time.deltaTime * _speed);
        transform.LookAt(_LookForTarget);
    }

}
