using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColider : MonoBehaviour
{
    [SerializeField] private ModulAmmorack _ammorack;

    private void OnCollisionEnter(Collision collision)
    {
        _ammorack._tankDestroyed = false;
    }
}
