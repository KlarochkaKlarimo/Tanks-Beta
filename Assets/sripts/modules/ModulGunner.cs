using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulGunner : ModulBase
{
    public override void GetDamage(int damage)
    {
        print("driver damaged");
        base.GetDamage(damage);
        switch (hp)
        {

            case 0:
                print("Modul destroed");
                tankTowerController.enabled = false;
                mouseLook.RotationSensitivity = 0;
                break;

            case int n when (n <= 3):
                if (isModelDamaged)
                {
                    return;
                }
                isModelDamaged = true;
                print("Modul damaged");

                mouseLook.RotationSensitivity /=2;
                break;


        }
    }
}
