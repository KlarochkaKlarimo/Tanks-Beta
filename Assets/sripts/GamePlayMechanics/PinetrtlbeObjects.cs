using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PinetrtlbeObjects : MonoBehaviour
{
    [SerializeField] private int _thickness;
    public int GetThicknes()
    {
        return _thickness;
    }

}
