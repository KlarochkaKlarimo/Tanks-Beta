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
    [SerializeField] private Transform _fragmentsParent;
    private Collider armorCollider;
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
        _penetrationDamage = damage;
        m_Rigidbody.AddForce(speed*transform.forward + deviation,ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var arrmor = collision.gameObject.GetComponent<armor_panel>();
        if (arrmor == null)
        {
            DestroyBullet();
            return;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("trigger");
        var arrmor = other.gameObject.GetComponent<armor_panel>();
        if (arrmor == null)
        {
            DestroyBullet();
            return;
        }
        var angle = 50;//((Vector3.Angle(transform.forward, other.contactOffset.)) - 90);
        var anngleKoefecent = (angle * 0.9f)/100;
        Debug.Log(arrmor.GetThicknes() + " arrmor.GetThicknes() " + anngleKoefecent + " anngleKoefecent " + " anngleKoefecent * _penetrationDamage " + anngleKoefecent * _penetrationDamage);
        Rickoshet(arrmor, anngleKoefecent, other);
        SpawnFragments();
    }
    public virtual void SpawnFragments()
    {
        //_fragmentsParent.DetachChildren();
        //foreach(GameObject fragment in fragments)
        //{
            
        //    fragment.SetActive(true);
        //}
    }

    public virtual void Rickoshet(armor_panel arrmor, float anngleKoefecent, Collider collision)
    {

        if (false/*arrmor.GetThicknes() <= anngleKoefecent * _penetrationDamage*/)
        {     
            print("Probitie " + collision.gameObject.name);
        }
        else
        {
            print("Not Probitie " + collision.gameObject.name);
            if (anngleKoefecent < 0.6f)
            {
                var rikoshetChanse = (1 - anngleKoefecent) * 100f;
                var rikoshetRandom = Random.Range(0, 100);
              // Debug.Log(rikoshetChanse + " rikoshetChanse " + rikoshetRandom + " rikoshetRandom " + " anngleKoefecent " + anngleKoefecent);
                if (false/*rikoshetChanse < rikoshetRandom*/)
                {
                    print("Not Rikoshet " + collision.gameObject.name);
                    DestroyBullet();
                }
                else
                {
                    print("Rikoshet " + collision.gameObject.name+" angle "+ transform.eulerAngles);
                    var trans = transform;
                    m_Rigidbody.velocity = Vector3.Reflect(transform.position, Vector3.left);
                    if(transform.eulerAngles.y>180)
                    {
                        m_Rigidbody.velocity = Vector3.Reflect(transform.position, Vector3.right);
                    }
                }
            }
            else
            {
                print("Not Rikoshet by angle" + collision.gameObject.name);
                DestroyBullet();
            }
        }
    }
    private void DestroyBullet()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public int GetModulDamage()
    {
        return modulDamage;
    }
}
