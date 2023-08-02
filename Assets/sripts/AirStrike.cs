using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{
    [SerializeField] private Vector3 koordinates;
    [SerializeField] private GameObject missile;
    [SerializeField] private float reloadTime;
    private bool isReloading = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isReloading)
            {
                return;
            }
            missile.SetActive(true);
            //missile.transform.position = koordinates;
            GameObject _missile = Instantiate(missile, koordinates, Quaternion.identity);
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        
        yield return new WaitForSeconds(reloadTime);
        
        isReloading = false;
    }
}
