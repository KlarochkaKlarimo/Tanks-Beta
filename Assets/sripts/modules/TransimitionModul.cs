using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransimitionModul : ModulBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        //tankWheelControl.steerTorque /=5;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
       // tankWheelControl.enabled = false;
    }

}
