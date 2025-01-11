// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;

using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace KINEMATION.MotionWarping.Editor.Widgets
{
    public class WarpPosePreviewWidget : IWarpWidgetInterface
    {
        private GameObject _warpTracerObject;
        private Vector3 _basePos;
        private Quaternion _baseRot;
        
        private GameObject _sceneCharacter;
        private AnimationClipPlayable _previewMotion;
        private int _phaseSlider = 0;
        private bool _preview = false;

        private WarpPoint[] _warpPoints = null;
        private Dictionary<string, (Vector3, Quaternion)> _cachedTransforms 
            = new Dictionary<string, (Vector3, Quaternion)>();
        
        private PlayableGraph _playableGraph;
        private MotionWarpingAsset _motionWarpingAsset;

        public WarpPosePreviewWidget(MotionWarpingAsset asset)
        {
            _motionWarpingAsset = asset;
        }
        
        private void CacheTransforms()
        {
            _cachedTransforms.Clear();
            Transform[] allChildren = _sceneCharacter.GetComponentsInChildren<Transform>();

            foreach (Transform child in allChildren)
            {
                (Vector3, Quaternion) data;
                data.Item1 = child.localPosition;
                data.Item2 = child.localRotation;
                
                _cachedTransforms[child.name] = data;
            }
        }
        
        private void ApplyCachedTransforms()
        {
            if (_sceneCharacter == null) return;
            
            Transform[] allChildren = _sceneCharacter.GetComponentsInChildren<Transform>();

            foreach (Transform child in allChildren)
            {
                if (_cachedTransforms.ContainsKey(child.name))
                {
                    child.localPosition = _cachedTransforms[child.name].Item1;
                    child.localRotation = _cachedTransforms[child.name].Item2;
                }
            }
        }

        public void RestorePose()
        {
            if (_playableGraph.IsValid())
            {
                _playableGraph.Destroy();
            }

            if (_previewMotion.IsValid())
            {
                _previewMotion.Destroy();
            }

            if (_preview)
            {
                ApplyCachedTransforms();
            }
            
            _cachedTransforms.Clear();
        }
        
        public void Render()
        {
            _sceneCharacter = (GameObject) EditorGUILayout.ObjectField("Character", _sceneCharacter,
                typeof(GameObject), true);
            
            if (_sceneCharacter == null)
            {
                EditorGUILayout.HelpBox("Missing Character Reference!", MessageType.Error);
                return;
            }

            Animator animator = _sceneCharacter.GetComponentInChildren<Animator>();

            if (animator == null)
            {
                EditorGUILayout.HelpBox("No Animator Found!", MessageType.Error);
                return;
            }

            _warpTracerObject = (GameObject) EditorGUILayout.ObjectField("Source", _warpTracerObject,
                typeof(GameObject), true);

            if (_warpTracerObject == null)
            {
                EditorGUILayout.HelpBox("Select Warp Tracer game object.", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            AnimationPlayableOutput playableOutput;

            if (GUILayout.Button("Play"))
            {
                CacheTransforms();
                
                _playableGraph = PlayableGraph.Create("WarpPosePreviewGraph");

                playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation",
                    animator);

                _previewMotion = AnimationClipPlayable.Create(_playableGraph, _motionWarpingAsset.animation);
                playableOutput.SetSourcePlayable(_previewMotion);
                _preview = true;
                animator.fireEvents = false;
                animator.applyRootMotion = false;

                if (_warpTracerObject.GetComponent<IWarpPointProvider>() is var tracer)
                {
                    var result = tracer.Interact(_sceneCharacter);
                    _warpPoints = result.points;
                }

                _phaseSlider = -1;
            }

            if (GUILayout.Button("Stop"))
            {
                _preview = false;
                animator.Rebind();
                animator.fireEvents = true;
                animator.applyRootMotion = true;
                _phaseSlider = 0;

                if (_playableGraph.IsValid())
                {
                    _playableGraph.Stop();
                    _playableGraph.Destroy();
                }

                if (_previewMotion.IsValid())
                {
                    _previewMotion.Destroy();
                }
                
                ApplyCachedTransforms();
            }

            EditorGUILayout.EndHorizontal();

            if (!_preview) return;
            
            if (_warpPoints == null || _warpPoints.Length != _motionWarpingAsset.warpPhases.Count)
            {
                string msg = "MotionWarping: WarpObject points are null or size does not match!";
                EditorGUILayout.HelpBox(msg, MessageType.Error);
                return;
            }

            int max = _motionWarpingAsset.warpPhases.Count - 1;

            int prevSlider = _phaseSlider;
            _phaseSlider = EditorGUILayout.IntSlider("Phase", _phaseSlider, 0, max);

            if (_previewMotion.IsValid())
            {
                float time = _motionWarpingAsset.warpPhases[_phaseSlider].endTime;
                _previewMotion.SetTime(time);
            }

            var phase = _motionWarpingAsset.warpPhases[_phaseSlider];
            
            if (prevSlider != _phaseSlider)
            {
                _playableGraph.Evaluate();
                
                _sceneCharacter.transform.position = _warpPoints[_phaseSlider].GetPosition();
                _sceneCharacter.transform.rotation = _warpPoints[_phaseSlider].GetRotation();

                _basePos = _sceneCharacter.transform.position;
                _baseRot = _sceneCharacter.transform.rotation;

                var pos = _sceneCharacter.transform.TransformPoint(phase.tOffset);
                var rot = _sceneCharacter.transform.rotation * Quaternion.Euler(phase.rOffset);

                _sceneCharacter.transform.position = pos;
                _sceneCharacter.transform.rotation = rot;
            }
            
            var tDelta = _sceneCharacter.transform.position - _basePos;
            var tLocalDelta = Quaternion.Inverse(_baseRot) * tDelta;

            var rLocalDelta = Quaternion.Inverse(_baseRot) * _sceneCharacter.transform.rotation;

            phase.tOffset = tLocalDelta;
            phase.rOffset = rLocalDelta.eulerAngles;

            _motionWarpingAsset.warpPhases[_phaseSlider] = phase;
        }
    }
}