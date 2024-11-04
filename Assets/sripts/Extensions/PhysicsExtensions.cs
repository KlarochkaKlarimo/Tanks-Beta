using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsExtensions
{
    public static List<T> ExplosionWave<T>(this Transform transform, float _radius, float _explForce) where T : class 
    {
        var damagedObjects = new List<T>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_explForce, transform.position, _radius);
                var item = nearbyObject.GetComponent<T>();
                if(item != null)
                {
                    damagedObjects.Add(item);
                }               
            }
        }
        return damagedObjects;
    }
}
