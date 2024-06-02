using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCollision : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        var ammorack = other.gameObject.GetComponent<ModulAmmorack>();
        if (ammorack!=null)
        {
            ammorack.ModulDestroyed();
        }
    }

    private void Awake()
    {
        Destroy(gameObject, 1f);
    }
}
