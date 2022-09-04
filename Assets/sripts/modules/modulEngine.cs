using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modulEngine : ModulBase
{
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                rearWheelDrive.enabled = false;
                break;

            case int n when (n <= 25):
                if (isModelDamaged)
                {
                    return;
                }
                isModelDamaged = true;
                print("Modul damaged");

                rearWheelDrive.maxTorque /=2;
                break;


        }
    }
}
    
