using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulAmmorack : ModulBase
{
    [SerializeField] private GameObject _ammorackExplosion;
    [SerializeField] private GameObject _ammorackFire;
    [SerializeField] private GameObject _tower;
    [SerializeField] private float _turretMass;
    [SerializeField] private bool _isDestroyTank;

    private Rigidbody _towerBody;
    
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
    public override void ModulDamaged()
    {
        base.ModulDamaged();
        //tankTowerController.reloadTime *= 2;
    }

    public override void ModulDestroyed()
    {
        base.ModulDestroyed();
        if (_isDestroyTank)
        {
            TankDestroed();
            return;
        }
        else
        {
            ActiveteAmmorackFire();
            cannonFire.enabled = false;
            Invoke("DeActivateAmmorackFire", 5f);
        }
    }

    public override void TankDestroed()
    {
        
        _ammorackExplosion.SetActive(true);
        Invoke("ActiveteAmmorackFire", 1f);      
        _tower.transform.parent = null;

        _towerBody = _tower.AddComponent<Rigidbody>();
        _towerBody.mass = _turretMass;
        _towerBody.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(1800, 2200), Random.Range(-100, 100)));
        _destroedTowerRotation = new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360));
        _tankDestroyed = true;
        base.TankDestroed();
    }

    private void ActiveteAmmorackFire()
    {
        _ammorackFire.SetActive(true);
    }

    private void DeActivateAmmorackFire()
    {
        _ammorackFire.SetActive(false);
    }
}
