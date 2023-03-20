using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveReactiveArmour : MonoBehaviour
{
    [SerializeField] private int _heatDamagePenetrationReduction;
    [SerializeField] private int _heatDamageModulReduction;
    [SerializeField] private int _kineticDamagePenetrationReduction;
    [SerializeField] private int _kineticDamageModulReduction;


    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.gameObject.GetComponent<Bullet>();
        if (bullet == null)
        {
            return;
        }
        print("Helooooooooooooooo");
        if (other.gameObject.layer == 12)
        {

            bullet.DamageReduction(_kineticDamageModulReduction, _kineticDamagePenetrationReduction);
        }

        if (other.gameObject.layer == 15)
        {
            bullet.DamageReduction(_heatDamageModulReduction, _heatDamagePenetrationReduction);
        }
        Destroy(gameObject);
    }

}
