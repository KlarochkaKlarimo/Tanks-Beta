using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class ModulTowerRotation : ModulBase
{
    [SerializeField] private Turret_Horizontal_CS _horizontalControl;
    public override void modulDamaged()
    {
        base.modulDamaged();
        _horizontalControl.Speed_Mag /= 2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        _horizontalControl.Speed_Mag = 0;
    }

}
