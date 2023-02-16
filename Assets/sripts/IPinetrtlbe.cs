using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPinetrtlbe : MonoBehaviour
{
    [SerializeField] private int _thickness;
    public int GetThicknes()
    {
        return _thickness;
    }

}
