using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransimitionModul : ModulBase
{
    public override void modulDamaged()
    {
        base.modulDamaged();
        rearWheelDrive.rotationSpeed /=2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        rearWheelDrive.enabled = false;
    }

}
