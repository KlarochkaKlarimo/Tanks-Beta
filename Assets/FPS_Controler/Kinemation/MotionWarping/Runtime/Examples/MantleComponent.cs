// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kinemation.MotionWarping.Runtime.Examples
{
    public class MantleComponent : MonoBehaviour, IWarpPointProvider
    {
        [SerializeField] private MotionWarpingAsset mantleHigh;
        [SerializeField] private MotionWarpingAsset mantleLow;
        [SerializeField] private MantleSettings settings;
        
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        
        public WarpInteractionResult Interact(GameObject instigator)
        {
            WarpInteractionResult result = new WarpInteractionResult()
            {
                points = null,
                asset = null,
                success = false
            };

            if (settings == null)
            {
                return result;
            }

            var motionWarping = instigator.GetComponent<Core.MotionWarping>();

            Vector3 start = transform.position;
            Vector3 end = start;

            start.y += settings.minHeight + settings.characterCapsuleRadius;
            end.y += settings.maxHeight;

            Vector3 direction = transform.forward;
            float distance = settings.maxDistance;

            bool bHit = Physics.CapsuleCast(start, end, settings.characterCapsuleRadius, direction,
                out var hit, distance, settings.layerMask);
            
            if (!bHit)
            {
                return result;
            }

            _targetRotation = Quaternion.LookRotation(-hit.normal, transform.up);

            distance = (end - start).magnitude;
            
            start = hit.point;
            start += (_targetRotation * Vector3.forward) * settings.forwardOffset;
            
            start.y = end.y;

            bHit = Physics.SphereCast(start, settings.sphereEdgeCheckRadius, -transform.up, out hit,
                distance, settings.layerMask);
            
            start = hit.point;
            
            if (!bHit)
            {
                return result;
            }

            Vector3 surfaceNormal = hit.normal;
            
            start += surfaceNormal * (0.02f + settings.characterCapsuleRadius);
            end = start + surfaceNormal * settings.characterCapsuleHeight;
            
            bHit = Physics.CheckCapsule(start, end, settings.characterCapsuleRadius, settings.layerMask);
            
            if (bHit)
            {
                return result;
            }

            float surfaceIncline = Mathf.Clamp(Vector3.Dot(transform.up, surfaceNormal), -1f, 1f);
            surfaceIncline = Mathf.Acos(surfaceIncline) * Mathf.Rad2Deg;

            if (surfaceIncline > settings.maxSurfaceInclineAngle)
            {
                return result;
            }

            Vector3 forwardVector = _targetRotation * Vector3.forward;
            _targetRotation = Quaternion.LookRotation(forwardVector);
            _targetPosition = hit.point;

            result.points = new[]
            {
                new WarpPoint()
                {
                    transform = hit.transform,
                    position = hit.transform.InverseTransformPoint(_targetPosition),
                    rotation = Quaternion.Inverse(hit.transform.rotation) * _targetRotation
                }
            };

            float height = _targetPosition.y - transform.position.y;

            result.asset = height > settings.lowHeight ? mantleHigh : mantleLow;
            result.success = true;
            
#if UNITY_EDITOR
            Core.MotionWarping.AddWarpDebugData(motionWarping, new WarpDebugData()
            {
                duration = 5f,
                onDrawGizmos = () =>
                {
                    var color = Gizmos.color;
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(_targetPosition, 0.1f);
                    Handles.Label(_targetPosition, "Mantle Target Point");
                    Gizmos.color = color;
                }
            });
#endif
            
            return result;
        }
    }
}