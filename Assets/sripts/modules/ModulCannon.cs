using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCannon : ModulBase
{
    [SerializeField] private TankTowerController _tankTowerController;

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                _tankTowerController.enabled = false;
                break;

            case int n when (n <= 10):
                print("Modul damaged");

                
                break;


        }
    }

}
