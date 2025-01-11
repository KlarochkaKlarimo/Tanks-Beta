// Designed by KINEMATION, 2024.

using System.Collections.Generic;
using Kinemation.MotionWarping.Runtime.Utility;
using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Core
{
    // Obstacle with pre-defined Warp Points
    public class WarpObstacle : MonoBehaviour, IWarpPointProvider
    {
        [SerializeField] private MotionWarpingAsset motionWarpingAsset;
        [SerializeField] private List<Transform> points = new List<Transform>();
        [SerializeField] private bool useTransforms = false;
        [SerializeField] private bool drawDebugPoints = false;

        private void OnDrawGizmos()
        {
            if (!drawDebugPoints) return;

            var color = Gizmos.color;
            Gizmos.color = Color.green;

            foreach (var point in points)
            {
                if(point == null) continue;
                Gizmos.DrawWireSphere(point.position, 0.07f);
            }
            
            Gizmos.color = color;
        }

        public WarpInteractionResult Interact(GameObject instigator)
        {
            WarpInteractionResult result;
            result.points = new WarpPoint[points.Count];
            result.asset = motionWarpingAsset;
            result.success = points.Count > 0;
            
            for (int i = 0; i < result.points.Length; i++)
            {
                if (useTransforms)
                {
                    result.points[i] = new WarpPoint()
                    {
                        transform = points[i]
                    };
                }
                else
                {
                    result.points[i] = new WarpPoint()
                    {
                        position = points[i].position,
                        rotation = points[i].rotation
                    };
                }
            }
            
            return result;
        }
    }
}