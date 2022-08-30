using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ModulBase : MonoBehaviour
{
    public int hp;
    public bool isModelDamaged;

    public TankTowerController tankTowerController;
    public RearWheelDrive rearWheelDrive;

    private void OnCollisionEnter(Collision collision)
    {
        var bullet = collision.gameObject.GetComponentInParent<bullet>();
        if (bullet == null)
        {
            return;
        }

        hp = Mathf.Clamp(hp - bullet.GetModulDamage(), 0, hp);

        if (hp == 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            
        }

        

        GetDamage();
    }
    

    public virtual void GetDamage()
    {
        print(hp);
    }

    public virtual void TankDestroed()
    {
        tankTowerController.enabled = false;
        rearWheelDrive.enabled = false;

    }

}
