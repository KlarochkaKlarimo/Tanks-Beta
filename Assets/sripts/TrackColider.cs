using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrackColider : MonoBehaviour
{
    [SerializeField] private TrackSystem _trackSystem;
    [SerializeField] private Collider [] _collidersToIgnore;

    private void Awake()
    {
        var coll = GetComponent<Collider>();
        foreach (var col in _collidersToIgnore)
        {
            Physics.IgnoreCollision(coll, col);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {       
        _trackSystem.ChekTrack(collision);
    }
}
