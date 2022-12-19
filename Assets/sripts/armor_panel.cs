using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armor_panel : MonoBehaviour
{
    [SerializeField] private int _thickness;
    public int GetThicknes()
    { 
        return _thickness;
    }

    //private void OnSetDamage(f)
    //{
    //    var bullet = collision.gameObject.GetComponent<bullet>();
    //    if (bullet != null)
    //    {
    //        var penetrationDamage = bullet.GetPenetrationDamage();
    //        var angleBetween = Vector3.Angle( collision.transform.localEulerAngles,transform.localEulerAngles.normalized);// Vector3.Angle(transform.localEulerAngles, collision.transform.localEulerAngles);
    //        var anngleKoefecent= 
    //        print(90 - (Vector3.Angle(transform.position.forward, contact.normal)));
    //        //if (penetrationDamage > thickness)
    //        //{
    //        //    Destroy(gameObject);
    //        //}
    //    }
    //}

}
