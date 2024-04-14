using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChobiAssets.PTM;

public class TrackSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] _tracksLeft;
    [SerializeField] private GameObject[] _tracksRight;
    private bool _isReaped;

    void Update()
    {
        if (_isReaped)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _isReaped = true;
            DestroyTrack(_tracksLeft);
            DestroyTrack(_tracksRight);
            gameObject.GetComponent<Static_Track_Parent_CS>().enabled = false;
        }
    }

    private void DestroyTrack(GameObject [] tracks)
    {
        var i = 0;
        foreach (var track in tracks)
        {
            track.AddComponent<Rigidbody>();
            var HJ = track.AddComponent<HingeJoint>();
            HJ.breakForce = 10000;
            if (i>0)
            {
                HJ.connectedBody = tracks[i-1].GetComponent<Rigidbody>();
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
}
