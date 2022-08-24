using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ModulBase : MonoBehaviour
{
    public int hp; 


    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.gameObject.GetComponent<bullet>();
        if (bullet == null)
        {
            return;
        }

        if (hp == 0)
        {
            return;
        }

        hp = Mathf.Clamp(hp - bullet.GetModulDamage(), 0, hp); 
    }

    public virtual void GetDamage(int damage)
    {
        print(hp);
    }

}
