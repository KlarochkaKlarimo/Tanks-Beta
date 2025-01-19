using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VehicleInteract : Interactable
{
    [SerializeField] private Transform _playerPoint;
    private GameObject _player;
    private void Update()
    {
        if (_player != null && Input.GetKeyDown(KeyCode.Y))
        {
            _player.SetActive(true);
            _player.transform.position = _playerPoint.position;
            _player =null;
        }
    }
    public override void Interact(GameObject obj)
    {
        _player = obj;
        obj.SetActive(false);
    }
}
