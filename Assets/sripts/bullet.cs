using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    private float _damage;
    private Vector3 _destination;
    [SerializeField] private float speed = 0.1f;

    public void SetVariables(Vector3 destination, float damage, float lifeTime)
    {
        _destination = destination;
        _damage = damage;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (_destination != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * speed);
          //  transform.position += _destination * (Time.deltaTime * speed);
        }
    }


   

    private void OnCollisionStay(Collision collision)
    {
        print("Boom");
        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);

        
    }


}
