using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulTowerRotation : ModulBase
{

    public override void modulDamaged()
    {
        base.modulDamaged();
        mouseLook._horizontalSpeed /= 2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        mouseLook._horizontalSpeed = 0;
    }
    
}
