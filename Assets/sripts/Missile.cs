using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    [SerializeField] private float _radius;
    [SerializeField] private float _explForse;
    [SerializeField] private DeadCollision _deadCollider;

    
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
        Instantiate(_deadCollider.gameObject, transform.position, Quaternion.identity);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_explForse, transform.position, _radius);
            }
        }
    }
}
