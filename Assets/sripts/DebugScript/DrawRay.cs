using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRay : MonoBehaviour
{   
    void Update()
    {
        var ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan, 14f);
    }
}
