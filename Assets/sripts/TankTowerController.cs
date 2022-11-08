using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankTowerController : MonoBehaviour
{
    public bool isCannonDamaged;
    public float reloadingTime;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private Camera camera;
    [SerializeField] private ParticleSystem flesh;
    [SerializeField] private int _BulletPenetration;
    [SerializeField] private bool _isTest;
    [SerializeField] private int _misFireChance;
    [SerializeField] private GameObject _imageGreen;
    [SerializeField] private Image _imageYellow;
    [SerializeField] private Text _reloadingText;
    [SerializeField] private GameObject _cannon;
    private bool _isBreachGunDamaged;
    private bool _isMisFire;

    private bool isReloading;
    private Vector3 _destination;

    private void Awake()
    {
        _reloadingText.text = reloadingTime.ToString();
    }

    public void SetIsTest(bool isTest)
    {
        _isTest = isTest;
    }

    public void DamagedBreachGun()
    {
        _isBreachGunDamaged = true;

    }

    void Update()
    {
        if (_isTest)
        {
            return;
        }


        var ray = new Ray(ShootPoint.position, ShootPoint.forward);
        _destination = ray.origin + ray.direction * 1000f;
        Debug.DrawLine(ShootPoint.position, _destination, Color.cyan, 10f);
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
        if (_isBreachGunDamaged)
        {
            _isMisFire = Random.Range(0, 100)>= _misFireChance;
            if (_isMisFire)
            {
                return;
            }
        }
        
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        isReloading = true;
        StartCoroutine(ReloadingTimer());
        //var bulletRotation = new Quaternion.(_cannon.transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        var _bullet = Instantiate(bullet, ShootPoint.position, transform.rotation);
        _bullet.transform.Rotate (_cannon.transform.rotation.x, transform.rotation.y, transform.rotation.z);
        _bullet.GetComponent<bullet> ().SetVariables(_destination, _BulletPenetration, 40, isCannonDamaged, _cannon);
        flesh.Play();
    }
    
    IEnumerator ReloadingTimer()
    {
        _reloadingText.color = Color.yellow;
        _imageYellow.gameObject.SetActive(true);
        _imageGreen.SetActive(false);
        _imageYellow.fillAmount = 0;
        var _reloadingTimer = reloadingTime;
        float _imageFillStep = (float)0.1/reloadingTime;
        do
        {
            yield return new WaitForSeconds(0.1f);
            _reloadingTimer -= 0.1f;
            _reloadingText.text = _reloadingTimer.ToString("0.0");
            _imageYellow.fillAmount += _imageFillStep;
        }
        while (_reloadingTimer>0);
        _imageGreen.SetActive(true);
        isReloading = false;
        _imageYellow.gameObject.SetActive(false);
        _reloadingText.text = reloadingTime.ToString();
        _reloadingText.color = Color.black;
    }
}
