using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Turret_Base_CS))]
	public class Turret_Base_CSEditor : Editor
	{

		SerializedProperty Part_MeshProp;

		SerializedProperty Colliders_NumProp;
		SerializedProperty Colliders_MeshProp;
		SerializedProperty Collider_MeshProp; // for old versions.
		SerializedProperty Sub_Collider_MeshProp; // for old versions.

		SerializedProperty Materials_NumProp;
		SerializedProperty MaterialsProp;
		SerializedProperty Part_MaterialProp; // for old versions.

		SerializedProperty Offset_XProp;
		SerializedProperty Offset_YProp;
		SerializedProperty Offset_ZProp;

		SerializedProperty Use_Damage_ControlProp;
		SerializedProperty Turret_IndexProp;

        SerializedProperty Has_ChangedProp;

        Transform thisTransform;


		void  OnEnable ()
		{
			Part_MeshProp = serializedObject.FindProperty ("Part_Mesh");

			Colliders_NumProp = serializedObject.FindProperty ("Colliders_Num");
			Colliders_MeshProp = serializedObject.FindProperty ("Colliders_Mesh");
			Collider_MeshProp = serializedObject.FindProperty ("Collider_Mesh"); // for old versions.
			Sub_Collider_MeshProp = serializedObject.FindProperty ("Sub_Collider_Mesh"); // for old versions.

			Materials_NumProp = serializedObject.FindProperty ("Materials_Num");
			MaterialsProp = serializedObject.FindProperty ("Materials");
			Part_MaterialProp = serializedObject.FindProperty ("Part_Material");

			Offset_XProp = serializedObject.FindProperty ("Offset_X");
			Offset_YProp = serializedObject.FindProperty ("Offset_Y");
			Offset_ZProp = serializedObject.FindProperty ("Offset_Z");
		
			Use_Damage_ControlProp = serializedObject.FindProperty ("Use_Damage_Control");
			Turret_IndexProp = serializedObject.FindProperty ("Turret_Index");

            Has_ChangedProp = serializedObject.FindProperty("Has_Changed");

            if (Selection.activeGameObject)
            {
				thisTransform = Selection.activeGameObject.transform;
			}
		}


		public override void OnInspectorGUI ()
		{
			Set_Inspector();
		}


		void Set_Inspector ()
		{
            // Check the Prefab Mode.
            if (PrefabUtility.IsPartOfPrefabInstance(thisTransform))
            {
                GUI.backgroundColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);
                EditorGUILayout.HelpBox("\n'Turret can be modified only in the Prefab Mode.\nPlease go to the Prefab Mode, or Unpack the prefab.\n", MessageType.Warning, true);
                return;
            }

            serializedObject.Update();

            GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Fold out above 'Transform' window when you move this object.", MessageType.Warning, true);

			// Mesh settings
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Mesh settings", MessageType.None, true);
			Part_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ("Mesh", Part_MeshProp.objectReferenceValue, typeof(Mesh), false);
			EditorGUILayout.Space ();
			EditorGUILayout.IntSlider (Materials_NumProp, 1, 10, "Number of Materials");
			MaterialsProp.arraySize = Materials_NumProp.intValue;
			if (Materials_NumProp.intValue == 1 && Part_MaterialProp.objectReferenceValue != null) {
				if (MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue == null) {
					MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue = Part_MaterialProp.objectReferenceValue;
				}
				Part_MaterialProp.objectReferenceValue = null;
			}
			EditorGUI.indentLevel++;
			for (int i = 0; i < Materials_NumProp.intValue; i++) {
				MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue = EditorGUILayout.ObjectField ("Material " + "(" + i + ")", MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue, typeof(Material), false);
			}
			EditorGUI.indentLevel--;

			// Position settings
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Position settings", MessageType.None, true);
			EditorGUILayout.Slider (Offset_XProp, -5.0f, 5.0f, "Offset X");
			EditorGUILayout.Slider (Offset_YProp, -5.0f, 5.0f, "Offset Y");
			EditorGUILayout.Slider (Offset_ZProp, -10.0f, 10.0f, "Offset Z");

			// Collider settings
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Collider settings", MessageType.None, true);
			// for old versions.
			if (Collider_MeshProp.objectReferenceValue || Sub_Collider_MeshProp.objectReferenceValue) {
				Colliders_NumProp.intValue = 2;
				Colliders_MeshProp.arraySize = Colliders_NumProp.intValue;
				Colliders_MeshProp.GetArrayElementAtIndex(0).objectReferenceValue = Collider_MeshProp.objectReferenceValue as Mesh;
				Colliders_MeshProp.GetArrayElementAtIndex(1).objectReferenceValue = Sub_Collider_MeshProp.objectReferenceValue as Mesh;
				Collider_MeshProp.objectReferenceValue = null;
				Sub_Collider_MeshProp.objectReferenceValue = null;
			}
			// for new version.
			EditorGUILayout.IntSlider (Colliders_NumProp, 0, 10, "Number of Colliders");
			Colliders_MeshProp.arraySize = Colliders_NumProp.intValue;
			EditorGUI.indentLevel++;
			for (int i = 0; i < Colliders_NumProp.intValue; i++) {
				Colliders_MeshProp.GetArrayElementAtIndex (i).objectReferenceValue = EditorGUILayout.ObjectField ("MeshCollider " + "(" + i + ")", Colliders_MeshProp.GetArrayElementAtIndex (i).objectReferenceValue, typeof(Mesh), false);
			}
			EditorGUI.indentLevel--;

			// Damage settings
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Damage settings", MessageType.None, true);
			Use_Damage_ControlProp.boolValue = EditorGUILayout.Toggle ("Use Damage Control", Use_Damage_ControlProp.boolValue);
			if (Use_Damage_ControlProp.boolValue) {
				EditorGUI.indentLevel++;
				EditorGUILayout.IntSlider (Turret_IndexProp, 0, 16, "Turret Index");
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

            // Update Value
            if (GUI.changed || GUILayout.Button("Update Values") || Event.current.commandName == "UndoRedoPerformed")
            {
                Has_ChangedProp.boolValue = !Has_ChangedProp.boolValue;
                Create();
            }

            // Call "Create()" while the object is moving.
            if (thisTransform.hasChanged)
            {
                thisTransform.hasChanged = false;
                Create();
            }

            EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			serializedObject.ApplyModifiedProperties ();
		}


		void Create()
		{
            thisTransform.gameObject.GetComponent<Turret_Base_CS>().Create();
        }
	}

}