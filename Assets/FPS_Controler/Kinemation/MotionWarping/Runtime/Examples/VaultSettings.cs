// Designed by KINEMATION, 2024

using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Examples
{
    [CreateAssetMenu(menuName = "KINEMATION/MotionWarping/VaultSettings", fileName = "New VaultSettings", order = 1)]
    public class VaultSettings : ScriptableObject
    {
        [Header("General Settings")] 
        public LayerMask layerMask;
        [Min(0f)] public float characterCapsuleRadius;
        [Min(0f)] public float maxObstacleLength;
        [Min(0f)] public float minObstacleLength;
        [Min(0f)] public float sphereEdgeCheckRadius;

        [Header("Close Edge Check")]
        [Min(0f)] public float maxAllowedStartLength;
        [Min(0f)] public float maxAllowedStartHeight;
        [Min(0f)] public float minAllowedStartHeight;
        
        [Header("Far Edge Check")]
        [Min(0f)] public float closeEdgeDeviation;

        [Header("End Check")] 
        [Min(0f)] public float farEdgeOffset;
        [Min(0f)] public float maxAllowedEndHeight;
        [Min(0f)] public float minAllowedEndHeight;
    }
}