using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireArm : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _lifeTime;
    private void OnCollisionEnter(Collision collision)
    {
        var damageble = collision.collider.GetComponent<IDamageble>();
        if (damageble != null)
        {
            damageble.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        Destroy(gameObject, _lifeTime);
    }
}
