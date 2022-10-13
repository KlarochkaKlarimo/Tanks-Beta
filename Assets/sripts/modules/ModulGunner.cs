using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulGunner : CharacterBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        mouseLook.RotationSensitivity /=2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        mouseLook.RotationSensitivity = 0;
    }

    
}
