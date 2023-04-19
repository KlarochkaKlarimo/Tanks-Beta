using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    [SerializeField] private GameObject _ammorackExplosion;
    [SerializeField] private GameObject _ammorackFire;

    public override void modulDamaged()
    {
        base.modulDamaged();
        tankTowerController.reloadTime *= 2;
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        TankDestroed();
    }

    public override void TankDestroed()
    {
        base.TankDestroed();
        _ammorackExplosion.SetActive(true);
        Invoke("ActiveteAmmorackFire", 2f);
    }

    private void ActiveteAmmorackFire()
    {
        _ammorackFire.SetActive(true);
    }
}
