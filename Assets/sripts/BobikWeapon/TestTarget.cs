using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageble
{
    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
    }
}
