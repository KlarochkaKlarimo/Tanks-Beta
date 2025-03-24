using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected GameObject _projectile;
    [SerializeField] protected int _ammoCapacity;
    [SerializeField] protected float _initialVelocity = 500f;
    protected int _currentAmmoCapacity;
    private bool _reloaded;

    private void Awake()
    {
        _reloaded = true;
        _currentAmmoCapacity = _ammoCapacity;
    }

    public virtual void Shoot()
    {
        var currentProjectile = Instantiate(_projectile, _shootPoint.position, transform.rotation);
        _currentAmmoCapacity--;
        Rigidbody rigidbody = currentProjectile.GetComponent<Rigidbody>();
        Vector3 currentVelocity = currentProjectile.transform.forward * _initialVelocity;
        rigidbody.velocity = currentVelocity;
        if (_currentAmmoCapacity <= 0)
        {
            _reloaded = false;
        }
    }

    public virtual void Reload()
    {
        _reloaded = true;
        _currentAmmoCapacity = _ammoCapacity;
    }

    public bool AbleToShoot()
    {
        return _reloaded;
    }

    void Update()
    {
        
    }    
}
