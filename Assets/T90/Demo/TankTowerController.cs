using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTowerController : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float reloadingTime;
    private bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }


    }

    private void Shoot()
    {
        if (isReloading)
        {
            return;
        }
            var shoot = Instantiate(bullet, ShootPoint.position, ShootPoint.rotation);
        shoot.GetComponent<Rigidbody>().velocity = bullet.transform.forward*100f;
            isReloading = true;
        Invoke("Reloading", reloadingTime);
    }

    private void Reloading()
    {
        isReloading = false;

    }
}
