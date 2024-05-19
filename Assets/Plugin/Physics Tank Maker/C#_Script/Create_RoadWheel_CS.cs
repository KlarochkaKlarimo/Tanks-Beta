using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{
	
	[ System.Serializable]
	public class RoadWheelsProp
	{
		public string parentName;
		public float baseRadius;
		public float [] angles;
	}


	public class Create_RoadWheel_CS : MonoBehaviour
	{
		/* 
		 * This script is attached to the "Create_RoadWheel" in the tank.
		 * This script is used for creating and setting the road wheels and the suspension arms by the editor script.
		*/

		public bool Fit_ST_Flag = false;
		public Vector3 wheelsInitialAngles;
		public float Sus_Distance = 2.06f;
		public int Num = 6;
		public float Spacing = 0.88f;
		public float Sus_Length = 0.5f;
		public bool Set_Individually = false;
		public float Sus_Angle = 0.0f;
		public float [] Sus_Angles;
		public float Sus_Anchor = 0.0f;
		public float Sus_Mass = 30.0f;
		public float Sus_Spring = 900.0f;
		public float Sus_Damper = 20.0f;
		public float Sus_Target = 30.0f;
		public float Sus_Forward_Limit = 30.0f;
		public float Sus_Backward_Limit = 30.0f;
		public Mesh Sus_L_Mesh;
		public Mesh Sus_R_Mesh;
		public int Sus_Materials_Num = 1;
		public Material[] Sus_Materials;
		public Material Sus_L_Material; // for old versions.
		public Material Sus_R_Material; // for old versions.
		public float Reinforce_Radius = 0.5f;

		public float Wheel_Distance = 2.7f;
		public float Wheel_Mass = 30.0f;
		public float Wheel_Radius = 0.3f;
		public PhysicMaterial Collider_Material;
		public Mesh Wheel_Mesh;
		public int Wheel_Materials_Num = 1;
		public Material[] Wheel_Materials;
		public Material Wheel_Material; // for old versions.

		public bool Drive_Wheel = true;
		public bool Use_BrakeTurn = true;
		public bool Wheel_Resize = false;
		public float ScaleDown_Size = 0.5f;
		public float Return_Speed = 0.05f;

        // For editor script.
        public bool Has_Changed;


        public RoadWheelsProp Get_Current_Angles()
        { // Called from "Create_TrackBelt_CSEditor" while converting the 'Physics Track' into 'Static Track'.
            RoadWheelsProp currentProp = new RoadWheelsProp();
            currentProp.parentName = gameObject.name;
            currentProp.baseRadius = Wheel_Radius;
            currentProp.angles = new float[Num];
            for (int i = 0; i < Num; i++)
            {
                Transform susTransform = transform.Find("Suspension_L_" + (i + 1));
                if (susTransform)
                {
                    float currentAngle = susTransform.localEulerAngles.y;
                    currentAngle = Mathf.DeltaAngle(0.0f, currentAngle);
                    currentProp.angles[i] = currentAngle;
                }
            }
            return currentProp;
        }

		public void Create()
		{
			// Delete Objects
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				DestroyImmediate(transform.GetChild(0).gameObject);
			}

			// Set "Drive_Wheel_Parent_CS" script in this object.
			Set_Drive_Wheel_Parent_Script();

			// Create Suspension
			for (int i = 0; i < Num; i++)
			{
				Create_Suspension_Arm("L", i + 1);
			}
			for (int i = 0; i < Num; i++)
			{
				Create_Suspension_Arm("R", i + 1);
			}

			// Create Wheel
			for (int i = 0; i < Num; i++)
			{
				Create_Wheel("L", i + 1);
			}
			for (int i = 0; i < Num; i++)
			{
				Create_Wheel("R", i + 1);
			}
		}
		void Set_Drive_Wheel_Parent_Script()
		{
			// Set "Drive_Wheel_Parent_CS" in this object.
			Drive_Wheel_Parent_CS driveWheelParentScript = transform.GetComponent<Drive_Wheel_Parent_CS>();
			if (driveWheelParentScript == null)
			{
				driveWheelParentScript = transform.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
			}
			driveWheelParentScript.Drive_Flag = Drive_Wheel;
			driveWheelParentScript.Radius = Wheel_Radius;
			driveWheelParentScript.Use_BrakeTurn = true;
		}


		void Create_Suspension_Arm(string direction, int number)
		{
			// Create gameobject & Set parent
			GameObject armObject = new GameObject("Suspension_" + direction + "_" + number);
			armObject.transform.parent = transform;
			// Set position.
			Vector3 pos;
			pos.x = 0.0f;
			pos.z = -Spacing * (number - 1);
			pos.y = Sus_Distance / 2.0f;
			if (direction == "R")
			{
				pos.y *= -1.0f;
			}
			armObject.transform.localPosition = pos;
			// Set rotation.
			if (Set_Individually)
			{
				armObject.transform.localRotation = Quaternion.Euler(0.0f, Sus_Angles[number - 1], -90.0f);
			}
			else
			{
				armObject.transform.localRotation = Quaternion.Euler(0.0f, Sus_Angle, -90.0f);
			}
			// Mesh
			if (direction == "L")
			{ // Left
				if (Sus_L_Mesh)
				{
					MeshFilter meshFilter;
					meshFilter = armObject.AddComponent<MeshFilter>();
					meshFilter.mesh = Sus_L_Mesh;
				}
			}
			else
			{ // Right
				if (Sus_R_Mesh)
				{
					MeshFilter meshFilter;
					meshFilter = armObject.AddComponent<MeshFilter>();
					meshFilter.mesh = Sus_R_Mesh;
				}
			}
			MeshRenderer meshRenderer = armObject.AddComponent<MeshRenderer>();
			Material[] materials = new Material[Sus_Materials_Num];
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i] = Sus_Materials[i];
			}
			meshRenderer.materials = materials;
			// Rigidbody
			Rigidbody rigidbody = armObject.AddComponent<Rigidbody>();
			rigidbody.mass = Sus_Mass;
			// HingeJoint
			HingeJoint hingeJoint = armObject.AddComponent<HingeJoint>();
			hingeJoint.connectedBody = transform.parent.gameObject.GetComponent<Rigidbody>(); //MainBody's Rigidbody.
			hingeJoint.anchor = new Vector3(0.0f, 0.0f, Sus_Anchor);
			hingeJoint.axis = new Vector3(1.0f, 0.0f, 0.0f);
			hingeJoint.useSpring = true;
			JointSpring jointSpring = hingeJoint.spring;
			jointSpring.spring = Sus_Spring;
			jointSpring.damper = Sus_Damper;
			if (Set_Individually)
			{
				jointSpring.targetPosition = Sus_Target + Sus_Angles[number - 1];
			}
			else
			{
				jointSpring.targetPosition = Sus_Target + Sus_Angle;
			}
			hingeJoint.spring = jointSpring;
			hingeJoint.useLimits = true;
			JointLimits jointLimits = hingeJoint.limits;
			if (Set_Individually)
			{
				jointLimits.max = Sus_Forward_Limit + Sus_Angles[number - 1];
				jointLimits.min = -(Sus_Backward_Limit - Sus_Angles[number - 1]);
			}
			else
			{
				jointLimits.max = Sus_Forward_Limit + Sus_Angle;
				jointLimits.min = -(Sus_Backward_Limit - Sus_Angle);
			}
			hingeJoint.limits = jointLimits;
			// Reinforce SphereCollider
			SphereCollider sphereCollider = armObject.AddComponent<SphereCollider>();
			sphereCollider.radius = Reinforce_Radius;
			// Set Layer
			armObject.layer = Layer_Settings_CS.Reinforce_Layer;
		}


		void Create_Wheel(string direction, int number)
		{
			// Create gameobject & Set parent.
			GameObject wheelObject = new GameObject("RoadWheel_" + direction + "_" + number);
			wheelObject.transform.parent = transform;
			// Set position.
			Vector3 pos;
			if (Set_Individually)
			{
				pos.x = Mathf.Sin(Mathf.Deg2Rad * (180.0f + Sus_Angles[number - 1])) * Sus_Length;
				pos.z = Mathf.Cos(Mathf.Deg2Rad * (180.0f + Sus_Angles[number - 1])) * Sus_Length;
			}
			else
			{
				pos.x = Mathf.Sin(Mathf.Deg2Rad * (180.0f + Sus_Angle)) * Sus_Length;
				pos.z = Mathf.Cos(Mathf.Deg2Rad * (180.0f + Sus_Angle)) * Sus_Length;
			}
			pos.z -= Spacing * (number -1);
			pos.y = Wheel_Distance / 2.0f;
			if (direction == "R")
			{
				pos.y *= -1.0f;
			}
			wheelObject.transform.localPosition = pos;
			// Set rotation.
			if (direction == "L")
			{ // Left
				wheelObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
			else
			{ // Right
				wheelObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180);
			}
			// Mesh
			if (Wheel_Mesh)
			{
				MeshFilter meshFilter = wheelObject.AddComponent<MeshFilter>();
				meshFilter.mesh = Wheel_Mesh;
				MeshRenderer meshRenderer = wheelObject.AddComponent<MeshRenderer>();
				Material[] materials = new Material[Wheel_Materials_Num];
				for (int i = 0; i < materials.Length; i++)
				{
					materials[i] = Wheel_Materials[i];
				}
				meshRenderer.materials = materials;
			}
			// Rigidbody
			Rigidbody rigidbody = wheelObject.AddComponent<Rigidbody>();
			rigidbody.mass = Wheel_Mass;
			// HingeJoint
			HingeJoint hingeJoint = wheelObject.AddComponent<HingeJoint>();
			hingeJoint.anchor = Vector3.zero;
			hingeJoint.axis = new Vector3(0.0f, 1.0f, 0.0f);
			hingeJoint.connectedBody = transform.Find("Suspension_" + direction + "_" + number).gameObject.GetComponent<Rigidbody>();
			// SphereCollider
			SphereCollider sphereCollider = wheelObject.AddComponent<SphereCollider>();
			sphereCollider.radius = Wheel_Radius;
			sphereCollider.center = Vector3.zero;
			sphereCollider.material = Collider_Material;
			// Drive_Wheel_CS
			Drive_Wheel_CS driveScript = wheelObject.AddComponent<Drive_Wheel_CS>();
			driveScript.This_Rigidbody = rigidbody;
			driveScript.Is_Left = (direction == "L");
			driveScript.Parent_Script = transform.GetComponent<Drive_Wheel_Parent_CS>();
			// Fix_Shaking_Rotation_CS
			if (Fit_ST_Flag)
			{ // for Physics Tracks
				Fix_Shaking_Rotation_CS fixScript = wheelObject.AddComponent<Fix_Shaking_Rotation_CS>();
				fixScript.Is_Left = (direction == "L");
				fixScript.This_Transform = wheelObject.transform;
			}
			// Wheel_Resize_CS
			if (Fit_ST_Flag)
			{ // for Physics Tracks
				if (Wheel_Resize)
				{
					Wheel_Resize_CS resizeScript = wheelObject.AddComponent<Wheel_Resize_CS>();
					resizeScript.ScaleDown_Size = ScaleDown_Size;
					resizeScript.Return_Speed = Return_Speed;
				}
			}
			// Stabilizer_CS
			Stabilizer_CS stabilizerScript = wheelObject.AddComponent<Stabilizer_CS>();
			stabilizerScript.This_Transform = wheelObject.transform;
			stabilizerScript.Is_Left = (direction == "L");
			stabilizerScript.Initial_Pos_Y = wheelObject.transform.localPosition.y;
			stabilizerScript.Initial_Angles = wheelsInitialAngles;// wheelObject.transform.localEulerAngles;
            // Set Layer
            wheelObject.layer = Layer_Settings_CS.Wheels_Layer;

		}
	}

}