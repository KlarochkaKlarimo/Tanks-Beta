using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TankTowerController : MonoBehaviour
{
    public bool isCannonDamaged;
    public float reloadingTime;

    [SerializeField] private GameObject _currentAmmo;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Camera camera;
    [SerializeField] private ParticleSystem flesh;
    [SerializeField] private int _BulletPenetration;
    [SerializeField] private bool _isTest;
    [SerializeField] private int _misFireChance;
    [SerializeField] private GameObject _imageGreen;
    [SerializeField] private Image _imageYellow;
    [SerializeField] private Text _reloadingText;
    [SerializeField] private GameObject _cannon;
    [SerializeField] private Text _currentAmmoText;
    [SerializeField] private AmmoList[] _ammoLists;

    private bool _isBreachGunDamaged;
    private bool _isMisFire;

    private bool isReloading;
    

    private void Awake()
    {
        _reloadingText.text = reloadingTime.ToString();
        //ChangeAmmo(0);
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
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }

        AmmoChanger();
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
        var _bullet = Instantiate(_currentAmmo, _shootPoint.position, transform.rotation);
        _bullet.transform.Rotate (_cannon.transform.rotation.x, transform.rotation.y, transform.rotation.z);
        _bullet.GetComponent<Bullet> ().SetVariables( _BulletPenetration, 40, isCannonDamaged, _cannon);
        var atgm = _bullet.GetComponent<Atgm>();
        if (atgm!=null)
        {
            atgm.SetShootPoint(_shootPoint);
        }
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

    public void AmmoChanger()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ChangeAmmo(0);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            ChangeAmmo(1);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            ChangeAmmo(2);
        }
    }

    private void ChangeAmmo(int index)
    {
        _currentAmmo = _ammoLists[index].prefab;
        _currentAmmoText.text = _ammoLists[index].ammoName;
        StopAllCoroutines();
        isReloading = true;
        StartCoroutine(ReloadingTimer());
    }
}
