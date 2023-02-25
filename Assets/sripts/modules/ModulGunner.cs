using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulGunner : CharacterBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        mouseLook._horizontalSpeed /=2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        mouseLook._horizontalSpeed = 0;
    }

    
}
