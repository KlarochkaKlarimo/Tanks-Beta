using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject[] fragments;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private Collider _collider;
    private int _penetrationDamage;
    private Vector3 _destination;
    [SerializeField] private float speed = 0.1f;
    private Vector3 _step;
    [SerializeField] private int modulDamage;
    public void SetVariables(Vector3 destination, int damage, float lifeTime, bool isCannonDamaged, GameObject cannon)
    {
        transform.rotation = cannon.transform.rotation;
           var cannonColliders = cannon.GetComponents<Collider>();
        if (cannonColliders != null)
        {
            foreach (Collider collider in cannon.GetComponents<Collider>())
            {
                Physics.IgnoreCollision(collider, _collider);
            }
        }
        var deviation = new Vector3();
        if (isCannonDamaged)
        {
            deviation = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
        }
           //_destination = new Vector3(destination.x,0, destination.z);
        
        _penetrationDamage = damage;
        m_Rigidbody.AddForce(speed*transform.forward + deviation,ForceMode.Impulse);
        
        //  m_Rigidbody.velocity = _destination;
        //m_Rigidbody.AddForce(Vector3.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {

        var arrmor= collision.gameObject.GetComponent<armor_panel>();
        if (arrmor == null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }
        var angle = ((Vector3.Angle(transform.forward, collision.contacts[0].normal)) - 90);
        var anngleKoefecent = (angle * 0.9f)/100;
        Debug.Log(arrmor.GetThicknes() + " arrmor.GetThicknes() " + anngleKoefecent + " anngleKoefecent " + " anngleKoefecent * _penetrationDamage " + anngleKoefecent * _penetrationDamage);

        Rickoshet(arrmor, anngleKoefecent, collision);
        SpawnFragments();
    }

    public virtual void SpawnFragments()
    {
        foreach(GameObject fragment in fragments)
        {
            fragment.SetActive(true);
        }
    }

    public virtual void Rickoshet(armor_panel arrmor, float anngleKoefecent, Collision collision)
    {

        if (arrmor.GetThicknes() <= anngleKoefecent * _penetrationDamage)
        {
            Physics.IgnoreCollision(collision.collider, _collider);
            m_Rigidbody.AddForce(100 * transform.forward, ForceMode.Impulse);
            print("Probitie" + collision.gameObject.name);
        }
        else
        {
            Physics.IgnoreCollision(collision.collider, _collider);
            if (anngleKoefecent < 0.6f)
            {
                var rikoshetChanse = (1 - anngleKoefecent) * 100f;
                var rikoshetRandom = Random.Range(0, 100);
                Debug.Log(rikoshetChanse+ " rikoshetChanse " + rikoshetRandom + " rikoshetRandom " + " anngleKoefecent "+ anngleKoefecent);
                if (rikoshetChanse < rikoshetRandom)
                {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

    }
    public int GetModulDamage()
    {
        return modulDamage;
    }
}
