using System;
using UnityEngine;
using UnityEngine.UI;
using ChobiAssets.PTM;

public abstract class ModulBase : IPinetrtlbe
{
    [SerializeField] private int hp;
    [SerializeField] private int _damagedHp;
    [SerializeField] private Image _modulImage;
    [SerializeField] private bool isTest;
    public bool isModelDamaged;

    [SerializeField] protected Cannon_Fire_CS cannonFire;
    [SerializeField] protected Aiming_Control_CS aimingControl;
    [SerializeField] protected Drive_Control_CS driveControl;
    protected Bullet_Control_CS bullet;

    public static Action isTankDestroyed;

    public void setBullet(Bullet_Control_CS _bullet)
    {
        bullet = _bullet;
    }
  
    private void ChangeModulImageColor(Color _color)
    {
        if(_modulImage != null)
        {
            _modulImage.color = _color;
        }
    }

    public virtual void ModulDamaged()
    {
        ChangeModulImageColor(Color.yellow);
    }

    public virtual void ModulDestroyed()
    {
        ChangeModulImageColor(Color.red);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public virtual void GetDamage(int damage)
    {
        if (isTest)
        {
            return;
        }
        hp = Mathf.Clamp(hp - damage, 0, hp);

        switch (hp)
        {
            case 0:            
                ModulDestroyed();          
                break;

            case int n when (n <= _damagedHp):
                if (isModelDamaged)
                {
                    return;
                }
                isModelDamaged = true;
                ModulDamaged();
                break;
        }
    }

    public virtual void TankDestroed()
    {
        aimingControl.enabled = false;
        driveControl.enabled = false;
        Destroy(aimingControl);
        Destroy(driveControl);
        isTankDestroyed?.Invoke();
    }

}
