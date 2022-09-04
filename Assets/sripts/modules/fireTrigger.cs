using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var isModul = other.gameObject.GetComponent<ModulBase>();

        if (isModul != null)
        {
            transform.parent.GetComponent<ModulFuelTank>().damagedModules.Add(isModul);
        }
    }
}
