using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulDriver : CharacterBase
{
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        //tankWheelControl. /=4;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        //tankWheelControl.enabled = false;
    }

}
