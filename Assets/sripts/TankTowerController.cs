using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTowerController : MonoBehaviour
{
    public bool isCannonDamaged;
    public float reloadingTime;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private Camera camera;
    [SerializeField] private ParticleSystem flesh;
    [SerializeField] private int _BulletPenetration;
    [SerializeField] private bool _isTest;
    [SerializeField] private int _misFireChance;
    private bool _isBreachGunDamaged;
    private bool _isMisFire;

    private bool isReloading;
    private Vector3 _destination;

    public void SetIsTest(bool isTest)
    {
        _isTest = isTest;
    }

    public void DamagedBreachGun()
    {
        _isBreachGunDamaged = true;

    }

    void Update()
    {
        if (_isTest)
        {
            return;
        }


        var ray = new Ray(ShootPoint.position, ShootPoint.forward);
        _destination = ray.origin + ray.direction * 1000f;
        Debug.DrawLine(ShootPoint.position, _destination, Color.cyan, 10f);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }


    }

    private void Shoot()
    {
        if (isReloading)
        {
            return;
        }
        if (_isBreachGunDamaged)
        {
            _isMisFire = Random.Range(0, 100)>= _misFireChance;
            if (_isMisFire)
            {
                return;
            }
        }
        
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        isReloading = true;
        Invoke("Reloading", reloadingTime);
        var _bullet = Instantiate(bullet, ShootPoint.position, transform.rotation);
        _bullet.GetComponent<bullet> ().SetVariables(_destination, _BulletPenetration, 40, isCannonDamaged);
        flesh.Play();
    }
    private void Reloading()
    {
        isReloading = false;

    }

    
}
