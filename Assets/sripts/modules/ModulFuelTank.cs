using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulFuelTank : ModulBase
{
    public List<ModulBase> damagedModules;

    private bool _isBurn;


    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                BurnWithChance(100);
                break;

            case int n when (n <= 10):
                print("Modul damaged");

                BurnWithChance(30);
                break;


        }
    }

    private void BurnWithChance(int chance)
    {
        var isBurn = Random.Range(0, 100) <= chance;

        if (isBurn)
        {
            StartCoroutine("BurnDamage");
            _isBurn = true;
        }
    }

    private IEnumerable BurnDamage()
    {
        while (_isBurn)
        {
            foreach(ModulBase modul in damagedModules)
            {
                modul.GetDamage(3);
                print("burning");

            }
            yield return new WaitForSeconds(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }
}
