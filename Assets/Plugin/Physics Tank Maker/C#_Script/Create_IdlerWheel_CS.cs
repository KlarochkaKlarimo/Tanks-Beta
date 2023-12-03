using UnityEngine;
using System.Collections;
using UnityEditor;

namespace ChobiAssets.PTM
{

	public class Create_IdlerWheel_CS : MonoBehaviour
	{
		/* 
		 * This script is attached to the "Create_IdlerWheel" in the tank.
		 * This script is used for creating and setting the idler wheels by the editor script.
		*/

		public bool Static_Flag = false;
		public float Radius_Offset;
		public bool Invisible_Physics_Wheel = false;

		public bool Arm_Flag = false;
		public float Arm_Distance = 2.11f;
		public float Arm_Length = 0.25f;
		public float Arm_Angle = 110.0f;
		public Mesh Arm_L_Mesh;
		public Mesh Arm_R_Mesh;
		public int Arm_Materials_Num = 1;
		public Material[] Arm_Materials;
		public Material Arm_L_Material; // for old versions.
		public Material Arm_R_Material; // for old versions.
	
		public float Wheel_Distance = 2.7f;
		public float Wheel_Mass = 30.0f;
		public float Wheel_Radius = 0.35f;
		public PhysicMaterial Collider_Material;
		public Mesh Wheel_Mesh;
		public int Wheel_Materials_Num = 1;
		public Material[] Wheel_Materials;
		public Material Wheel_Material; // for old versions.
		public bool Drive_Wheel = true;
		public bool Wheel_Resize = true;
		public float ScaleDown_Size = 0.7f;
		public float Return_Speed = 0.05f;

        // For editor script.
        public bool Has_Changed;


        public void Create()
        {
            // Delete Objects
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            // Create Arm and Wheel
            Vector3 pos;
            if (Arm_Flag)
            { //With Arm
              // Create Arms.
                Create_Arm("L", new Vector3(0.0f, Arm_Distance / 2.0f, 0.0f));
                Create_Arm("R", new Vector3(0.0f, -Arm_Distance / 2.0f, 0.0f));
                // Set Wheel Pos.
                pos.x = Mathf.Sin(Mathf.Deg2Rad * (180.0f + Arm_Angle)) * Arm_Length;
                pos.y = Wheel_Distance / 2.0f;
                pos.z = Mathf.Cos(Mathf.Deg2Rad * (180.0f + Arm_Angle)) * Arm_Length;
            }
            else
            { // No Arm
              // Set Wheel Pos.
                pos.x = 0.0f;
                pos.y = Wheel_Distance / 2.0f;
                pos.z = 0.0f;
            }
            // Create Wheels.
            Set_Drive_Wheel_Parent_Script();
            if (Static_Flag == false)
            { //Drive Wheel.
                Remove_Static_Wheel_Parent_Script();
                Create_Physics_Wheel("L", new Vector3(pos.x, pos.y, pos.z));
                Create_Physics_Wheel("R", new Vector3(pos.x, -pos.y, pos.z));
            }
            else
            { //Static Wheel.
                Set_Static_Wheel_Parent_Script();
                Create_Static_Wheel("L", new Vector3(pos.x, pos.y, pos.z));
                Create_Static_Wheel("R", new Vector3(pos.x, -pos.y, pos.z));
                Create_Invisible_Wheel("L", new Vector3(pos.x, pos.y, pos.z));
                Create_Invisible_Wheel("R", new Vector3(pos.x, -pos.y, pos.z));
            }
        }


        void Create_Arm(string direction, Vector3 position)
        {
            //Create gameobject & Set transform
            GameObject armObject = new GameObject("TensionerArm_" + direction);
            armObject.transform.parent = transform;
            armObject.transform.localPosition = position;
            armObject.transform.localRotation = Quaternion.Euler(0.0f, Arm_Angle, -90.0f);
            // Add Mesh
            if (direction == "L")
            { // Left
                if (Arm_L_Mesh)
                {
                    MeshFilter meshFilter = armObject.AddComponent<MeshFilter>();
                    meshFilter.mesh = Arm_L_Mesh;
                }
            }
            else
            { //Right
                if (Arm_R_Mesh)
                {
                    MeshFilter meshFilter = armObject.AddComponent<MeshFilter>();
                    meshFilter.mesh = Arm_R_Mesh;
                }
            }
            MeshRenderer meshRenderer = armObject.AddComponent<MeshRenderer>();
            Material[] materials = new Material[Arm_Materials_Num];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Arm_Materials[i];
            }
            meshRenderer.materials = materials;
        }


