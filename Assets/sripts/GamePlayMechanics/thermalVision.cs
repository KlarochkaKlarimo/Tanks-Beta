using System;
using UnityEngine;
using UnityEngine.Rendering;

public class thermalVision : MonoBehaviour
{
    public static Action<bool> thermalVisionAction;
    [SerializeField] private Camera _thermalCamera;
    //[SerializeField] private GameObject _pixelCanvas;
    [SerializeField] private Camera _gunCamera;
    private GameObject _thermalCameraGameObject;
    private GameObject _gunCameraGameObject;
    private bool _isOn;

    private void Awake()
    {
        _thermalCameraGameObject = _thermalCamera.gameObject;
    }
    void Update()
    {
        if (_isOn)
        {
            
            if (_gunCamera.enabled == false)
            {
                _isOn = false;
                _thermalCameraGameObject.SetActive(_isOn);
                thermalVisionAction.Invoke(_isOn);
                UpdateThermalVision();
            }

            else
            {
                _thermalCamera.fieldOfView = _gunCamera.fieldOfView;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.C))
        {
            //_normalCamera.enabled = !_normalCamera.enabled;
            _isOn = !_isOn;
            _thermalCameraGameObject.SetActive(_isOn);
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
