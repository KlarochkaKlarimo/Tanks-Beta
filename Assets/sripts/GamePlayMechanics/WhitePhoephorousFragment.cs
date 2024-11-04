using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePhoephorousFragment : MonoBehaviour
{
    private GameObject _fireFX;

    public void SetData(GameObject fireFX)
    {
        _fireFX = fireFX;
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Instantiate(_fireFX, transform);
    }
}
