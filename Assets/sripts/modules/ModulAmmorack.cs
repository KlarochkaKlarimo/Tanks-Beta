using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    [SerializeField] private GameObject _ammorackExplosion;
    [SerializeField] private GameObject _ammorackFire;
    [SerializeField] private Rigidbody _towerBody;

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
        _towerBody.transform.parent = null;
        var joint = _towerBody.GetComponent<HingeJoint>();
        Destroy(joint);
        _towerBody.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(800, 1200), Random.Range(-100, 100)));

    }

    private void ActiveteAmmorackFire()
    {
        _ammorackFire.SetActive(true);
    }


}
