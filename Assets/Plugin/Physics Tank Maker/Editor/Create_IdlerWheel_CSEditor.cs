﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	[ CustomEditor (typeof(Create_IdlerWheel_CS))]
	public class Create_IdlerWheel_CSEditor : Editor
	{
		SerializedProperty Static_FlagProp;
		SerializedProperty Radius_OffsetProp;

		SerializedProperty Arm_FlagProp;
		SerializedProperty Arm_DistanceProp;
		SerializedProperty Arm_LengthProp;
		SerializedProperty Arm_AngleProp;
		SerializedProperty Arm_L_MeshProp;
		SerializedProperty Arm_R_MeshProp;
		SerializedProperty Arm_Materials_NumProp;
		SerializedProperty Arm_MaterialsProp;
		SerializedProperty Arm_L_MaterialProp; // for old versions.
		SerializedProperty Arm_R_MaterialProp; // for old versions.

		SerializedProperty Wheel_DistanceProp;
		SerializedProperty Wheel_MassProp;
		SerializedProperty Wheel_RadiusProp;
		SerializedProperty Collider_MaterialProp;
		SerializedProperty Wheel_MeshProp;
		SerializedProperty Wheel_Materials_NumProp;
		SerializedProperty Wheel_MaterialsProp;
		SerializedProperty Wheel_MaterialProp; // for old versions.
		SerializedProperty Drive_WheelProp;
		SerializedProperty Wheel_ResizeProp;
		SerializedProperty ScaleDown_SizeProp;
		SerializedProperty Return_SpeedProp;

        SerializedProperty Has_ChangedProp;

        Transform thisTransform;


		void OnEnable ()
		{
			Static_FlagProp = serializedObject.FindProperty ("Static_Flag");
			Radius_OffsetProp = serializedObject.FindProperty ("Radius_Offset");

			Arm_FlagProp = serializedObject.FindProperty ("Arm_Flag");
			Arm_DistanceProp = serializedObject.FindProperty ("Arm_Distance");
			Arm_LengthProp = serializedObject.FindProperty ("Arm_Length");
			Arm_AngleProp = serializedObject.FindProperty ("Arm_Angle");
			Arm_L_MeshProp = serializedObject.FindProperty ("Arm_L_Mesh");
			Arm_R_MeshProp = serializedObject.FindProperty ("Arm_R_Mesh");
			Arm_Materials_NumProp = serializedObject.FindProperty ("Arm_Materials_Num");
			Arm_MaterialsProp = serializedObject.FindProperty ("Arm_Materials");
			Arm_L_MaterialProp = serializedObject.FindProperty ("Arm_L_Material");
			Arm_R_MaterialProp = serializedObject.FindProperty ("Arm_R_Material");
		
			Wheel_DistanceProp = serializedObject.FindProperty ("Wheel_Distance");
			Wheel_MassProp = serializedObject.FindProperty ("Wheel_Mass");
			Wheel_RadiusProp = serializedObject.FindProperty ("Wheel_Radius");
			Collider_MaterialProp = serializedObject.FindProperty ("Collider_Material");
			Wheel_MeshProp = serializedObject.FindProperty ("Wheel_Mesh");
			Wheel_Materials_NumProp = serializedObject.FindProperty ("Wheel_Materials_Num");
			Wheel_MaterialsProp = serializedObject.FindProperty ("Wheel_Materials");
			Wheel_MaterialProp = serializedObject.FindProperty ("Wheel_Material");
			Drive_WheelProp = serializedObject.FindProperty ("Drive_Wheel");
			Wheel_ResizeProp = serializedObject.FindProperty ("Wheel_Resize");
			ScaleDown_SizeProp = serializedObject.FindProperty ("ScaleDown_Size");
			Return_SpeedProp = serializedObject.FindProperty ("Return_Speed");

            Has_ChangedProp = serializedObject.FindProperty("Has_Changed");

            if (Selection.activeGameObject)
            {
				thisTransform = Selection.activeGameObject.transform;
			}
		}


		public override void OnInspectorGUI()
		{
			bool isPrepared;
			if (Application.isPlaying || thisTransform.parent == null || thisTransform.parent.gameObject.GetComponent<Rigidbody>() == null)
            {
				isPrepared = false;
			}
            else
            {
				isPrepared = true;
			}
		
			if (isPrepared)
            {
                // Keep rotation.
                Vector3 localAngles = thisTransform.localEulerAngles;
                localAngles.z = 90.0f;
                thisTransform.localEulerAngles = localAngles;

                // Set Inspector window.
                Set_Inspector();
			}
		}


		void Set_Inspector ()
		{
            // Check the Prefab Mode.
            if (PrefabUtility.IsPartOfPrefabInstance(thisTransform))
            {
                GUI.backgroundColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);
                EditorGUILayout.HelpBox("\n'The wheels can be modified only in the Prefab Mode.\nPlease go to the Prefab Mode, or Unpack the prefab.\n", MessageType.Warning, true);
                return;
            }

            serializedObject.Update ();

			GUI.backgroundColor = new Color(1.0f, 1.0f, 0.5f, 1.0f);

			// for Static Wheel
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Wheel Type", MessageType.None, true);
			Static_FlagProp.boolValue = EditorGUILayout.Toggle ("Static Wheel", Static_FlagProp.boolValue);
			if (Static_FlagProp.boolValue) {
				EditorGUILayout.Slider (Radius_OffsetProp, -0.5f, 0.5f, "Radius Offset");
				EditorGUILayout.Space ();
				if (GUILayout.Button ("Update Values", GUILayout.Width (200))) {
					Create ();
				}
			}

			// Tensioner Arms settings
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Tensioner Arms settings", MessageType.None, true);
			Arm_FlagProp.boolValue = EditorGUILayout.Toggle ("Use Tensioner Arm", Arm_FlagProp.boolValue);
			if (Arm_FlagProp.boolValue) {
				EditorGUILayout.Slider (Arm_DistanceProp, 0.1f, 10.0f, "Arm Distance");
				EditorGUILayout.Slider (Arm_LengthProp, -1.0f, 1.0f, "Length");
				EditorGUILayout.Slider (Arm_AngleProp, -180.0f, 180.0f, "Angle");
				Arm_L_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ("Left Arm Mesh", Arm_L_MeshProp.objectReferenceValue, typeof(Mesh), false);
				Arm_R_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ("Right Arm Mesh", Arm_R_MeshProp.objectReferenceValue, typeof(Mesh), false);

				EditorGUILayout.IntSlider (Arm_Materials_NumProp, 1, 10, "Number of Materials");
				Arm_MaterialsProp.arraySize = Arm_Materials_NumProp.intValue;
				if (Arm_Materials_NumProp.intValue == 1 && Arm_L_MaterialProp.objectReferenceValue != null) {
					if (Arm_MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue == null) {
						Arm_MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue = Arm_L_MaterialProp.objectReferenceValue;
					}
					Arm_L_MaterialProp.objectReferenceValue = null;
					Arm_R_MaterialProp.objectReferenceValue = null;
				}
				EditorGUI.indentLevel++;
				for (int i = 0; i < Arm_Materials_NumProp.intValue; i++) {
					Arm_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue = EditorGUILayout.ObjectField ("Material " + "(" + i + ")", Arm_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue, typeof(Material), false);
				}
				EditorGUI.indentLevel--;
			}

			// Wheels settings
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.HelpBox ("Wheels settings", MessageType.None, true);
			EditorGUILayout.Slider (Wheel_DistanceProp, 0.1f, 10.0f, "Wheel Distance");
			EditorGUILayout.Slider (Wheel_MassProp, 0.1f, 300.0f, "Mass");
			EditorGUILayout.Space ();
			GUI.backgroundColor = new Color (1.0f, 0.5f, 0.5f, 1.0f);
			EditorGUILayout.Slider (Wheel_RadiusProp, 0.01f, 1.0f, "Wheel Collider Radius");
			GUI.backgroundColor = new Color (1.0f, 1.0f, 0.5f, 1.0f);
			Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ("Physic Material", Collider_MaterialProp.objectReferenceValue, typeof(PhysicMaterial), false);
			EditorGUILayout.Space ();
			Wheel_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ("Wheel Mesh", Wheel_MeshProp.objectReferenceValue, typeof(Mesh), false);

			EditorGUILayout.IntSlider (Wheel_Materials_NumProp, 1, 10, "Number of Materials");
			Wheel_MaterialsProp.arraySize = Wheel_Materials_NumProp.intValue;
			if (Wheel_Materials_NumProp.intValue == 1 && Wheel_MaterialProp.objectReferenceValue != null) {
				if (Wheel_MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue == null) {
					Wheel_MaterialsProp.GetArrayElementAtIndex (0).objectReferenceValue = Wheel_MaterialProp.objectReferenceValue;
				}
				Wheel_MaterialProp.objectReferenceValue = null;
			}
			EditorGUI.indentLevel++;
			for (int i = 0; i < Wheel_Materials_NumProp.intValue; i++) {
				Wheel_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue = EditorGUILayout.ObjectField ("Material " + "(" + i + ")", Wheel_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue, typeof(Material), false);
			}
			EditorGUI.indentLevel--;

			if (Static_FlagProp.boolValue == false) { // Physics Wheel
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				// Scripts settings
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.HelpBox ("Scripts settings", MessageType.None, true);
				// Drive Wheel
				Drive_WheelProp.boolValue = EditorGUILayout.Toggle ("Drive Wheel", Drive_WheelProp.boolValue);
				EditorGUILayout.Space ();
				// Wheel Resize
				Wheel_ResizeProp.boolValue = EditorGUILayout.Toggle ("Wheel Resize Script", Wheel_ResizeProp.boolValue);
				if (Wheel_ResizeProp.boolValue) {
					EditorGUILayout.Slider (ScaleDown_SizeProp, 0.1f, 3.0f, "Scale Size");
					EditorGUILayout.Slider (Return_SpeedProp, 0.01f, 0.1f, "Return Speed");
				}
				EditorGUILayout.Space ();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

            // Update Value
            if (GUI.changed || GUILayout.Button("Update Values") || Event.current.commandName == "UndoRedoPerformed")
            {
                Has_ChangedProp.boolValue = !Has_ChangedProp.boolValue;
                Create();
            }

            EditorGUILayout.Space();
			EditorGUILayout.Space();

			serializedObject.ApplyModifiedProperties ();
		}


        public void Create()
        {
            // Delete Objects
            int childCount = thisTransform.childCount;
			for (int i = 0; i < childCount; i++) {
				DestroyImmediate (thisTransform.GetChild (0).gameObject);
			}
			// Create Arm and Wheel
			Vector3 pos;
			if (Arm_FlagProp.boolValue) { //With Arm
				// Create Arms.
				Create_Arm ("L", new Vector3 (0.0f, Arm_DistanceProp.floatValue / 2.0f, 0.0f));
				Create_Arm ("R", new Vector3 (0.0f, -Arm_DistanceProp.floatValue / 2.0f, 0.0f));
				// Set Wheel Pos.
				pos.x = Mathf.Sin (Mathf.Deg2Rad * (180.0f + Arm_AngleProp.floatValue)) * Arm_LengthProp.floatValue;
				pos.y = Wheel_DistanceProp.floatValue / 2.0f;
				pos.z = Mathf.Cos (Mathf.Deg2Rad * (180.0f + Arm_AngleProp.floatValue)) * Arm_LengthProp.floatValue;
			} else { // No Arm
				// Set Wheel Pos.
				pos.x = 0.0f;
				pos.y = Wheel_DistanceProp.floatValue / 2.0f;
				pos.z = 0.0f;
			}
			// Create Wheels.
			Set_Drive_Wheel_Parent_Script();
			if (Static_FlagProp.boolValue == false) { //Drive Wheel.
				Remove_Static_Wheel_Parent_Script();
				Create_Physics_Wheel ("L", new Vector3 (pos.x, pos.y, pos.z));
				Create_Physics_Wheel ("R", new Vector3 (pos.x, -pos.y, pos.z));
			}
			else { //Static Wheel.
				Set_Static_Wheel_Parent_Script();
				Create_Static_Wheel ("L", new Vector3 (pos.x, pos.y, pos.z));
				Create_Static_Wheel ("R", new Vector3 (pos.x, -pos.y, pos.z));
				Create_Invisible_Wheel ("L", new Vector3 (pos.x, pos.y, pos.z));
				Create_Invisible_Wheel ("R", new Vector3 (pos.x, -pos.y, pos.z));
			}
		}


		void Create_Arm (string direction, Vector3 position)
		{
			//Create gameobject & Set transform
			GameObject armObject = new GameObject ("TensionerArm_" + direction);
			armObject.transform.parent = thisTransform;
			armObject.transform.localPosition = position;
			armObject.transform.localRotation = Quaternion.Euler (0.0f, Arm_AngleProp.floatValue, -90.0f);
			// Add Mesh
			if (direction == "L") { // Left
				if (Arm_L_MeshProp.objectReferenceValue) {
					MeshFilter meshFilter = armObject.AddComponent < MeshFilter > ();
					meshFilter.mesh = Arm_L_MeshProp.objectReferenceValue as Mesh;
				}
			} else { //Right
				if (Arm_R_MeshProp.objectReferenceValue) {
					MeshFilter meshFilter = armObject.AddComponent < MeshFilter > ();
					meshFilter.mesh = Arm_R_MeshProp.objectReferenceValue as Mesh;
				}
			}
			MeshRenderer meshRenderer = armObject.AddComponent < MeshRenderer > ();
			Material[] materials = new Material [ Arm_Materials_NumProp.intValue ];
			for (int i = 0; i < materials.Length; i++) {
				materials [i] = Arm_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue as Material;
			}
			meshRenderer.materials = materials;
		}


		void Create_Physics_Wheel (string direction, Vector3 position)
		{
			GameObject wheelObject = Create_GameObject ("IdlerWheel", direction, position);
			Add_Mesh (wheelObject);
			Add_SphereCollider (wheelObject);
			Add_DrivingComponents (wheelObject, direction);
			// Wheel_Resize_CS
			if (Wheel_ResizeProp.boolValue) {
				Wheel_Resize_CS resizeScript;
				resizeScript = wheelObject.AddComponent < Wheel_Resize_CS > ();
				resizeScript.ScaleDown_Size = ScaleDown_SizeProp.floatValue;
				resizeScript.Return_Speed = Return_SpeedProp.floatValue;
			}
			// Fix_Shaking_Rotation_CS
			Fix_Shaking_Rotation_CS fixScript = wheelObject.AddComponent <Fix_Shaking_Rotation_CS>();
			fixScript.Is_Left = (direction == "L");
			fixScript.This_Transform = wheelObject.transform;
			// Stabilizer_CS
			Stabilizer_CS stabilizerScript = wheelObject.AddComponent <Stabilizer_CS>();
			stabilizerScript.This_Transform = wheelObject.transform;
			stabilizerScript.Is_Left = (direction == "L");
			stabilizerScript.Initial_Pos_Y = wheelObject.transform.localPosition.y;
			stabilizerScript.Initial_Angles = wheelObject.transform.localEulerAngles;
		}


		void Set_Static_Wheel_Parent_Script()
		{
			// Set "Static_Wheel_Parent_CS" in this object.
			Static_Wheel_Parent_CS staticWheelParentScript = thisTransform.GetComponent <Static_Wheel_Parent_CS>();
			if (staticWheelParentScript == null) {
				staticWheelParentScript = thisTransform.gameObject.AddComponent <Static_Wheel_Parent_CS>();
			}
			Mesh wheelMesh = Wheel_MeshProp.objectReferenceValue as Mesh;
			float wheelRadius = wheelMesh.bounds.extents.x;
			staticWheelParentScript.Wheel_Radius = wheelRadius + Radius_OffsetProp.floatValue;
		}


		void Create_Static_Wheel (string direction, Vector3 position)
		{
			GameObject gameObject = Create_GameObject ("IdlerWheel", direction, position);
			Add_Mesh (gameObject);
			// Static_Wheel_CS script
			Static_Wheel_CS staticWheelScript = gameObject.AddComponent <Static_Wheel_CS>();
			staticWheelScript.Is_Left = (direction == "L");
			staticWheelScript.Parent_Script = thisTransform.GetComponent <Static_Wheel_Parent_CS>();
		}


		void Create_Invisible_Wheel (string direction, Vector3 position)
		{
			GameObject gameObject = Create_GameObject ("Invisible_IdlerWheel", direction, position);
			MeshFilter meshFilter = gameObject.AddComponent < MeshFilter > (); // Set only MeshFilter in order to get the mesh size.
			meshFilter.mesh = Wheel_MeshProp.objectReferenceValue as Mesh;
			Add_SphereCollider (gameObject);
			Add_DrivingComponents (gameObject, direction);
		}


		GameObject Create_GameObject (string name, string direction, Vector3 position)
		{
			GameObject gameObject = new GameObject (name + "_" + direction);
			gameObject.transform.parent = thisTransform;
			gameObject.transform.localPosition = position;
			if (direction == "L") {
				gameObject.transform.localRotation = Quaternion.Euler (Vector3.zero);
			} else {
				gameObject.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 180.0f);
			}
			gameObject.layer = Layer_Settings_CS.Wheels_Layer;
            return gameObject;
		}


		void Add_Mesh (GameObject gameObject)
		{
			MeshFilter meshFilter = gameObject.AddComponent < MeshFilter > ();
			meshFilter.mesh = Wheel_MeshProp.objectReferenceValue as Mesh;
			MeshRenderer meshRenderer = gameObject.AddComponent < MeshRenderer > ();
			Material[] materials = new Material [ Wheel_Materials_NumProp.intValue ];
			for (int i = 0; i < materials.Length; i++) {
				materials [i] = Wheel_MaterialsProp.GetArrayElementAtIndex (i).objectReferenceValue as Material;
			}
			meshRenderer.materials = materials;
		}


		void Add_SphereCollider (GameObject gameObject)
		{
			SphereCollider sphereCollider = gameObject.AddComponent < SphereCollider > ();
			sphereCollider.radius = Wheel_RadiusProp.floatValue;
			sphereCollider.center = Vector3.zero;
			sphereCollider.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial;

		}


		void Remove_Static_Wheel_Parent_Script()
		{
			// Remove "Static_Wheel_Parent_CS" in this object.
			Static_Wheel_Parent_CS staticWheelParentScript = thisTransform.GetComponent <Static_Wheel_Parent_CS>();
			if (staticWheelParentScript) {
                EditorApplication.delayCall += () => DestroyImmediate(staticWheelParentScript);
            }
        }


		void Set_Drive_Wheel_Parent_Script()
		{
			// Set "Drive_Wheel_Parent_CS" in this object.
			Drive_Wheel_Parent_CS driveWheelParentScript = thisTransform.GetComponent <Drive_Wheel_Parent_CS>();
			if (driveWheelParentScript == null) {
				driveWheelParentScript = thisTransform.gameObject.AddComponent <Drive_Wheel_Parent_CS>();
			}
			driveWheelParentScript.Drive_Flag = Drive_WheelProp.boolValue;
			driveWheelParentScript.Radius = Wheel_RadiusProp.floatValue;
			driveWheelParentScript.Use_BrakeTurn = true;
		}


		void Add_DrivingComponents (GameObject gameObject, string direction)
		{
			// Rigidbody
			Rigidbody rigidbody = gameObject.AddComponent <Rigidbody>();
			rigidbody.mass = Wheel_MassProp.floatValue;
			// HingeJoint
			HingeJoint hingeJoint;
			hingeJoint = gameObject.AddComponent <HingeJoint>();
			hingeJoint.anchor = Vector3.zero;
			hingeJoint.axis = new Vector3 (0.0f, 1.0f, 0.0f);
			hingeJoint.connectedBody = thisTransform.parent.gameObject.GetComponent <Rigidbody>();
			// Drive_Wheel_CS
			Drive_Wheel_CS driveScript = gameObject.AddComponent <Drive_Wheel_CS>();
			driveScript.This_Rigidbody = rigidbody;
			driveScript.Is_Left = (direction == "L");
			driveScript.Parent_Script = thisTransform.GetComponent <Drive_Wheel_Parent_CS>();
		}

	}

}