using System;
using UnityEngine;

public class thermalVision : MonoBehaviour
{
    public static Action<bool> thermalVisionAction;
    [SerializeField] private GameObject _thermalCamera;
    private bool _isOn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //_normalCamera.enabled = !_normalCamera.enabled;
            _isOn = !_isOn;
            _thermalCamera.SetActive(_isOn);
            thermalVisionAction.Invoke(_isOn);
        }       
    }
}
