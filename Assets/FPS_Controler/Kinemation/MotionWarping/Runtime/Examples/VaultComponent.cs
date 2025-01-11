// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kinemation.MotionWarping.Runtime.Examples
{
    public class VaultComponent : MonoBehaviour, IWarpPointProvider
    {
        [SerializeField] private MotionWarpingAsset motionWarpingAsset;
        [SerializeField] private VaultSettings settings;
        
        private Vector3 _closeEdge;
        private Vector3 _farEdge;
        private Vector3 _endPoint;
        private Quaternion _targetRotation;

        private bool FindCloseEdge()
        {
            if (settings == null)
            {
                return false;
            }
            
            Vector3 start = transform.position;
            Vector3 end = start + transform.up * settings.maxAllowedStartHeight;
            start.y += settings.characterCapsuleRadius + settings.minAllowedStartHeight;
            
            Vector3 direction = transform.forward;

            bool bHit = Physics.CapsuleCast(start, end, settings.characterCapsuleRadius, direction,
                out var hit, settings.maxAllowedStartLength, settings.layerMask);

            if (!bHit)
            {
                return false;
            }
            
            _targetRotation = Quaternion.LookRotation(-hit.normal, transform.up);

            start = hit.point;
            start.y = end.y;
            direction = -transform.up;

            bHit = Physics.SphereCast(start, settings.sphereEdgeCheckRadius, direction, out hit,
                settings.maxAllowedStartHeight, settings.layerMask);

            if (!bHit)
            {
                return false;
            }

            _closeEdge = hit.point;
            
            return true;
        }

        private bool FindEndPoint()
        {
            Vector3 start = _farEdge + (_targetRotation * Vector3.forward) * settings.farEdgeOffset;
            Vector3 direction = -transform.up;
            float distance = settings.maxAllowedEndHeight;

            bool bHit = Physics.SphereCast(start, settings.sphereEdgeCheckRadius, direction, out var hit,
                distance, settings.layerMask);

            if (!bHit || (hit.point - start).magnitude < settings.minAllowedEndHeight)
            {
                return false;
            }

            _endPoint = hit.point;
            return true;
        }

        private bool FindEndEdge()
        {
            Vector3 forward = (_targetRotation * Vector3.forward).normalized;
            
            float length = settings.maxObstacleLength + settings.characterCapsuleRadius;
            Vector3 start = _closeEdge + forward * length;
            Vector3 end = start;
            
            start.y = _closeEdge.y - settings.closeEdgeDeviation;
            end.y = _closeEdge.y + settings.closeEdgeDeviation;

            length -= settings.minObstacleLength;

            bool bHit = Physics.CapsuleCast(start, end, settings.characterCapsuleRadius,
                -forward, out var hit, length, settings.layerMask);

            if (!bHit) return false;

            start = hit.point;
            start.y = _closeEdge.y + settings.closeEdgeDeviation + settings.sphereEdgeCheckRadius;

            bHit = Physics.SphereCast(start, settings.sphereEdgeCheckRadius, -transform.up, out hit,
                settings.closeEdgeDeviation * 2f, settings.layerMask);

            if (!bHit) return false;

            _farEdge = hit.point;
            return true;
        }

        public WarpInteractionResult Interact(GameObject instigator)
        {
            WarpInteractionResult result = new WarpInteractionResult()
            {
                points = null,
                asset = null,
                success = false
            };

            if (motionWarpingAsset == null)
            {
                return result;
            }

            var motionWarping = instigator.GetComponent<Core.MotionWarping>();
            
            bool success = FindCloseEdge() && FindEndEdge() && FindEndPoint();

            if (!success)
            {
                return result;
            }

            result.asset = motionWarpingAsset;

            result.points = new WarpPoint[]
            {
                new WarpPoint()
                {
                    position = _closeEdge,
                    rotation = _targetRotation
                },
                new WarpPoint()
                {
                    position = _farEdge,
                    rotation = _targetRotation
                },
                new WarpPoint()
                {
                    position = _endPoint,
                    rotation = _targetRotation
                }
            };

            result.success = true;
            
#if UNITY_EDITOR
            Core.MotionWarping.AddWarpDebugData(motionWarping, new WarpDebugData()
            {
                duration = 5f,
                onDrawGizmos = () =>
                {
                    var color = Gizmos.color;

                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(_closeEdge, 0.1f);
                    Handles.Label(_closeEdge, "Close Edge");
                    
                    Gizmos.DrawWireSphere(_farEdge, 0.1f);
                    Handles.Label(_farEdge, "Far Edge");
                    
                    Gizmos.DrawWireSphere(_endPoint, 0.1f);
                    Handles.Label(_endPoint, "End Point");

                    Gizmos.color = color;
                }
            });
#endif
            
            return result;
        }
    }
}