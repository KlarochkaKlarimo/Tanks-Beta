using System;
using UnityEngine;
using UnityEngine.Rendering;

public class thermalVision : MonoBehaviour
{
    public static Action<bool> thermalVisionAction;
    [SerializeField] private GameObject _thermalCamera;
    [SerializeField] private Camera _gunCamera;
    private bool _isOn;

    void Update()
    {
        if (_gunCamera.enabled == false)
        {
            if (_isOn)
            {
                _isOn = false;
                _thermalCamera.SetActive(_isOn);
                thermalVisionAction.Invoke(_isOn);
                UpdateThermalVision();
            }            
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            //_normalCamera.enabled = !_normalCamera.enabled;
            _isOn = !_isOn;
            _thermalCamera.SetActive(_isOn);
            thermalVisionAction.Invoke(_isOn);
            UpdateThermalVision();
        }       
    }

    void UpdateThermalVision()
    {
        
        var meshes = GameObject.FindObjectsOfType<MeshRenderer>();
        var lights = GameObject.FindObjectsOfType<Light>();
        if (_isOn) 
        {
            foreach (var mesh in meshes)
            {
                mesh.shadowCastingMode = ShadowCastingMode.Off;
            }
            foreach (var light in lights)
            {
                light.enabled = false;
            }
            return;
        }
        foreach (var mesh in meshes)
        {
            mesh.shadowCastingMode = ShadowCastingMode.On;
        }
        foreach (var light in lights)
        {
            light.enabled = true;
        }
    }
}
