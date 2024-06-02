using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulBreachGun : ModulBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        cannonFire.DamagedBreachGun();
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        cannonFire.enabled = false;
    }
}
