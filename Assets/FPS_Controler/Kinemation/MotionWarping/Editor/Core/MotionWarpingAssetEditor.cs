// Designed by KINEMATION, 2024.

using KINEMATION.MotionWarping.Editor.Widgets;
using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Utility;

using System;
using System.Reflection;

using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace KINEMATION.MotionWarping.Editor.Core
{
    [CustomEditor(typeof(MotionWarpingAsset))]
    public class MotionWarpingAssetEditor : UnityEditor.Editor
    {
        private const BindingFlags PrivateFieldBindingFlags =
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;

        private const BindingFlags PublicFieldBindingFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField;

        private const BindingFlags PublicPropertyBindingFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty;

        private const BindingFlags PublicMethodBindingFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
        
        private static Type _animationClipEditorType;
        private static Type _avatarPreviewType;
        private static Type _timeControlType;
        
        private MotionWarpingAsset _asset;
        private UnityEditor.Editor _meshEditor;

        private float _frameSlider = 0f;
        private float _length = 0f;
        private bool _manualPlaybackOverride;
        
        private WarpWindowWidget _warpWindowWidget;
        private WarpPosePreviewWidget _posePreviewer;
        
        private string[] _toolbarOptions = new string[] { "Motion Warping", "Pose Preview", "Animation Settings"};
        private int _selectedTab = 0;
        
        public override bool HasPreviewGUI() => true;

        private void OnAreaModified(int areaIndex)
        {
            float warpLength = _asset.GetLength();

            var phase = _asset.warpPhases[areaIndex];
            var size = _warpWindowWidget.GetAreaSize(areaIndex);
            
            phase.startTime = (float) Math.Round(size.Item1 * warpLength, 4);
            phase.endTime = (float) Math.Round(size.Item2 * warpLength, 4);
            _asset.warpPhases[areaIndex] = phase;
        }

        private void StopLoop()
        {
            var avatarPreview = _animationClipEditorType.GetField("m_AvatarPreview", PrivateFieldBindingFlags)?.GetValue(_meshEditor);
            if (avatarPreview == null) return;
 
            var timeControl = _avatarPreviewType.GetField("timeControl", PublicFieldBindingFlags)?.GetValue(avatarPreview);
            if (timeControl == null) return;
            
            var stopTime = _timeControlType.GetProperty("playing", PublicPropertyBindingFlags);
            if (stopTime == null) return;
            
            stopTime.SetValue(timeControl, false);
        }

        private void UpdatePlayback()
        {
            if (!_manualPlaybackOverride) return;

            var avatarPreview = _animationClipEditorType.GetField("m_AvatarPreview", PrivateFieldBindingFlags)
                ?.GetValue(_meshEditor);
            if (avatarPreview == null) return;

            var timeControl = _avatarPreviewType.GetField("timeControl", PublicFieldBindingFlags)
                ?.GetValue(avatarPreview);
            if (timeControl == null) return;

            var stopTime = _timeControlType.GetField("stopTime", PublicFieldBindingFlags);
            if (stopTime == null) return;

            stopTime.SetValue(timeControl, _length);

            var timeProperty = _timeControlType.GetField("currentTime", PublicFieldBindingFlags);
            if (timeProperty == null) return;

            timeProperty.SetValue(timeControl, _frameSlider);
        }

        private void GenerateAreas()
        {
            float totalTime = _asset.GetLength();

            if (Mathf.Approximately(totalTime, 0f)) return;
            _warpWindowWidget.ClearPhases();
            
            foreach (var phase in _asset.warpPhases)
            {
                _warpWindowWidget.AddWarpPhase(phase.startTime / totalTime, phase.endTime / totalTime);
            }
        }
        
        private void GeneratePhases()
        {
            float totalTime = _asset.GetLength();
            if (Mathf.Approximately(totalTime, 0f)) return;
            
            _asset.warpPhases.Clear();

            float timeStep = totalTime / _asset.phasesAmount;

            for (int i = 0; i < _asset.phasesAmount; i++)
            {
                WarpPhase phase = new WarpPhase()
                {
                    minRate = 0f,
                    maxRate = 1f,
                    startTime = timeStep * i,
                    endTime = timeStep * i + timeStep,
                };
                
                _asset.warpPhases.Add(phase);
            }
        }

        private void ExtractCurves()
        {
            if (_asset == null || _asset.animation == null)
            {
                Debug.LogError("WarpingAsset or AnimationClip is null!");
                return;
            }
            
            EditorCurveBinding[] tBindings = new EditorCurveBinding[3];

            var curveBindings = AnimationUtility.GetCurveBindings(_asset.animation);
            foreach (var binding in curveBindings)
            {
                if (_asset.animation.isHumanMotion)
                {
                    if (binding.propertyName.ToLower().Contains("roott.x"))
                    {
                        tBindings[0] = binding;
                    }
                    else if (binding.propertyName.ToLower().Contains("roott.y"))
                    {
                        tBindings[1] = binding;
                    }
                    else if (binding.propertyName.ToLower().Contains("roott.z"))
                    {
                        tBindings[2] = binding;
                    }
                }
                else
                {
                    if(!binding.path.ToLower().EndsWith("root")) continue;
                    
                    if (binding.propertyName.ToLower().Contains("localposition.x"))
                    {
                        tBindings[0] = binding;
                    }
                    else if (binding.propertyName.ToLower().Contains("localposition.y"))
                    {
                        tBindings[1] = binding;
                    }
                    else if (binding.propertyName.ToLower().Contains("localposition.z"))
                    {
                        tBindings[2] = binding;
                    }
                }
            }

            var curves = WarpingEditorUtility.ValidateCurves(_asset, 60f, tBindings);
            
            _asset.rootX = curves.X;
            _asset.rootY = curves.Y;
            _asset.rootZ = curves.Z;

            ComputeTotalRootMotion();
        }

        private void RenderMotionWarpingTab()
        {
            base.OnInspectorGUI();

            if (_asset.animation == null)
            {
                EditorGUILayout.HelpBox("Specify the animation", MessageType.Warning);
                return;
            }
            
            _length = _asset.animation.length;
            
            _warpWindowWidget.Render();
            
            var prevSlider = _frameSlider;
            _frameSlider = _length * _warpWindowWidget.GetPlayback();
           
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate Phases"))
            {
                GeneratePhases();
                GenerateAreas();
            }
            
            if (GUILayout.Button("Extract Curves"))
            {
                ExtractCurves();
            }

            EditorGUILayout.EndHorizontal();

            if (!Mathf.Approximately(prevSlider, _frameSlider) && !_manualPlaybackOverride)
            {
                _manualPlaybackOverride = true;
                StopLoop();
            }
            else
            {
                _manualPlaybackOverride = false;
            }
            
            UpdatePlayback();
        }

        private void RenderAnimationSettingsTab()
        {
            if (_meshEditor == null) return;
            _meshEditor.OnInspectorGUI();
        }

        private void ComputeTotalRootMotion()
        {
            if (_asset.animation == null) return;

            float sampleRate = _asset.animation.frameRate;
            if (Mathf.Approximately(sampleRate, 0f)) return;

            for (int i = 0; i < _asset.warpPhases.Count; i++)
            {
                var phase = _asset.warpPhases[i];
                
                float playback = phase.startTime;
                Vector3 lastValue = _asset.GetVectorValue(playback);
                
                phase.totalRootMotion = Vector3.zero;
               
                while (playback <= phase.endTime)
                {
                    // Accumulate the delta.
                    Vector3 value = _asset.GetVectorValue(playback);
                    Vector3 delta = value - lastValue;
                    
                    phase.totalRootMotion.x += Mathf.Abs(delta.x);
                    phase.totalRootMotion.y += Mathf.Abs(delta.y);
                    phase.totalRootMotion.z += Mathf.Abs(delta.z);
                    
                    lastValue = value;
                    
                    playback += 1f / sampleRate;
                }

                _asset.warpPhases[i] = phase;
            }
            
            EditorUtility.SetDirty(_asset);
            AssetDatabase.SaveAssets();
        }
        
        private void OnEnable()
        {
            _asset = (target) as MotionWarpingAsset;

            if (_asset == null)
            {
                Debug.LogError("Target MotionWarpingAsset is null!");
                return;
            }
            
            _meshEditor = CreateEditor(_asset.animation);
            
            _animationClipEditorType = Type.GetType("UnityEditor.AnimationClipEditor,UnityEditor");
            _avatarPreviewType = Type.GetType("UnityEditor.AvatarPreview,UnityEditor");
            _timeControlType = Type.GetType("UnityEditor.TimeControl,UnityEditor");
            
            _warpWindowWidget = new WarpWindowWidget();
            _warpWindowWidget.OnAreaModified += OnAreaModified;

            _posePreviewer = new WarpPosePreviewWidget(_asset);
            
            GenerateAreas();
        }

        private void OnDisable()
        {
            ComputeTotalRootMotion();
            
            _posePreviewer.RestorePose();
            _meshEditor = null;
        }

        public override void OnInspectorGUI()
        {
            _selectedTab = GUILayout.Toolbar(_selectedTab, _toolbarOptions);

            if (_selectedTab == 0)
            {
                RenderMotionWarpingTab();
                return;
            }

            if (_selectedTab == 1)
            {
                _posePreviewer?.Render();
                return;
            }
            
            RenderAnimationSettingsTab();
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (_meshEditor == null || !_meshEditor.HasPreviewGUI()) return;
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            _meshEditor.OnPreviewSettings();
            EditorGUILayout.EndHorizontal();
            
            _meshEditor.OnInteractivePreviewGUI(r, GUIStyle.none);
        }
    }
}