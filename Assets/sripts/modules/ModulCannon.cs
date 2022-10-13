using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulCannon : ModulBase
{
    [SerializeField] private TankTowerController _tankTowerController;
    public override void modulDamaged()
    {
        base.modulDamaged();

    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        _tankTowerController.enabled = false;
    } 
}
