using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : Bullet
{
    public override void Rickoshet(IPinetrtlbe arrmor, float anngleKoefecent, Collider collision)
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        var ERA = other.gameObject.GetComponent<ExplosiveReactiveArmour>();
        if (ERA != null)
        {
            return; 
        }
        DestroyBullet(ExpEffType.metallic);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        DestroyBullet(ExpEffType.ground);
    }
}
