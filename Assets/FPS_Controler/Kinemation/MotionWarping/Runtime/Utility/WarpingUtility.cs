// Designed by KINEMATION, 2024

using System;
using KINEMATION.MotionWarping.Runtime.Core;
using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Utility
{
    public struct WarpDebugData
    {
        public float duration;
        public float timer;
        public Action onDrawGizmos;
    }
    
    public struct WarpPoint
    {
        // Target transform in world space
        public Transform transform;
        public Vector3 localPosition;
        public Vector3 localRotation;

        // Target position in world space
        public Vector3 position;

        // Target rotation in world space
        public Quaternion rotation;

        public WarpPoint(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            localPosition = localRotation = Vector3.zero;
            this.transform = null;
        }
        
        public Vector3 GetPosition()
        {
            if (transform == null)
            {
                return position;
            }

            // Get the raw warp point in world space.
            Vector3 rawPosition = transform.TransformPoint(position);
            Quaternion rawRotation = transform.rotation * rotation;

            return WarpingUtility.ToWorld(rawPosition, rawRotation, localPosition);
        }

        public Quaternion GetRotation()
        {
            if (transform == null)
            {
                return rotation;
            }

            return transform.rotation * rotation * Quaternion.Euler(localRotation);
        }
    }
    
    [Serializable]
    public struct WarpPhase
    {
        // Target point
        public WarpPoint Target;

        // Translation offset for the B point
        public Vector3 tOffset;

        // Angular offset for the B point
        public Vector3 rOffset;
        
        [Min(0f)] public float startTime;
        [Min(0f)] public float endTime;

        // Min allowed play rate
        [Range(0f, 1f)] public float minRate;

        // Max allowed play rate
        [Range(1f, 2f)] public float maxRate;

        [ReadOnly] public Vector3 totalRootMotion;
    }

    public struct WarpingCurve
    {
        public AnimationCurve X;
        public AnimationCurve Y;
        public AnimationCurve Z;
    }

    [Serializable]
    public struct VectorBool
    {
        public bool x;
        public bool y;
        public bool z;

        public VectorBool(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static VectorBool Enabled = new VectorBool(true, true, true);
    }

    public static class WarpingUtility
    {
        public static float ExpDecayAlpha(float speed, float deltaTime)
        {
            return 1 - Mathf.Exp(-speed * deltaTime);
        }

        public static Vector3 ToWorld(Vector3 tWorld, Quaternion rWorld, Vector3 localOffset)
        {
            return tWorld + rWorld * localOffset;
        }

        public static Vector3 ToLocal(Vector3 tWorld, Quaternion rWorld, Vector3 worldOffset)
        {
            return Quaternion.Inverse(rWorld) * (worldOffset - tWorld);
        }

        public static void SolveTwoBoneIK(Transform bone, Transform target, float weight)
        {
            Transform mid = bone.parent;
            TwoBoneIk.SolveTwoBoneIK(mid.parent, mid, bone, (target.position, target.rotation),  mid, 
                weight, 1f);
        }
    }
}