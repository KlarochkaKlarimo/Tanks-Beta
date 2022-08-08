using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private Rigidbody m_Rigidbody;
    private float _damage;
    private Vector3 _destination;
    [SerializeField] private float speed = 0.1f;
    private Vector3 _step;
    public void SetVariables(Vector3 destination, float damage, float lifeTime)
    {
           _destination = new Vector3(destination.x,0, destination.z) ;
        _damage = damage;

      //  m_Rigidbody.velocity = _destination;
        //m_Rigidbody.AddForce(Vector3.forward * speed);
    }

    private void FixedUpdate()
    {
        if (_destination != null)
        {
    //        _step = Vector3.MoveTowards(_step, _destination, speed * Time.fixedDeltaTime);
            m_Rigidbody.AddForce(_destination);
        }
    }


   

    private void OnCollisionStay(Collision collision)
    {
        print("Boom");
        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);

        
    }


}
