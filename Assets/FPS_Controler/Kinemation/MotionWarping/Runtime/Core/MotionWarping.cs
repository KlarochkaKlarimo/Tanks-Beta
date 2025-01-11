// Designed by KINEMATION, 2024.

using KINEMATION.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Kinemation.MotionWarping.Runtime.Core
{
    public class MotionWarping : MonoBehaviour
    {
        [Header("Warping")]
        [SerializeField] private bool scalePlayRate = true;
        
        [Header("Animator")]
        [SerializeField, Tooltip("Will try to play the animation.")] private bool playAnimator;
        [SerializeField, Range(0f, 1f)] private float blendTime;

        [Header("Events")]
        public UnityEvent onWarpStarted;
        public UnityEvent onWarpEnded;
        
        [SerializeField] 
        private Animator animator;
        public WarpPhase[] warpPhases;
        
        private Vector3 _endCurveValue;
        private Vector3 _startCurveValue;

        private int _phaseIndex;

        private MotionWarpingAsset _asset;
        private WarpPhase _warpPhase;
        private float _nextPhaseTime;

        private Vector3 _originPosition;
        private Quaternion _originRotation;
        
        private bool _bUpdateWarping;
        private float _warpPlayback;

        private float _rateScale = 1f;
        private float _warpLength;

        private Vector3 _accumRootMotion;
        private Vector3 _rootMotion;

        private bool _hasActivePhase;
        private static readonly int WarpRate = Animator.StringToHash("WarpRate");

        private MotionWarpingIk _motionWarpingIk;

        private CharacterController _characterController;
        private Rigidbody _rigidBody;

        private void OnWarpStarted_Internal()
        {
            // Disable collision if needed.
            if (_characterController != null)
            {
                _characterController.enabled = _asset.useCollision;
            }

            if (_rigidBody != null)
            {
                _rigidBody.isKinematic = _asset.useCollision;
            }
        }

        private void OnWarpEnded_Internal()
        {
            // Restore collision if needed.
            if (_characterController != null && !_asset.useCollision)
            {
                _characterController.enabled = true;
            }

            if (_rigidBody != null && !_asset.useCollision)
            {
                _rigidBody.isKinematic = true;
            }
        }
        
        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            _motionWarpingIk = GetComponent<MotionWarpingIk>();
            
            _characterController = GetComponent<CharacterController>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            onWarpEnded = onWarpStarted = null;
        }
        
        private float InvLerp(float startValue, float targetValue, float curveValue)
        {
            if (Mathf.Approximately(startValue, targetValue)) return 0f;
            
            float numerator = curveValue - startValue;
            float denominator = targetValue - startValue;

            return Mathf.Approximately(denominator, 0f) ? 0f : numerator / denominator;
        }

        private float SafeDivide(float a, float b)
        {
            if (Mathf.Approximately(b, 0f)) return 0f;
            return a / b;
        }

        private float GetNormalizedPlayback()
        {
            return _warpPlayback / _warpLength;
        }

        private float GetPhaseProgress()
        {
            float alpha = InvLerp(_warpPhase.startTime, _warpPhase.endTime, _warpPlayback);
            return Mathf.Clamp01(alpha);
        }

        private void EnterNewPhase()
        {
            _originPosition = transform.position;
            _originRotation = transform.rotation;

            _accumRootMotion = _rootMotion = Vector3.zero;
            
            _warpPhase = warpPhases[_phaseIndex];
            _nextPhaseTime = _phaseIndex == warpPhases.Length - 1 ? _warpLength : warpPhases[_phaseIndex + 1].startTime;
            _hasActivePhase = true;

            _startCurveValue = _asset.GetVectorValue(_warpPhase.startTime);
            _endCurveValue = _asset.GetVectorValue(_warpPhase.endTime);
            
            _phaseIndex++;

            if (scalePlayRate && animator != null)
            {
                float curveVec = (_endCurveValue - _startCurveValue).magnitude;
                float realVec = (_warpPhase.Target.GetPosition() - _originPosition).magnitude;
                realVec = Mathf.Max(0.001f, realVec);

                _rateScale = Mathf.Clamp(curveVec / realVec, _warpPhase.minRate, _warpPhase.maxRate);
                _rateScale *= _asset.playRateBasis;

                animator.SetFloat(WarpRate, _rateScale);
            }
            else
            {
                _rateScale = 1f;
            }

            if (_warpPhase.Target.transform != null)
            {
                _originPosition = _warpPhase.Target.transform.InverseTransformPoint(transform.position);
                _originRotation = Quaternion.Inverse(_warpPhase.Target.transform.rotation) * transform.rotation;
            }
        }

        private void ExitCurrentPhase()
        {
            _hasActivePhase = false;

            if (_warpPhase.Target.transform == null)
            {
                _originPosition = transform.position;
                _originRotation = transform.rotation;
            }
            else
            {
                _originPosition = _warpPhase.Target.transform.InverseTransformPoint(transform.position);
                _originRotation = Quaternion.Inverse(_warpPhase.Target.transform.rotation) * transform.rotation;
            }
            
            _startCurveValue = _endCurveValue = _asset.GetVectorValue(_warpPlayback);
        }
        
        private Quaternion WarpRotation()
        {
            float alpha = _hasActivePhase ? GetPhaseProgress() : 0f;
            return Quaternion.Slerp(transform.rotation, _warpPhase.Target.GetRotation(), alpha);
        }

        private Vector3 WarpTranslation()
        {
            // 1. Compute the original additive curve value
            Vector3 prevRootMotion = _rootMotion;
            _rootMotion = _asset.GetVectorValue(_warpPlayback) - _startCurveValue;

            if (!_hasActivePhase)
            {
                // 2. If not in the segment - play the animation itself.

                Vector3 modifiedRootMotion = _rootMotion;
                modifiedRootMotion.x = _asset.useAnimation.x ? modifiedRootMotion.x : 0f;
                modifiedRootMotion.y = _asset.useAnimation.y ? modifiedRootMotion.y : 0f;
                modifiedRootMotion.z = _asset.useAnimation.z ? modifiedRootMotion.z : 0f;
                return modifiedRootMotion;
            }
            
            // 3. Compute the target in the origin space
            Vector3 localTarget = transform.InverseTransformPoint(_warpPhase.Target.GetPosition());

            // 4. Compute the deltas.
            Vector3 animationTarget = _endCurveValue - _startCurveValue;
            animationTarget.x = _asset.useAnimation.x ? animationTarget.x : 0f;
            animationTarget.y = _asset.useAnimation.y ? animationTarget.y : 0f;
            animationTarget.z = _asset.useAnimation.z ? animationTarget.z : 0f;
            
            Vector3 targetDelta = localTarget - animationTarget;

            Vector3 rootMotionDelta = _rootMotion - prevRootMotion;
            _accumRootMotion.x += Mathf.Abs(rootMotionDelta.x);
            _accumRootMotion.y += Mathf.Abs(rootMotionDelta.y);
            _accumRootMotion.z += Mathf.Abs(rootMotionDelta.z);
            
            // 5. Finally warp the motion.
            targetDelta.x *= _asset.useLinear.x
                ? GetPhaseProgress()
                : Mathf.Clamp01(SafeDivide(_accumRootMotion.x, _warpPhase.totalRootMotion.x));

            targetDelta.y *= _asset.useLinear.y
                ? GetPhaseProgress()
                : Mathf.Clamp01(SafeDivide(_accumRootMotion.y, _warpPhase.totalRootMotion.y));

            targetDelta.z *= _asset.useLinear.z
                ? GetPhaseProgress()
                : Mathf.Clamp01(SafeDivide(_accumRootMotion.z, _warpPhase.totalRootMotion.z));

            Vector3 rootAnimation = Vector3.zero;
            rootAnimation.x = _asset.useAnimation.x ? _rootMotion.x : 0f;
            rootAnimation.y = _asset.useAnimation.y ? _rootMotion.y : 0f;
            rootAnimation.z = _asset.useAnimation.z ? _rootMotion.z : 0f;
            
            return rootAnimation + targetDelta;
        }

        private void WarpAnimation()
        {
            Vector3 cachedPosition = transform.position;
            
            if (_warpPhase.Target.transform == null)
            {
                transform.position = _originPosition;
                transform.rotation = _originRotation;
            }
            else
            {
                transform.position = _warpPhase.Target.transform.TransformPoint(_originPosition);
                transform.rotation = _warpPhase.Target.transform.rotation * _originRotation;
            }
            
            Vector3 warpedTranslation = WarpTranslation();
            Quaternion warpedRotation = WarpRotation();
            
            warpedTranslation = transform.TransformPoint(warpedTranslation);

            if (_asset.useCollision)
            {
                if (_characterController != null)
                {
                    Vector3 delta = warpedTranslation - cachedPosition;
                    delta.x = _asset.useWarping.x ? delta.x : 0;
                    delta.y = _asset.useWarping.y ? delta.y : 0;
                    delta.z = _asset.useWarping.z ? delta.z : 0;
                
                    _characterController.Move(delta);
                }
                else if (_rigidBody != null)
                {
                    Vector3 delta = warpedTranslation - cachedPosition;
                    delta.x = _asset.useWarping.x ? delta.x : 0;
                    delta.y = _asset.useWarping.y ? delta.y : 0;
                    delta.z = _asset.useWarping.z ? delta.z : 0;
                
                    _rigidBody.MovePosition(delta);
                }
            }
            else
            {
                transform.position = warpedTranslation;
            }
            
            transform.rotation = warpedRotation;
        }

        private void UpdateWarping()
        {
            if (_warpPlayback > _warpPhase.endTime && _hasActivePhase)
            {
                ExitCurrentPhase();
            }
            
            if (!_hasActivePhase && _warpPlayback > _nextPhaseTime)
            {
                EnterNewPhase();
            }

            WarpAnimation();
            
            // Update playback
            _warpPlayback += Time.deltaTime * _rateScale;
            _warpPlayback = Mathf.Clamp(_warpPlayback, 0f, _warpLength);
            
            if (Mathf.Approximately(GetNormalizedPlayback(), 1f))
            {
                Stop();
            }
        }

        private void LateUpdate()
        {
            if (!_bUpdateWarping) return;
            
            UpdateWarping();

            if (_motionWarpingIk == null) return;
            _motionWarpingIk.ApplyIK();
        }

        private void Play_Internal(MotionWarpingAsset motionWarpingAsset)
        {
            if (playAnimator && animator != null)
            {
                animator.CrossFade(motionWarpingAsset.animation.name, blendTime);
            }
            
            _startCurveValue = _endCurveValue = _accumRootMotion = _rootMotion = Vector3.zero;

            _warpPhase.Target.transform = null;
            _originPosition = transform.position;
            _originRotation = transform.rotation;
            
            _asset = motionWarpingAsset;
            
            _phaseIndex = 0;
            _nextPhaseTime = warpPhases[0].startTime;
           
            _bUpdateWarping = true;
            _hasActivePhase = false;
            
            _rateScale = 1f;
            _warpLength = motionWarpingAsset.GetLength();
            
            onWarpStarted.Invoke();
            OnWarpStarted_Internal();
        }

        public bool Interact(GameObject target)
        {
            if (target == null)
            {
                return false;
            }

            return Interact(target.GetComponent<IWarpPointProvider>());
        }

        public bool Interact(IWarpPointProvider target)
        {
            if (target == null)
            {
                return false;
            }

            var result = target.Interact(gameObject);
            if (!result.IsValid())
            {
                return false;
            }
            
            Play(result.asset, result.points);
            return true;
        }
        
        public void Play(MotionWarpingAsset motionWarpingAsset, WarpPoint[] warpPoints)
        {
            if (motionWarpingAsset == null)
            {
                Debug.LogError("MotionWarping: WarpPoint[] warpPoints is null!");
                return;
            }
            
            if (warpPoints == null)
            {
                Debug.LogError("MotionWarping: Warp Points array is null!");
                return;
            }
            
            warpPhases = motionWarpingAsset.warpPhases.ToArray();

            if (warpPhases.Length != warpPoints.Length)
            {
                Debug.LogError("MotionWarping: Warp Phases and Warp Points array do not match!");
                return;
            }

            for (int i = 0; i < warpPhases.Length; i++)
            {
                WarpPhase phase = warpPhases[i];
                WarpPoint target = warpPoints[i];

                if (target.transform == null)
                {
                    phase.Target.position = WarpingUtility.ToWorld(target.position, target.rotation, 
                        phase.tOffset);
                    phase.Target.rotation = target.rotation * Quaternion.Euler(phase.rOffset);
                }
                else
                {
                    phase.Target.transform = target.transform;

                    phase.Target.position = target.position;
                    phase.Target.rotation = target.rotation;
                    
                    phase.Target.localPosition = phase.tOffset;
                    phase.Target.localRotation = phase.rOffset;
                }
                
                warpPhases[i] = phase;
            }
            
            Play_Internal(motionWarpingAsset);
        }

        public void Stop()
        {
            _bUpdateWarping = false;
            _warpPlayback = 0f;
            onWarpEnded.Invoke();
            OnWarpEnded_Internal();
        }

        public bool IsActive()
        {
            return _bUpdateWarping;
        }
        
#if UNITY_EDITOR
        private List<WarpDebugData> _warpDebugData = new List<WarpDebugData>();

        public static void AddWarpDebugData(MotionWarping target, WarpDebugData warpDebugData)
        {
            if (target == null) return;
            target._warpDebugData.Add(warpDebugData);
        }
        
        private void OnDrawGizmos()
        {
            for (int i = 0; i < _warpDebugData.Count; i++)
            {
                var debugData = _warpDebugData[i];
                debugData.onDrawGizmos?.Invoke();
                if(debugData.duration < 0f) continue;

                // Progress the timer.
                debugData.timer = Mathf.Clamp(debugData.timer + Time.deltaTime, 0f, debugData.duration);
                _warpDebugData[i] = debugData;

                if (Mathf.Approximately(debugData.timer, debugData.duration))
                {
                    _warpDebugData.RemoveAt(i);
                    i--;
                }
            }
        }
#endif
    }
}