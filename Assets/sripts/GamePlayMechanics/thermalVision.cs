using System;
using UnityEngine;
using UnityEngine.Rendering;

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
