using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    public override void GetDamage()
    {
        base.GetDamage();
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                break;

            case int n when (n <= 10):
                print("Modul damaged");
                break;

            
        }
    }
}
