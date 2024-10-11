using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{
    [SerializeField] private Vector3 _uslovnieCoordinates;
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private float _distance;
    [SerializeField] private GameObject missile;
    [SerializeField] private float reloadTime;
    private Vector3 _coordinates;
    private bool isReloading = false;
    private Rigidbody missileRB;
    [SerializeField] private float _missileSpeed;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (missileRB != null)
        {

            missileRB.transform.position=Vector3.MoveTowards(missileRB.transform.position, _coordinates, _missileSpeed * Time.fixedDeltaTime);
            missileRB.transform.LookAt(_coordinates);
            //Vector3 movePosition = Vector3.Slerp(missileRB.position, koordinates, 1f * Time.fixedDeltaTime);
            //missileRB.MovePosition(movePosition);
            //Debug.Log(missileRB.position);
        }
    }

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isReloading)
            {
                return;
            }
            
            _coordinates = _startPoint + _uslovnieCoordinates * _distance;
            missileRB =  Instantiate(missile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
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
