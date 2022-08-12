using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armor_panel : MonoBehaviour
{
    [SerializeField] private int thickness;
    
    private void OnCollisionEnter(Collision collision)
    {
        var bullet = collision.gameObject.GetComponent<bullet>();
        if (bullet != null)
        {
            var penetrationDamage = bullet.GetPenetrationDamage();
            var panelAngle = transform.localEulerAngles;
            var bulletAngle = collision.transform.localEulerAngles;
            print(panelAngle);
            print(bulletAngle);

            if (penetrationDamage > thickness)
            {
                Destroy(gameObject);
            }
        }
    }

}
