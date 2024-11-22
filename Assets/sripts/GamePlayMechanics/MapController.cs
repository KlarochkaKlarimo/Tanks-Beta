using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject _fullMap;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _fullMap.SetActive(!_fullMap.activeInHierarchy);
        }
    }
}
