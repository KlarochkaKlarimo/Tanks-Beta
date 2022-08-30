using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    [SerializeField] private GameObject _ammorackExplosion;
    public override void GetDamage()
    {
        base.GetDamage();
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                TankDestroed();
                break;

            case int n when (n <= 10):
                print("Modul damaged");

                tankTowerController.reloadingTime *= 2;
                break;

            
        }
    }

    public override void TankDestroed()
    {
        base.TankDestroed();
        _ammorackExplosion.SetActive(true);
    }
}
