using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFragments : Bullet
{
    public override void SpawnFragments()
    {
        
    }

    public override void Rickoshet(armor_panel arrmor, float anngleKoefecent, Collider collision)
    {

    }

    public override void SetVariables(int damage, float lifeTime, bool isCannonDamaged, GameObject cannon)
    {
        _penetrationDamage = damage;
        m_Rigidbody.AddForce(speed*transform.forward, ForceMode.Impulse);
    }

}
