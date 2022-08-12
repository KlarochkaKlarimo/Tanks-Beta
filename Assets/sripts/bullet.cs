using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private Rigidbody m_Rigidbody;
    private int _penetrationDamage;
    private Vector3 _destination;
    [SerializeField] private float speed = 0.1f;
    private Vector3 _step;
    public void SetVariables(Vector3 destination, int damage, float lifeTime)
    {
           //_destination = new Vector3(destination.x,0, destination.z);
        var vector=  transform.position - _destination;
        _penetrationDamage = damage;
        m_Rigidbody.AddForce(100*transform.forward,ForceMode.Impulse);
        
        //  m_Rigidbody.velocity = _destination;
        //m_Rigidbody.AddForce(Vector3.forward * speed);
    }

    private void FixedUpdate()
    {
        if (_destination != null)
        {
    //        _step = Vector3.MoveTowards(_step, _destination, speed * Time.fixedDeltaTime);
          //  m_Rigidbody.AddForce(_destination);
        }
    }


   

    private void OnCollisionStay(Collision collision)
    {
        
        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);

        
    }
    public int GetPenetrationDamage()
    {
        return _penetrationDamage;
    }
}
