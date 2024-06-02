using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FXPalyOnAwake : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<VisualEffect>().Play();
    }
}
