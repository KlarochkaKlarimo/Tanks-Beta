using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingTankController : MonoBehaviour
{
    [SerializeField] private TankTesting _testingTank;
    [SerializeField] private TankTesting _playerTank;
    private bool isTestingTankControl;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isTestingTankControl)
            {
                isTestingTankControl = false;
                _testingTank.ChangeTankTestingStatus(false);
                _playerTank.ChangeTankTestingStatus(true);
            }
            else
            {
                isTestingTankControl = true;
                _testingTank.ChangeTankTestingStatus(true);
                _playerTank.ChangeTankTestingStatus(false);
            }
        }
    }
}

