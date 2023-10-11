using ChobiAssets.PTM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveReactiveArmour : MonoBehaviour
{
    [SerializeField] private int _heatDamagePenetrationReduction;
    [SerializeField] private int _heatDamageModulReduction;
    [SerializeField] private int _kineticDamagePenetrationReduction;
    [SerializeField] private int _kineticDamageModulReduction;


    
    private void OnCollisionEnter(Collision collision)
    {
        var bullet = collision.gameObject.GetComponent<Bullet_Control_CS>();
        if (bullet == null)
        {
            return;
        }
        print("Helooooooooooooooo");
        if (collision.gameObject.layer == 12)
        {
            bullet.DamageReduction(_kineticDamageModulReduction, _kineticDamagePenetrationReduction);
        }

        if (collision.gameObject.layer == 15)
        {
            bullet.DamageReduction(_heatDamageModulReduction, _heatDamagePenetrationReduction);
        }
        Destroy(gameObject);
    }

}
