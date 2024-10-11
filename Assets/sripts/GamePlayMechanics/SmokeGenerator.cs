using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _smokeFX;

    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            _smokeFX.SetActive(true);
        }
        else
        {
            _smokeFX.SetActive(false);
        }
    }
}
