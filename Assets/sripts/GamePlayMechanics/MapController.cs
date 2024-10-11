using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject _fullMap;
    [SerializeField] private GameObject _miniMap;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _fullMap.SetActive(!_fullMap.activeInHierarchy);
            _miniMap.SetActive(!_fullMap.activeInHierarchy);
        }
    }
}
