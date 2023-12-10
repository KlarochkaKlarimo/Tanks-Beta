using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemperatureTest : MonoBehaviour
{
    [SerializeField] private Material[] _thermalMaterials;
    private Material[] _normalMaterials;
    private MeshRenderer _meshRenderer;
    [SerializeField] private bool _isHeatsUp;
    private float _themperature;
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _normalMaterials = _meshRenderer.materials;
        thermalVision.thermalVisionAction += ThermalVisionAction;
    }
    public void SetIsHeatsUp(bool value)
    {
        _isHeatsUp = value;
    }
    private void Update()
    {
        if (_isHeatsUp)
        {
            _themperature += 5 * Time.deltaTime;
        }
        else
        {
            _themperature -= 5 * Time.deltaTime;
        }
        foreach (var material in _thermalMaterials)
        {
            material.SetFloat("_Themperature", _themperature);
        }
    }
    private void ThermalVisionAction(bool isOn)
    {
        if (isOn)
        {
            _meshRenderer.materials = _thermalMaterials;
            return;
        }
        _meshRenderer.materials = _normalMaterials;
    }
    private void OnDestroy()
    {
        thermalVision.thermalVisionAction -= ThermalVisionAction;
    }
}
