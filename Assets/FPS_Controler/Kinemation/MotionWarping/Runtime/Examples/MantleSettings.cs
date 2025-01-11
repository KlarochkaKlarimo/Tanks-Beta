// Designed by KINEMATION, 2024

using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Examples
{
    [CreateAssetMenu(menuName = "KINEMATION/MotionWarping/MantleSettings", fileName = "NewMantleSettings", order = 2)]
    public class MantleSettings : ScriptableObject
    {
        public LayerMask layerMask;

        [Min(0f)] public float maxHeight;
        [Min(0f)] public float lowHeight;
        [Min(0f)] public float minHeight;
        [Min(0f)] public float maxDistance;
        
        [Min(0f)] public float characterCapsuleRadius;
        [Min(0f)] public float characterCapsuleHeight;
        [Min(0f)] public float sphereEdgeCheckRadius;

        [Range(0f, 90f)] public float maxSurfaceInclineAngle;
        
        [Min(0f)] public float forwardOffset;
    }
}
