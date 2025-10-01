using System.Collections;
using UnityEngine;

namespace UnityEditor.Tanks
{
    public static class EditorExtentions
    {

        public static void GenerateEditorArray( this SerializedObject serializedObject, string variableName) // only for public fields
        {
            serializedObject.Update();
            SerializedProperty stringsProperty = serializedObject.FindProperty(variableName);
            EditorGUILayout.PropertyField(stringsProperty, true);
            serializedObject.ApplyModifiedProperties();
        }
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent<T>(out var outComponent)
                ? outComponent
                : gameObject.AddComponent<T>();
        }
    }
}
