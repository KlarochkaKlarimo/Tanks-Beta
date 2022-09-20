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
            _startTrackBonesPosition.Add(bone.position);
        }
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        switch (hp)
        {
            case 0:
                print("Modul destroed");
                TrackDestroyWithChance(100);
                break;

            case int n when (n <= 10):
                print("Modul damaged");

                TrackDestroyWithChance(20);
                break;


        }
    }

    private void TrackDestroyWithChance(int chance)
    {
        var isDestroyed = Random.Range(0, 100) <= chance;

        if (isDestroyed)
        {
            var i = 0;
            foreach (Transform bone in _trackBones)
            {
                bone.position = _trackBonesPosition[i];
                i++;
            }
            rearWheelDrive.enabled = false;
            Invoke("RepairTruck", 7f);
        }
    }

    private void RepairTruck()
    {
        rearWheelDrive.enabled = true;
        var i = 0;
        foreach (Transform bone in _trackBones)
        {
            bone.position = _startTrackBonesPosition[i];
            i++;
        }
    }
}
