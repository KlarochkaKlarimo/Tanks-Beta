using FuzzySharp.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponLogic : MonoBehaviour
{
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected GameObject _projectile;
    [SerializeField] protected float _initialVelocity = 500f;
    [SerializeField] protected List<Magazine> _magazines;
    private bool _reloaded;
    private Magazine _currentMagazine;

    private void Awake()
    {
        _reloaded = true;
        _currentMagazine = _magazines[0];
    }

    public virtual void Shoot()
    {
        var currentProjectile = Instantiate(_projectile, _shootPoint.position, transform.rotation);
        _currentMagazine.ammo--;
        Rigidbody rigidbody = currentProjectile.GetComponent<Rigidbody>();
        Vector3 currentVelocity = currentProjectile.transform.forward * _initialVelocity;
        rigidbody.velocity = currentVelocity;
        if (_currentMagazine.ammo <= 0)
        {
            _reloaded = false;
        }
    }

    public virtual void Reload()
    {
        if (_currentMagazine.ammo <= 0)
        {
            _magazines.Remove(_currentMagazine);
        }

        if (_magazines.Count == 0)
        {
            return;
        }

        _currentMagazine = _magazines.OrderByDescending(p => p.ammo).FirstOrDefault();
        _reloaded = true;
        Debug.Log(_currentMagazine.ammo);
    }

    public bool AbleToShoot()
    {
        return _reloaded;
    }

    void Update()
    {
        
    }    
}
