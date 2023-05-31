using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    [SerializeField] private GameObject _ammorackExplosion;
    [SerializeField] private GameObject _ammorackFire;
    [SerializeField] private Rigidbody _towerBody;
    private Vector3 _destroedTowerRotation;
    public bool _tankDestroyed;

    private void FixedUpdate()
    {

        if (_tankDestroyed)
        {
            Quaternion delta = Quaternion.Euler(_destroedTowerRotation*Time.fixedDeltaTime);
            _towerBody.MoveRotation(delta*_towerBody.rotation);
        }
    }
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
        Invoke("ActiveteAmmorackFire", 1f);      
        _towerBody.transform.parent = null;
        var joint = _towerBody.GetComponent<HingeJoint>();
        Destroy(joint);
        _towerBody.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(800, 1200), Random.Range(-100, 100)));
        _destroedTowerRotation = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
        _tankDestroyed = true;
        
    }

    private void ActiveteAmmorackFire()
    {
        _ammorackFire.SetActive(true);
    }


}
