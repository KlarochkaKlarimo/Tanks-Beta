using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class MineScript : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _explForse;
    [SerializeField] private DeadCollision _deadCollider;
    [SerializeField] private Collider _vzrivatel;
    [SerializeField] private bool _hasCollided = false;
    [SerializeField] private float _explosionDelay;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float _otorvatKotokDistancia;
    [SerializeField] private GameObject _mine;
    

    private void OnCollisionEnter(Collision collision)
    {
        var wheel = collision.collider.gameObject.GetComponent<Damage_Control_03_Physics_Track_Piece_CS>();
        if (!_hasCollided && wheel!=null)
        {
            _hasCollided = true;
            StartCoroutine(ExplodeAfterDelay());            
        }

    }
    
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(_explosionDelay);
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                var wheel = nearbyObject.gameObject.GetComponent<Drive_Wheel_CS>();
                if (wheel!=null)
                {
                    if(Vector3.Distance(transform.position, wheel.transform.position)<_otorvatKotokDistancia)
                    {
                        wheel.transform.SetParent(null);
                        var hitJoint = wheel.gameObject.GetComponent<HingeJoint>();
                        if(hitJoint != null)
                        {
                            Destroy(hitJoint);
                            wheel.gameObject.GetComponent<Drive_Wheel_CS>().enabled = false;
                            wheel.gameObject.GetComponent<Fix_Shaking_Rotation_CS>().enabled = false;
                            wheel.gameObject.GetComponent<Stabilizer_CS>().enabled = false;
                        }
                    }
                }
                rb.AddExplosionForce(_explForse, transform.position, _radius);
            }
        }     
        explosion.transform.SetParent(null);
        explosion.transform.localScale = Vector3.one;
        explosion.SetActive(true);
        Destroy(_mine);
    }
}
