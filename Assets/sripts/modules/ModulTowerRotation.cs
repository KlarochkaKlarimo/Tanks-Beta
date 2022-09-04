using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulTowerRotation : ModulBase
{
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                mouseLook.RotationSensitivity = 0;
                break;

            case int n when (n <= 10):
                print("Modul damaged");
                mouseLook.RotationSensitivity /= 2;
                break;


        }
    }
}
