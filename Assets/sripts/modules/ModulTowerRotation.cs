using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class ModulTowerRotation : ModulBase
{
    [SerializeField] private Turret_Horizontal_CS _horizontalControl;
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        _horizontalControl.Speed_Mag /= 2;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        _horizontalControl.Speed_Mag = 0;
    }

}
