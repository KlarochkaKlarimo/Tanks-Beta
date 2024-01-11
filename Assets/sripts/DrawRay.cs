using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRay : MonoBehaviour
{
   
    void Update()
    {
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawRay(transform.position, transform.forward*100);
        var ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan, 14f);
    }
}
