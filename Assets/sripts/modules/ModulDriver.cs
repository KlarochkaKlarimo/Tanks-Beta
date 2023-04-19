using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulDriver : CharacterBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        tankWheelControl.steerTorque /=4;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        tankWheelControl.enabled = false;
    }

}
