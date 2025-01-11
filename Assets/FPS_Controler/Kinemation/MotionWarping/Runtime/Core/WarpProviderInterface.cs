// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Utility;
using UnityEngine;

namespace Kinemation.MotionWarping.Runtime.Core
{
    public struct WarpInteractionResult
    {
        public WarpPoint[] points;
        public MotionWarpingAsset asset;
        public bool success;

        public bool IsValid()
        {
            return success && points != null && asset != null;
        }
    }
    
    public interface IWarpPointProvider
    {
        public WarpInteractionResult Interact(GameObject instigator);
    }
}