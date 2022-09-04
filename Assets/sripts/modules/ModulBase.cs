using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ModulBase : MonoBehaviour
{
    public int hp;
    public bool isModelDamaged;

    public TankTowerController tankTowerController;
    public RearWheelDrive rearWheelDrive;
    public MouseLook mouseLook;

    private void OnCollisionEnter(Collision collision)
    {
        var bullet = collision.gameObject.GetComponentInParent<bullet>();
        if (bullet == null)
        {
            return;
        }

        

        if (hp == 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            
        }

        

        GetDamage(bullet.GetModulDamage());
    }
    

    public virtual void GetDamage(int damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, hp);
        print(hp);
    }

    public virtual void TankDestroed()
    {
        tankTowerController.enabled = false;
        rearWheelDrive.enabled = false;

    }

}
