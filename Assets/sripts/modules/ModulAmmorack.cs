using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    [SerializeField] private GameObject _ammorackExplosion;
    [SerializeField] private GameObject _ammorackFire;
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
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
        Invoke("ActiveteAmmorackFire", 2f);
    }

    private void ActiveteAmmorackFire()
    {
        _ammorackFire.SetActive(true);
    }
}
