// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;

using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Kinemation.MotionWarping.Runtime.Examples
{
    public class AlignComponent : MonoBehaviour, IWarpPointProvider
    {
        [SerializeField] [Range(0f, 180f)] private float interactionAngle = 0f;
        [SerializeField] [Range(-180f, 180f)] private float offsetAngle = 0f;
        [SerializeField] [Min(0f)] private float distance = 0f;
        [SerializeField] private MotionWarpingAsset motionWarpingAsset;
        [SerializeField] private string targetAnimName;

        private Animator _animator;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 position = transform.position;
            Vector3 forward = Quaternion.Euler(0f, offsetAngle, 0f) * transform.forward;
            float debugRadius = 0.05f;
            
            Vector3 left = Quaternion.Euler(0f, interactionAngle, 0f) * forward;
            Vector3 right = Quaternion.Euler(0f, -interactionAngle, 0f) * forward;
            
            Gizmos.DrawLine(position, position + left * distance);
            Gizmos.DrawLine(position, position + right * distance);
            
            Gizmos.DrawWireSphere(position, debugRadius);
            Gizmos.DrawWireSphere(position + left * distance, debugRadius);
            Gizmos.DrawWireSphere(position + right * distance, debugRadius);
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public WarpInteractionResult Interact(GameObject instigator)
        {
            WarpInteractionResult result = new WarpInteractionResult()
            {
                success = false,
                points = null,
                asset = null,
            };

            if (instigator == null || motionWarpingAsset == null)
            {
                return result;
            }

            if ((instigator.transform.position - transform.position).magnitude > distance)
            {
                return result;
            }

            Vector3 forward = Quaternion.Euler(0f, offsetAngle, 0f) * transform.forward;
            float angle = Mathf.Acos(Vector3.Dot(-instigator.transform.forward, forward)) * Mathf.Rad2Deg;
            
            if (angle > interactionAngle)
            {
                return result;
            }
            
            result.asset = motionWarpingAsset;
            result.points = new[]
            {
                new WarpPoint()
                {
                    transform = this.transform,
                    position = Vector3.zero,
                    rotation = Quaternion.identity
                }
            };
            
            result.success = true;

            if (_animator != null)
            {
                _animator.CrossFade(targetAnimName, 0.15f);
            }
            
            return result;
        }
    }
}
