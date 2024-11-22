using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class HangarMenu : MonoBehaviour
{
    [SerializeField] private Slider _allAmmo;
    [SerializeField] private Slider _firstAmmoType;
    [SerializeField] private Slider _secondAmmoType;
    [SerializeField] private Transform _tankListContent;
    [SerializeField] private TankCartButton _tankCartPrefab;

    private TanksSettingsCollection _tanksSettings;
    private TanksSetting _tank;

    public void StartGame()
    {
        _tanksSettings.settings[0].bullets[0].ammoCount = (int)_firstAmmoType.value;
        _tanksSettings.settings[0].bullets[1].ammoCount = (int)_secondAmmoType.value;

        SceneManager.LoadScene("Exam_Field");
    }

    private void Start()
    {
        _tanksSettings = Resources.Load<TanksSettingsCollection>("Tanks Settings");  
        foreach (var tank in _tanksSettings.settings)
        {
            var cart = Instantiate(_tankCartPrefab, _tankListContent);
            cart.SetData(this, tank);
        }
    }

    public void FindTank(TanksSetting tank)
    {
        _tank = tank; 
        
        _firstAmmoType.maxValue = _tank.maxAmmorack;
        _secondAmmoType.maxValue = _tank.maxAmmorack;

        _allAmmo.maxValue = _tank.maxAmmorack;
        _firstAmmoType.value = _tank.bullets[0].ammoCount;
        _secondAmmoType.value = _tank.bullets[1].ammoCount;
        _allAmmo.value = _firstAmmoType.value + _secondAmmoType.value;
       
    }

    public void FirstAmmoTypeChahged(Single value)
    {
        if (_firstAmmoType.value + _secondAmmoType.value >_tank.maxAmmorack)
        {
            _secondAmmoType.value = _tank.maxAmmorack - _firstAmmoType.value;
        }
    }

    public void SecondAmmoTypeChahged(Single value)
    {
        if (_firstAmmoType.value + _secondAmmoType.value >_tank.maxAmmorack)
        {
            _firstAmmoType.value = _tank.maxAmmorack - _secondAmmoType.value;
        }
    }
}
