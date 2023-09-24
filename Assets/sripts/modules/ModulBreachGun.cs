using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulBreachGun : ModulBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        cannonFire.DamagedBreachGun();
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        cannonFire.enabled = false;
    }
}
