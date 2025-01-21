using ChobiAssets.PTM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleInteract : Interactable
{
    [SerializeField] private Transform _playerPoint;
    private GameObject _player;
    private Drive_Control_CS _driveControlScript;
    private Aiming_Control_CS _aimControlScript;
    private Turret_Horizontal_CS _horizntalScript;
    private Cannon_Vertical_CS[] _verticalScript;
    private Cannon_Fire_CS[] _fireScript;
    private AnotherWeapon[] _anotherWeaponScript;
    private Gun_Camera_CS[] _gunCameras;
    private GameObject _canvasScript;
    private void Awake()
    {
        _driveControlScript = GetComponent<Drive_Control_CS>();
        _aimControlScript = GetComponent<Aiming_Control_CS>();
        _horizntalScript = GetComponentInChildren<Turret_Horizontal_CS>();
        _verticalScript = GetComponentsInChildren<Cannon_Vertical_CS>();
        _fireScript = GetComponentsInChildren<Cannon_Fire_CS>();    
        _anotherWeaponScript = GetComponentsInChildren<AnotherWeapon>();
        _gunCameras = GetComponentsInChildren<Gun_Camera_CS>();
        _canvasScript = GetComponentInChildren<LaserRangeFinder>().gameObject;
        ChangeVehicleState(false);
    }

    private void ChangeVehicleState(bool statka)
    {
        _driveControlScript.enabled = statka;
        _horizntalScript.enabled = statka;
        _aimControlScript.enabled = statka;
        _canvasScript.SetActive(statka);
        foreach (Cannon_Vertical_CS cannon_Vertical_CS in _verticalScript) // TODO chahge to actions
        {
            cannon_Vertical_CS.enabled = statka;
        }
        foreach (Cannon_Fire_CS cannon_Fire_CS in _fireScript)
        {
            cannon_Fire_CS.enabled = statka;
        }
        foreach (AnotherWeapon anotherWeapon in _anotherWeaponScript)
        {
            anotherWeapon.enabled = statka;
        }
        foreach (Gun_Camera_CS gun_Camera_CS in _gunCameras)
        {
            gun_Camera_CS.isAbleToControl = statka; 
        }
    }

    private void Update()
    {
        if (_player != null && Input.GetKeyDown(KeyCode.Y))
        {
            _player.SetActive(true);
            _player.transform.position = _playerPoint.position;
            _player =null;
            ChangeVehicleState(false);
        }
    }
    public override void Interact(GameObject obj)
    {
        _player = obj;
        obj.SetActive(false);
        ChangeVehicleState(true);
    }
}
