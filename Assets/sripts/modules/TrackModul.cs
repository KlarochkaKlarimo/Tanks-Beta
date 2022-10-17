using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackModul : ModulBase
{
    [SerializeField] private Transform[] _trackBones;
    [SerializeField] private Vector3[] _trackBonesPosition;
    private List<Vector3> _startTrackBonesPosition;

    private void Start()
    {
        _startTrackBonesPosition = new List<Vector3>();
        foreach (Transform bone in _trackBones)
        {
            _startTrackBonesPosition.Add(bone.localPosition);
        }
    }

    public override void modulDamaged()
    {
        base.modulDamaged();
        TrackDestroyWithChance(20);
    }

    public override void modulDestroyed()
    {
        base.modulDestroyed();
        TrackDestroyWithChance(100);
    }
    
    private void TrackDestroyWithChance(int chance)
    {
        var isDestroyed = Random.Range(0, 100) <= chance;

        if (isDestroyed)
        {
            var i = 0;
            foreach (Transform bone in _trackBones)
            {
                bone.localPosition = _trackBonesPosition[i];
                i++;
            }
            tankWheelControl.enabled = false;
            Invoke("RepairTruck", 7f);
        }
    }

    private void RepairTruck()
    {
        tankWheelControl.enabled = true;
        var i = 0;
        foreach (Transform bone in _trackBones)
        {
            bone.localPosition = _startTrackBonesPosition[i];
            i++;
        }
    }
}
