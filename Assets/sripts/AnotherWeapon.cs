using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class AnotherWeapon : MonoBehaviour
{
    [SerializeField] private Bullet_Generator_CS _linkForGenerator;
    [SerializeField] private float _reload_Time = 2.0f;
    [SerializeField] bool _isAutoFire;
    [SerializeField] private float _recoil_Force;
    [SerializeField] private Rigidbody _bodyRigidbody;
    [SerializeField] private KeyCode _anotherWeaponFire;
    private float _reloadTimer;
    private bool _isCannonDamaged;
    
    void Update()
    {
        if (_reloadTimer<=_reload_Time)
        {
            _reloadTimer+=Time.deltaTime;
        }

        if (_isAutoFire)
        {
            if (Input.GetKey(_anotherWeaponFire))
            {
                Shoot();
            }
        }

        else
        {
            if (Input.GetKeyDown(_anotherWeaponFire))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (_reloadTimer>_reload_Time)
        {
            var spread = _isCannonDamaged ? new Vector3(0, Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f)) : Vector3.zero;
            _linkForGenerator.Fire_Linkage(1, spread);
            _reloadTimer = 0;
            _bodyRigidbody.AddForceAtPosition(-transform.forward * _recoil_Force, transform.position, ForceMode.Impulse);
        }      
    }
}