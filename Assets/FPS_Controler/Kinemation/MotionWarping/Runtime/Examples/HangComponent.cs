// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;
using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Examples
{
    public class HangComponent : MonoBehaviour, IWarpPointProvider
    {
        [SerializeField] private MotionWarpingAsset hangAsset;
        [SerializeField] private MotionWarpingAsset jumpDownAsset;
        
        [Header("Trace Settings")]
        [SerializeField] [Min(0f)] private float capsuleRadius;
        [SerializeField] [Min(0f)] private float sphereCheckRadius;
        [SerializeField] [Min(0f)] private float minLedgeLength;
        [SerializeField] [Min(0f)] private LayerMask layerMask;
        
        [Header("Hanging")]
        [SerializeField] [Min(0f)] private float maxAllowedDistance;
        [SerializeField] [Min(0f)] private float maxAllowedHeight;
        [SerializeField] [Min(0f)] private float minAllowedHeight;

        private bool _isHanging;

        private WarpInteractionResult TryToJumpDown()
        {
            WarpInteractionResult result = new WarpInteractionResult()
            {
                success = false,
                points = null,
                asset = null,
            };

            Vector3 start = transform.position;
            bool bHit = Physics.SphereCast(start, sphereCheckRadius, -transform.up, out var hit, 
                maxAllowedHeight, layerMask);

            if (!bHit)
            {
                return result;
            }

            Quaternion resultRotation = transform.rotation;
            Vector3 resultPosition = hit.point;

            result.points = new[]
            {
                new WarpPoint()
                {
                    position = resultPosition,
                    rotation = resultRotation
                }
            };
            result.asset = jumpDownAsset;
            result.success = true;

            _isHanging = false;
            return result;
        }

        private WarpInteractionResult TryToHang()
        {
            WarpInteractionResult result = new WarpInteractionResult()
            {
                success = false,
                points = null,
                asset = null,
            };
            
            Vector3 start = transform.position;
            Vector3 end = start;

            start.y += minAllowedHeight;
            end.y += minAllowedHeight + maxAllowedHeight;

            Vector3 direction = transform.forward;
            float distance = maxAllowedDistance;

            bool bHit = Physics.CapsuleCast(start, end, capsuleRadius, direction,
                out var hit, distance, layerMask);

            if (!bHit)
            {
                return result;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(-hit.normal, transform.up);
            
            distance = (end - start).magnitude;

            start = hit.point;
            start.y = end.y;

            bHit = Physics.SphereCast(start, sphereCheckRadius, -transform.up, out hit, distance,
                layerMask);

            if (!bHit)
            {
                return result;
            }

            start = hit.point;
            start.y += sphereCheckRadius + 0.01f;

            Vector3 targetPosition = hit.point;

            bHit = Physics.SphereCast(start, sphereCheckRadius, targetRotation * Vector3.forward, out hit,
                minLedgeLength, layerMask);

            if (bHit)
            {
                return result;
            }

            result.success = true;
            result.asset = hangAsset;
            result.points = new[]
            {
                new WarpPoint()
                {
                    position = targetPosition,
                    rotation = targetRotation
                }
            };
            
            _isHanging = true;
            return result;
        }
        
        public WarpInteractionResult Interact(GameObject instigator)
        {
            return _isHanging ? TryToJumpDown() : TryToHang();
        }
    }
}