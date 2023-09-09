using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulGunner : CharacterBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        tankTowerController.Turret_Speed_Multiplier /=2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        tankTowerController.Turret_Speed_Multiplier = 0;
    }

    
}
