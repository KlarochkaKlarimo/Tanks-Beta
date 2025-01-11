// Designed by KINEMATION, 2024.

using System;
using UnityEngine;

namespace KINEMATION.MotionWarping.Runtime.Core
{
    [Serializable]
    public struct LockState
    {
        public string controlCurveName;
        public Transform boneReference;
        [Min(0f)] public float interpSpeed;
        [NonSerialized] public float Weight;
        [NonSerialized] public Quaternion LockRotation;
        [NonSerialized] public Vector3 LockPosition;
        [NonSerialized] public bool IsLocked;
        [NonSerialized] public bool Interpolate;
    }
    
    public class MotionWarpingIk : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float componentWeight = 1f;
        
        [SerializeField] private LockState rightHandLockState;
        [SerializeField] private LockState leftHandLockState;
        [SerializeField] private LockState rightFootLockState;
        [SerializeField] private LockState leftFootLockState;
        
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        // Updated the current bone lock state.
        private void UpdateBoneLock(ref LockState lockState)
        {
            Transform boneReference = lockState.boneReference;
            if (boneReference == null)
            {
                return;
            }
            
            float weight = _animator.GetFloat(lockState.controlCurveName);
            
            if (!lockState.IsLocked && Mathf.Approximately(weight, 1f))
            {
                lockState.LockPosition = boneReference.position;
                lockState.LockRotation = boneReference.rotation;
                lockState.IsLocked = true;
                lockState.Interpolate = false;
                lockState.Weight = 1f;
                return;
            }

            if (lockState.IsLocked && Mathf.Approximately(weight, 0f))
            {
                lockState.IsLocked = false;
                lockState.Interpolate = true;
            }

            if (!lockState.Interpolate)
            {
                return;
            }
            
            float alpha = 1f - Mathf.Exp(-lockState.interpSpeed * Time.deltaTime);
            lockState.Weight = Mathf.Lerp(lockState.Weight, 0f, alpha);
        }

        private void ApplyLock(ref LockState lockState)
        {
            Transform tip = lockState.boneReference;
            if (tip == null)
            {
                return;
            }

            Transform mid = tip.parent;
            TwoBoneIk.SolveTwoBoneIK(mid.parent, mid, tip, (lockState.LockPosition, lockState.LockRotation), 
                mid, lockState.Weight * componentWeight, 1f);
            
            UpdateBoneLock(ref lockState);
        }

        public void ApplyIK()
        {
            ApplyLock(ref rightHandLockState);
            ApplyLock(ref leftHandLockState);
            ApplyLock(ref rightFootLockState);
            ApplyLock(ref leftFootLockState);
        }
    }
}