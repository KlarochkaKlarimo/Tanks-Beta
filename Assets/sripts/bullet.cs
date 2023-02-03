using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] protected BulletFragments[] fragments;
    [SerializeField] protected GameObject explosion;
    [SerializeField] protected Rigidbody m_Rigidbody;
    [SerializeField] protected Collider _collider;
    protected int _penetrationDamage;
    [SerializeField] protected float speed = 0.1f;
    [SerializeField] protected int modulDamage;
    [SerializeField] protected Transform _fragmentsParent;
    private Collider armorCollider;
    public virtual void SetVariables( int damage, float lifeTime, bool isCannonDamaged, GameObject cannon)
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
        //    DestroyBullet();
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
        _fragmentsParent.DetachChildren();
        foreach(BulletFragments fragment in fragments)
        {
            
            fragment.gameObject.SetActive(true);
            fragment.SetVariables(2, 5, false, gameObject);
        }
    }

    public virtual void Rickoshet(armor_panel arrmor, float anngleKoefecent, Collider collision)
    {

        if (true/*arrmor.GetThicknes() <= anngleKoefecent * _penetrationDamage*/)
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
        explosion.SetActive(true);
        explosion.transform.parent = null;
        Destroy(gameObject);
    }
    public int GetModulDamage()
    {
        return modulDamage;
    }
}
