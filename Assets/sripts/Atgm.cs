using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atgm : Bullet
{
    private Vector3 _destination;
    private Transform _shootPoint;
    [SerializeField] private float _noControlAtgmDistance;
    [SerializeField] private float _rotationSpeed;
    private bool isControlled;
    public override void SetVariables(int damage, float lifeTime, bool isCannonDamaged, GameObject cannon)
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
        
        _penetrationDamage = damage;
     //   m_Rigidbody.AddForce(speed*transform.forward, ForceMode.Impulse);
    }
    public override void Rickoshet(IPinetrtlbe arrmor, float anngleKoefecent, Collider collision)
    {
        
    }

    private void FixedUpdate()
    {
        if(!isControlled)
        {
            return;
        }
        var distance = Vector3.Distance(transform.position, _shootPoint.position);
        if (distance > _noControlAtgmDistance)
        {
            isControlled = false;
            m_Rigidbody.AddForce(speed * transform.forward, ForceMode.Impulse);
            return;
        }
        var ray = new Ray(_shootPoint.position, _shootPoint.forward);
        _destination = ray.origin + ray.direction* 1000f;

        m_Rigidbody.velocity = (_destination - transform.position).normalized * speed;
     //   transform.position = Vector3.MoveTowards(transform.position, _destination, Time.deltaTime * _rotationSpeed);

    }

    public void SetShootPoint(Transform shootPoint)
    {
        isControlled = true;
        _shootPoint = shootPoint;

    }
}
