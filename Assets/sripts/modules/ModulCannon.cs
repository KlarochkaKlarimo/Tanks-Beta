using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCannon : ModulBase
{
    
    public override void modulDamaged()
    {
        base.modulDamaged();
        cannonFire.CannonDamage();
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        cannonFire.enabled = false;
    }
}
