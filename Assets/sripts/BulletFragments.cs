using UnityEngine;

public class BulletFragments : Bullet
{
    public override void SpawnFragments()
    {
        
    }

    public override void Rickoshet(IPinetrtlbe arrmor, float anngleKoefecent, Collider collision)
    {

    }

    public override void SetVariables(int damage, float lifeTime, bool isCannonDamaged, GameObject cannon)
    {
        _penetrationDamage = damage;
        m_Rigidbody.AddForce(speed*transform.forward, ForceMode.Impulse);
    }

    public override void DestroyBullet(ExpEffType type)
    {
        Destroy(gameObject);
    }
}
