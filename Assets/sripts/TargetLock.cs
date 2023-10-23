using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLock : MonoBehaviour
{
    public float maxDistance;

    [SerializeField] private GameObject targetCaptured;
    [SerializeField] private GameObject targetNotCaptured;
    [SerializeField] private Transform _lockedTarget;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private HomingMissile missileScript; // Ссылка на скрипт ракеты

    private void Start()
    {      
       // Найдите скрипт ракеты на текущем объекте
        missileScript = GetComponent<HomingMissile>();
    }

    private void Update()
    {
        if (isLocked)
        {
            // Здесь вы можете добавить логику для удержания цели и отслеживания ее движения.
        }
        else
        {
            // Если цель не захвачена, проверьте, смотрите ли вы на цель и нажимаете ли кнопку для захвата (например, X).
            if (Input.GetKeyDown(KeyCode.X))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
                {
                    if (hit.collider.CompareTag("Tank"))
                    {
                        _lockedTarget = hit.collider.transform;
                        isLocked = true;       
                        if (isLocked)
                        {
                            targetNotCaptured.SetActive(false);
                            targetCaptured.SetActive(true);
                        }



                        //HomingMissile _homingMissile = FindObjectOfType<HomingMissile>();
                        //if (_homingMissile != null)
                        //{
                        //    _homingMissile.GetTarget(_lockedTarget);
                        //}

                        
                    }
                }
            }
        }
    }

    public Transform GetLockedTarget()
    {
        return _lockedTarget;
    }
}
