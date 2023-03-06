using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : Bullet
{
    public override void Rickoshet(IPinetrtlbe arrmor, float anngleKoefecent, Collider collision)
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        DestroyBullet();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        DestroyBullet();
    }
}
