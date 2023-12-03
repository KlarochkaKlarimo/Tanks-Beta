using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thermalVision : MonoBehaviour
{
    [SerializeField] private Camera _normalCamera;
    [SerializeField] private GameObject _thermalCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //_normalCamera.enabled = !_normalCamera.enabled;
            _thermalCamera.SetActive(!_thermalCamera.activeInHierarchy);
        }       
    }
}
