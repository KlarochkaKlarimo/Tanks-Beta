using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;
using System;

public class TrackSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] _tracks;  
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _XmaxTankSpeedForTrackReap;
    [SerializeField] private float _YmaxTankSpeedForTrackReap;
    private bool _isReaped;
    private Collision _collision;
    [SerializeField] private Static_Track_Parent_CS _trackParent;
    [SerializeField] Vector3 _velocity;
    [SerializeField] private Collider[] _collidersToIgnore;

    private void Awake()
    {
        var coll = GetComponent<Collider>();
        foreach (var col in _collidersToIgnore)
        {
            Physics.IgnoreCollision(coll, col);
        }
    }

    void Update()
    {
        _velocity = _rb.velocity;

        if (_isReaped)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _isReaped = true;
            DestroyTrack();
            _trackParent.enabled = false;
        }
    }

    private void DestroyTrack()
    {
        var i = 0;
        foreach (var track in _tracks)
        {
            track.AddComponent<Rigidbody>();
            var HJ = track.AddComponent<HingeJoint>();
            HJ.breakForce = 10000;
            if (i>0)
            {
                HJ.connectedBody = _tracks[i-1].GetComponent<Rigidbody>();
            }
            track.transform.parent = null;
            foreach (var colider in track.GetComponents<CapsuleCollider>())
            {
                colider.enabled = true;
            }
            track.GetComponent<Static_Track_Piece_CS>().enabled = false;
            i++;
        }
    }
    public void ChekTrack(Collision collision)
    {
        _collision = collision;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isReaped)
        {
            return;
        }

        if (_collision.relativeVelocity.magnitude > _XmaxTankSpeedForTrackReap)
        {
            DestroyTrack();
        }

        //if (Mathf.Abs(_rb.velocity.x) > _XmaxTankSpeedForTrackReap)
        //{
        //    DestroyTrack();
        //}

        //if (Mathf.Abs(_rb.velocity.y)> _YmaxTankSpeedForTrackReap)
        //{
        //    DestroyTrack();
        //}

        //if (Mathf.Abs(_rb.velocity.z) > _XmaxTankSpeedForTrackReap)
        //{
        //    DestroyTrack();
        //}
    }
}
