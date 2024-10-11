using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadesScrip : MonoBehaviour
{
    public float launchForce = 10f;
    [SerializeField] private float _explosionTime;
    
    [SerializeField] private GameObject _explosionEffect;

    public void LaunchSmokeGrenade()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            Vector3 launchDirection = transform.forward;

            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
            Invoke("Explosion", _explosionTime);
        }
    }
    private void Explosion()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
