using ChobiAssets.PTM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePhosphorusAirStrike : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _explForse;
    [SerializeField] private GameObject _fireFX;
    [SerializeField] private GameObject _tailFX; 

    private void Start()
    {
        var damagedObjects = transform.ExplosionWave<WhitePhoephorousFragment>(_radius, _explForse);

        foreach (var nearbyObject in damagedObjects)
        {
            Instantiate(_tailFX, nearbyObject.transform);
            nearbyObject.SetData(_fireFX);           
        }
    } 
}
