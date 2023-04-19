using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTesting : MonoBehaviour
{
    [SerializeField] private RTC_TankGunController tankTowerController;
    [SerializeField] private RTC_TankController tankWheelControl;
    [SerializeField] private RTC_MainCamera mouseLook;

    
    public void ChangeTankTestingStatus(bool status)
    {
        tankTowerController.enabled = status;
        tankWheelControl.enabled = status;
        mouseLook.currentTank = transform;
        
    }
}
