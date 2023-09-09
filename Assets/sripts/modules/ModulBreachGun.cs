using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulBreachGun : ModulBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
       // tankTowerController.DamagedBreachGun();
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        //tankTowerController.enabled = false;
    }
}
