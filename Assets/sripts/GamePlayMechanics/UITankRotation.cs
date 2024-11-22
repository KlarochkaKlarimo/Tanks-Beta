using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITankRotation : MonoBehaviour
{
    [SerializeField] private Transform _turretImage;
    [SerializeField] private Transform _turretReal;

    [SerializeField] private Transform _hullImage;
    [SerializeField] private Transform _hullReal;

    [SerializeField] private Transform _cameraReal;

    private void Update()
    {
        _turretImage.localRotation = Quaternion.Euler(0, 0, _turretReal.localEulerAngles.y * -1);
        _hullImage.rotation = Quaternion.Euler(0, 0, (_cameraReal.localEulerAngles.y - _hullReal.eulerAngles.y));
    }
}
