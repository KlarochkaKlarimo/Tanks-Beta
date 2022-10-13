using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulTowerRotation : ModulBase
{

    public override void modulDamaged()
    {
        base.modulDamaged();
        mouseLook.RotationSensitivity /= 2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        mouseLook.RotationSensitivity = 0;
    }
    
}