        void Create_Physics_Wheel(string direction, Vector3 position)
        {
            GameObject wheelObject = Create_GameObject("IdlerWheel", direction, position);
            Add_Mesh(wheelObject);
            Add_SphereCollider(wheelObject);
            Add_DrivingComponents(wheelObject, direction);
            // Wheel_Resize_CS
            if (Wheel_Resize)
            {
                Wheel_Resize_CS resizeScript;
                resizeScript = wheelObject.AddComponent<Wheel_Resize_CS>();
                resizeScript.ScaleDown_Size = ScaleDown_Size;
                resizeScript.Return_Speed = Return_Speed;
            }
            // Fix_Shaking_Rotation_CS
            Fix_Shaking_Rotation_CS fixScript = wheelObject.AddComponent<Fix_Shaking_Rotation_CS>();
            fixScript.Is_Left = (direction == "L");
            fixScript.This_Transform = wheelObject.transform;
            // Stabilizer_CS
            Stabilizer_CS stabilizerScript = wheelObject.AddComponent<Stabilizer_CS>();
            stabilizerScript.This_Transform = wheelObject.transform;
            stabilizerScript.Is_Left = (direction == "L");
            stabilizerScript.Initial_Pos_Y = wheelObject.transform.localPosition.y;
            stabilizerScript.Initial_Angles = wheelObject.transform.localEulerAngles;
        }


        void Set_Static_Wheel_Parent_Script()
        {
            // Set "Static_Wheel_Parent_CS" in this object.
            Static_Wheel_Parent_CS staticWheelParentScript = GetComponent<Static_Wheel_Parent_CS>();
            if (staticWheelParentScript == null)
            {
                staticWheelParentScript = gameObject.AddComponent<Static_Wheel_Parent_CS>();
            }
            Mesh wheelMesh = Wheel_Mesh;
            float wheelRadius = wheelMesh.bounds.extents.x;
            staticWheelParentScript.Wheel_Radius = wheelRadius + Radius_Offset;
        }


        void Create_Static_Wheel(string direction, Vector3 position)
        {
            GameObject gameObject = Create_GameObject("IdlerWheel", direction, position);
            Add_Mesh(gameObject);
            // Static_Wheel_CS script
            Static_Wheel_CS staticWheelScript = gameObject.AddComponent<Static_Wheel_CS>();
            staticWheelScript.Is_Left = (direction == "L");
            staticWheelScript.Parent_Script = gameObject.GetComponent<Static_Wheel_Parent_CS>();
        }


        void Create_Invisible_Wheel(string direction, Vector3 position)
        {
            GameObject gameObject = Create_GameObject("Invisible_IdlerWheel", direction, position);
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>(); // Set only MeshFilter in order to get the mesh size.
            meshFilter.mesh = Wheel_Mesh;
            Add_SphereCollider(gameObject);
            Add_DrivingComponents(gameObject, direction);
        }


        GameObject Create_GameObject(string name, string direction, Vector3 position)
        {
            GameObject gameObject = new GameObject(name + "_" + direction);
            gameObject.transform.parent = transform;
            gameObject.transform.localPosition = position;
            if (direction == "L")
            {
                gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            }
            gameObject.layer = Layer_Settings_CS.Wheels_Layer;
            return gameObject;
        }


        void Add_Mesh(GameObject gameObject)
        {
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = Wheel_Mesh;
            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            Material[] materials = new Material[Wheel_Materials_Num];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Wheel_Materials[i];
            }
            meshRenderer.materials = materials;
        }


        void Add_SphereCollider(GameObject gameObject)
        {
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = Wheel_Radius;
            sphereCollider.center = Vector3.zero;
            sphereCollider.material = Collider_Material;

        }


        void Remove_Static_Wheel_Parent_Script()
        {
            // Remove "Static_Wheel_Parent_CS" in this object.
            Static_Wheel_Parent_CS staticWheelParentScript = GetComponent<Static_Wheel_Parent_CS>();
            if (staticWheelParentScript)
            {
                EditorApplication.delayCall += () => DestroyImmediate(staticWheelParentScript);
            }
        }


        void Set_Drive_Wheel_Parent_Script()
        {
            // Set "Drive_Wheel_Parent_CS" in this object.
            Drive_Wheel_Parent_CS driveWheelParentScript = GetComponent<Drive_Wheel_Parent_CS>();
            if (driveWheelParentScript == null)
            {
                driveWheelParentScript = gameObject.AddComponent<Drive_Wheel_Parent_CS>();
            }
            driveWheelParentScript.Drive_Flag = Drive_Wheel;
            driveWheelParentScript.Radius = Wheel_Radius;
            driveWheelParentScript.Use_BrakeTurn = true;
        }


        void Add_DrivingComponents(GameObject gameObject, string direction)
        {
            // Rigidbody
            Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.mass = Wheel_Mass;
            // HingeJoint
            HingeJoint hingeJoint;
            hingeJoint = gameObject.AddComponent<HingeJoint>();
            hingeJoint.anchor = Vector3.zero;
            hingeJoint.axis = new Vector3(0.0f, 1.0f, 0.0f);
            hingeJoint.connectedBody = transform.parent.gameObject.GetComponent<Rigidbody>();
            // Drive_Wheel_CS
            Drive_Wheel_CS driveScript = gameObject.AddComponent<Drive_Wheel_CS>();
            driveScript.This_Rigidbody = rigidbody;
            driveScript.Is_Left = (direction == "L");
            driveScript.Parent_Script = transform.GetComponent<Drive_Wheel_Parent_CS>();
        }

    }


}