using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSplinterArmor : MonoBehaviour
{
    [SerializeField] private float _chance;

    public bool IsBulletStopped()
    {
        return Random.Range(0, 100) < _chance;
    }
}
