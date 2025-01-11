// Designed by KINEMATION, 2024.

using Kinemation.MotionWarping.Runtime.Utility;

using UnityEditor;
using UnityEngine;

namespace KINEMATION.MotionWarping.Editor.Core
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}