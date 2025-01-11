// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Utility;

using System.Collections.Generic;
using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Core
{
    [CreateAssetMenu(menuName = "KINEMATION/MotionWarping/WarpingAsset", fileName = "NewWarpingAsset", order = 0)]
    public class MotionWarpingAsset : ScriptableObject
    {
        [Header("Animation")]
        public AnimationClip animation;
        
        public AnimationCurve rootX;
        public AnimationCurve rootY;
        public AnimationCurve rootZ;

        public VectorBool useLinear = VectorBool.Enabled;
        public VectorBool useAnimation = VectorBool.Enabled;
        public VectorBool useWarping = VectorBool.Enabled;

        [Tooltip("If true, CharacterController or a RigidBody will be moved with physics enabled.")]
        public bool useCollision = false;

        [Range(0f, 2f)] public float playRateBasis = 1f;
        
        [Range(1, 10)] public int phasesAmount = 1;
        public List<WarpPhase> warpPhases = new List<WarpPhase>();
        
        public Vector3 GetVectorValue(float time)
        {
            if (rootX == null || rootY == null || rootZ == null)
            {
                Debug.LogError(name + ": null reference curve!");
                return Vector3.zero;
            }

            return new Vector3(rootX.Evaluate(time), rootY.Evaluate(time), rootZ.Evaluate(time));
        }

        public float GetLength()
        {
            if (animation == null)
            {
                return 0f;
            }
            
            return animation.length;
        }
    }
}