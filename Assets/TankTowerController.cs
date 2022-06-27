using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTowerController : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private float reloadingTime;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject flesh;

    private bool isReloading;
    private Vector3 _destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(ShootPoint.position, ShootPoint.forward);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 100))
        //{
        //    _destination = hit.point;

        //}
        //else 
        //{
        //    
        //}
        _destination = ray.origin + ray.direction * 1000f;
        Debug.DrawLine(ShootPoint.position, _destination, Color.red, 10f);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }
        //else
        //{
        //    _destination = ray.origin + ray.direction * 1000f;
        //}
        //_destination = (_destination - transform.position).normalized * 50f;



    }

    private void Shoot()
    {
        if (isReloading)
        {
            return;
        }
        //var shoot = Instantiate(bullet, ShootPoint.position, transform.rotation);
        //shoot.GetComponent<Rigidbody>().AddForce (bullet.transform.forward*5000f);
        
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        isReloading = true;
        Invoke("Reloading", reloadingTime);
        var _bullet = Instantiate(bullet, ShootPoint.position, transform.rotation);
        _bullet.GetComponent<bullet> ().SetVariables(_destination, 100, 40);
         Instantiate(flesh, ShootPoint.position, Quaternion.identity);
    }
    private void Reloading()
    {
        isReloading = false;

    }
}
