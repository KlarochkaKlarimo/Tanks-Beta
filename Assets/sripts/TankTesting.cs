using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTesting : MonoBehaviour
{
    [SerializeField] private TankTowerController tankTowerController;
    [SerializeField] private TankWheelControl tankWheelControl;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Camera camera;
    
    public void ChangeTankTestingStatus(bool status)
    {
        tankTowerController.SetIsTest(status);
        tankWheelControl.SetIsTest(status);
        mouseLook.SetIsTest(status);
        camera.gameObject.SetActive(!status);
    }
}
