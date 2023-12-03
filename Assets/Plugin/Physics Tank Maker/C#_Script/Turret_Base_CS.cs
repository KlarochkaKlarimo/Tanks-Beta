using UnityEngine;
using System.Collections;

namespace ChobiAssets.PTM
{

	public class Turret_Base_CS : MonoBehaviour
	{
		/* 
		 * This script is attached to the "Turret_Base" in the tank.
		 * This script is used for creating and setting the turret by the editor script.
		*/

		public Mesh Part_Mesh;

		public int Colliders_Num;
		public Mesh[] Colliders_Mesh;
		public Mesh Collider_Mesh; // for old versions.
		public Mesh Sub_Collider_Mesh; // for old versions.

		public int Materials_Num = 1;
		public Material[] Materials;
		public Material Part_Material;

		public float Offset_X = 0.0f;
		public float Offset_Y = 0.0f;
		public float Offset_Z = 0.0f;

		public bool Use_Damage_Control = true;
		public int Turret_Index;

		public GameObject Damage_Effect_Object; // for old versions.


        // For editor script.
        public bool Has_Changed;
        void Start ()
		{
			Destroy (this);
		}
       public void Create()
        {
            Transform oldTransform = transform.Find("Turret"); // Find the old object.
            int childCount;
            Transform[] childTransforms;
            if (oldTransform)
            {
                childCount = oldTransform.transform.childCount;
                childTransforms = new Transform[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    childTransforms[i] = oldTransform.GetChild(0); // Get the child object such as "Armor_Collider".
                    childTransforms[i].parent = transform; // Change the parent of the child object.
                }
                DestroyImmediate(oldTransform.gameObject); // Delete old object.
            }
            else
            {
                childCount = 0;
                childTransforms = null;
            }

            // Create new Gameobject & Set Transform.
            GameObject newObject = new GameObject("Turret");
            newObject.transform.parent = transform;
            newObject.transform.localPosition = -transform.localPosition + new Vector3(Offset_X, Offset_Y, Offset_Z);
            newObject.transform.localRotation = Quaternion.identity;

            // Mesh settings.
            MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();
            Material[] materials = new Material[Materials_Num];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Materials[i];
            }
            meshRenderer.materials = materials;
            MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
            meshFilter.mesh = Part_Mesh;

            // Collider settings.
            for (int i = 0; i < Colliders_Num; i++)
            {
                MeshCollider meshCollider = newObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = Colliders_Mesh[i];
                meshCollider.convex = true;
            }

            // Add "Damage_Control_01_Turret_CS" script.
            if (Use_Damage_Control)
            {
                var damageScript = newObject.AddComponent<Damage_Control_02_Turret_CS>();
                damageScript.Turret_Index = Turret_Index;
                // Update the "Turret_Index" value of the "Cannon_Base_CS", "Barrel_Base_CS", and "Damage_Control_01_Turret_CS" in the "Cannon" and "Barrel".
                Cannon_Base_CS cannonScript = transform.parent.GetComponentInChildren<Cannon_Base_CS>();
                if (cannonScript)
                {
                    cannonScript.Turret_Index = Turret_Index;
                    Transform cannonTransform = cannonScript.transform.Find("Cannon");
                    if (cannonTransform)
                    {
                        var cannonDamageScript = cannonTransform.GetComponent<Damage_Control_02_Turret_CS>();
                        if (cannonDamageScript)
                        {
                            cannonDamageScript.Turret_Index = Turret_Index;
                        }
                    }
                }
                Barrel_Base_CS[] barrelScripts = transform.parent.GetComponentsInChildren<Barrel_Base_CS>();
                foreach (Barrel_Base_CS barrelScript in barrelScripts)
                {
                    barrelScript.Turret_Index = Turret_Index;
                    Transform barrelTransform = barrelScript.transform.Find("Barrel");
                    if (barrelTransform)
                    {
                        var barrelDamageScript = barrelTransform.GetComponent<Damage_Control_02_Turret_CS>();
                        if (barrelDamageScript)
                        {
                            barrelDamageScript.Turret_Index = Turret_Index;
                        }
                    }
                }
            }

            // Set the layer
            newObject.layer = 0;

            // Return the child objects.
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    childTransforms[i].transform.parent = newObject.transform;
                }
            }
        }
    }

}