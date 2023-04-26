using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulGunner : CharacterBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        tankTowerController.horizontalSensitivity /=2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        tankTowerController.horizontalSensitivity = 0;
    }

    
}
