using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ModulBase : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int _damagedHp;
    [SerializeField] private Image _modulImage;
    public bool isModelDamaged;
   

    public TankTowerController tankTowerController;
    public TankWheelControl tankWheelControl;
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

    private void ChangeModulImageColor(Color _color)
    {
        if(_modulImage != null)
        {
            _modulImage.color = _color;
            

        }
    }

    public virtual void modulDamaged()
    {
        ChangeModulImageColor(Color.yellow);
    }

    public virtual void modulDestroyed()
    {
        ChangeModulImageColor(Color.red);
    }

    public virtual void GetDamage(int damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, hp);
        print(hp);

        switch (hp)
        {
            case 0:
                print("Modul destroed" + gameObject.name);
                modulDestroyed();          
                break;

            case int n when (n <= _damagedHp):
                if (isModelDamaged)
                {
                    return;
                }
                isModelDamaged = true;
                print("Modul damaged" + gameObject.name);
                modulDamaged();
                break;


        }
    }

    public virtual void TankDestroed()
    {
        tankTowerController.enabled = false;
        tankWheelControl.enabled = false;

    }

}
