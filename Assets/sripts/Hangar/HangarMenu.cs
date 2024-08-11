using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class HangarMenu : MonoBehaviour
{
    [SerializeField] private InputField _tankName;
    [SerializeField] private Slider _allAmmo;
    [SerializeField] private Slider _firstAmmoType;
    [SerializeField] private Slider _secondAmmoType;

    private TanksSettings _tanksSettings;
    private TanksSetting _tank;

    public void StartGame()
    {
        _tanksSettings.settings[0].bullets[0].ammoCount = (int)_firstAmmoType.value;
        _tanksSettings.settings[0].bullets[1].ammoCount = (int)_secondAmmoType.value;
    }

    private void Start()
    {
        _tanksSettings = Resources.Load<TanksSettings>("Tanks Settings");      
    }

    public void FindTank()
    {
        _tank = _tanksSettings.settings.FirstOrDefault(t => t.tankName == _tankName.text);
        if (_tank != null)
        {
            _firstAmmoType.maxValue = _tank.maxAmmorack;
            _secondAmmoType.maxValue = _tank.maxAmmorack;

            _allAmmo.maxValue = _tank.maxAmmorack;
            _firstAmmoType.value = _tank.bullets[0].ammoCount;
            _secondAmmoType.value = _tank.bullets[1].ammoCount;
            _allAmmo.value = _firstAmmoType.value + _secondAmmoType.value;
        }
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
