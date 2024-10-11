using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private Transform target;
    [SerializeField] private TargetLock targetLock;
    [SerializeField] private GameObject _flightFire;
    [SerializeField] private GameObject _flightFire1;
    private bool _isStarted = false;

    private void Awake()
    {
        targetLock = FindObjectOfType<TargetLock>();
    }

    private void Update()
    {
        
        if (targetLock != null)
        {
            target = targetLock.GetLockedTarget();
        }

        if (target == null)       
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isStarted = true;                    
        }
        if (_isStarted)
        {
            transform.SetParent(null);

            _flightFire.SetActive(true);
            _flightFire1.SetActive(true);

            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            // Направление движения ракеты к цели
            Vector3 targetDirection = target.position - transform.position;

            // Поворот ракеты к цели
            transform.rotation = Quaternion.LookRotation(targetDirection);

            // Движение ракеты в направлении цели
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    
}
