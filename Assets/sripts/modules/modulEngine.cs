using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modulEngine : ModulBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        tankWheelControl.Torque /=5;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        tankWheelControl.enabled = false;
    }

}
    
