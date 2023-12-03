using UnityEngine;

namespace ChobiAssets.PTM
{

    public class Create_TrackBelt_CS : MonoBehaviour
    {
        /* 
		 * This script is attached to the "Create_TrackBelt" in the tank.
		 * This script is used for creating and setting the "Physics_Track" by the editor script.
		*/

        public bool Rear_Flag = false;
        public int SelectedAngle = 3600;
        public int Angle_Rear = 4500;
        public int Number_Straight = 17;
        public float Spacing = 0.3f;
        public float Distance = 2.7f;
        public float Track_Mass = 30.0f;
        public float Angular_Drag = 50.0f;
        public Bounds Collider_Info = new Bounds(new Vector3(0.0f, -0.016f, 0.0f), new Vector3(0.65f, 0.08f, 0.3f));
        public PhysicMaterial Collider_Material;
        public bool Use_BoxCollider;

        public Mesh Track_R_Mesh;
        public Mesh Track_L_Mesh;
        public int Track_Materials_Num = 1;
        public Material[] Track_Materials;
        public bool Use_2ndPiece = false;
        public Mesh Track_R_2nd_Mesh;
        public Mesh Track_L_2nd_Mesh;
        public int Track_2nd_Materials_Num = 1;
        public Material[] Track_2nd_Materials;
        public bool Use_ShadowMesh = false;
        public Mesh Track_R_Shadow_Mesh;
        public Mesh Track_L_Shadow_Mesh;

        public int SubJoint_Type = 1;
        public float Reinforce_Radius = 0.3f;

        public bool Use_Joint = false;
        public float Joint_Offset;
        public Mesh Joint_R_Mesh;
        public Mesh Joint_L_Mesh;
        public int Joint_Materials_Num = 1;
        public Material[] Joint_Materials;
        public bool Joint_Shadow = true;

        public float BreakForce = 5000.0f;

        public bool Static_Flag = false;

        // For editor script.
        public bool Has_Changed;


        void Start()
        {
            if (Static_Flag)
            { // For creating Static_Track.
                Control_Rigidbody();
            }
            else
            {
                Destroy(this);
            }
        }


        void Control_Rigidbody()
        {
            Rigidbody parentRigidbody = transform.parent.GetComponent<Rigidbody>();
            parentRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            parentRigidbody.drag = 15.0f;
        }
      public void Create()
      {
            // Delete Objects
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            // Create Track Pieces	(Preparation)
            float frontAng = SelectedAngle / 100.0f;
            float rearAng = Angle_Rear / 100.0f;
            float frontRad = Spacing / (2.0f * Mathf.Tan(Mathf.PI / (360.0f / frontAng)));
            float rearRad = Spacing / (2.0f * Mathf.Tan(Mathf.PI / (360.0f / rearAng)));
            float height = frontRad - rearRad;
            float bottom;
            float slopeAngle;
            if (Mathf.Abs(height) > Spacing * Number_Straight || Number_Straight == 0)
            {
                bottom = 0.0f;
                slopeAngle = 0.0f;
            }
            else
            {
                slopeAngle = Mathf.Asin(height / (Spacing * Number_Straight));
                if (slopeAngle != 0.0f)
                {
                    bottom = height / Mathf.Tan(slopeAngle);
                }
                else
                {
                    bottom = Spacing * Number_Straight;
                }
                slopeAngle *= Mathf.Rad2Deg;
            }
            // Create Front Arc
            int num = 0;
            Vector3 centerPos;
            centerPos.x = frontRad;
            centerPos.y = Distance / 2.0f;
            centerPos.z = 0.0f;
            Vector3 pos;
            pos.y = Distance / 2.0f;
            for (int i = 0; i <= 180 / frontAng; i++)
            {
                num++;
                pos.x = frontRad * Mathf.Sin(Mathf.Deg2Rad * (270.0f + (frontAng * i)));
                pos.x += centerPos.x;
                pos.z = frontRad * Mathf.Cos(Mathf.Deg2Rad * (270.0f + (frontAng * i)));
                Create_TrackPiece("L", new Vector3(pos.x, pos.y, pos.z), i * frontAng, num);
                Create_TrackPiece("R", new Vector3(pos.x, -pos.y, pos.z), i * frontAng, num);
            }
            // Create Upper Straight
            if (bottom != 0.0f)
            {
                centerPos.x = (frontRad * 2.0f) - (height / Number_Straight / 2.0f);
                centerPos.z = -((Spacing / 2.0f) + (bottom / Number_Straight / 2.0f));
                for (int i = 0; i < Number_Straight; i++)
                {
                    num++;
                    pos.x = centerPos.x - (height / Number_Straight * i);
                    pos.z = centerPos.z - (bottom / Number_Straight * i);
                    Create_TrackPiece("L", new Vector3(pos.x, pos.y, pos.z), 180.0f + slopeAngle, num);
                    Create_TrackPiece("R", new Vector3(pos.x, -pos.y, pos.z), 180.0f + slopeAngle, num);
                }
            }
            // Create Rear Arc
            centerPos.x = frontRad;
            centerPos.z = -(bottom + Spacing);
            for (int i = 0; i <= 180 / rearAng; i++)
            {
                num++;
                pos.x = rearRad * Mathf.Sin(Mathf.Deg2Rad * (90.0f + (rearAng * i)));
                pos.x += centerPos.x;
                pos.z = rearRad * Mathf.Cos(Mathf.Deg2Rad * (90.0f + (rearAng * i)));
                pos.z += centerPos.z;
                Create_TrackPiece("L", new Vector3(pos.x, pos.y, pos.z), 180.0f + (i * rearAng), num);
                Create_TrackPiece("R", new Vector3(pos.x, -pos.y, pos.z), 180.0f + (i * rearAng), num);
            }
            // Create lower Straight
            if (bottom != 0.0f)
            {
                centerPos.x = (frontRad - rearRad) - (height / Number_Straight / 2.0f);
                centerPos.z = -(bottom + (Spacing / 2.0f)) + (bottom / Number_Straight / 2.0f);
                for (int i = 0; i < Number_Straight; i++)
                {
                    num++;
                    pos.x = centerPos.x - (height / Number_Straight * i);
                    pos.z = centerPos.z + (bottom / Number_Straight * i);
                    Create_TrackPiece("L", new Vector3(pos.x, pos.y, pos.z), -slopeAngle, num);
                    Create_TrackPiece("R", new Vector3(pos.x, -pos.y, pos.z), -slopeAngle, num);
                }
            }
            //Create Shadow Mesh.
            if (Use_ShadowMesh)
            {
                for (int i = 0; i < num; i++)
                {
                    Create_ShadowMesh("L", i + 1);
                    Create_ShadowMesh("R", i + 1);
                }
            }
            // Create Reinforce Collider.
            if (SubJoint_Type != 2)
            {
                for (int i = 0; i < num; i++)
                {
                    if (SubJoint_Type == 0 || (i + 1) % 2 == 0)
                    {
                        Create_Reinforce("L", i + 1);
                        Create_Reinforce("R", i + 1);
                    }
                }
            }
            // Create Additional Joint.
            if (Use_Joint)
            {
                for (int i = 0; i < num; i++)
                {
                    Create_Joint("L", i + 1);
                    Create_Joint("R", i + 1);
                }
            }
            // Add RigidBody and Joint.
            Finishing("L");
            Finishing("R");

            void Create_TrackPiece(string direction, Vector3 position, float angleY, int Number_Straight)
            {
                //Create gameobject & Set transform
                GameObject pieceObject = new GameObject("TrackBelt_" + direction + "_" + Number_Straight);
                pieceObject.transform.parent = transform;
                pieceObject.transform.localPosition = position;
                pieceObject.transform.localRotation = Quaternion.Euler(0.0f, angleY, -90.0f);
                // Mesh
                MeshFilter meshFilter = pieceObject.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = pieceObject.AddComponent<MeshRenderer>();
                if (Use_ShadowMesh)
                {
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                if (direction == "L")
                {
                    if (Use_2ndPiece && Number_Straight % 2 == 0)
                    {
                        meshFilter.mesh = Track_L_2nd_Mesh;
                    }
                    else
                    {
                        meshFilter.mesh = Track_L_Mesh;
                    }
                }
                else
                {
                    if (Use_2ndPiece && Number_Straight % 2 == 0)
                    {
                        meshFilter.mesh = Track_R_2nd_Mesh;
                    }
                    else
                    {
                        meshFilter.mesh = Track_R_Mesh;
                    }
                }
                if (Use_2ndPiece && Number_Straight % 2 == 0)
                {
                    Material[] materials = new Material[Track_2nd_Materials_Num];
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i] = Track_2nd_Materials[i];
                    }
                    meshRenderer.materials = materials;
                }
                else
                {
                    Material[] materials = new Material[Track_Materials_Num];
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i] = Track_Materials[i];
                    }
                    meshRenderer.materials = materials;
                }
                // Collider
                if (Use_BoxCollider)
                {
                    // Box Collider
                    BoxCollider boxCollider = pieceObject.AddComponent<BoxCollider>();
                    boxCollider.center = Collider_Info.center;
                    boxCollider.size = Collider_Info.size;
                    boxCollider.material = Collider_Material;
                }
                else
                {
                    // CapsuleColliders
                    CapsuleCollider horizontalCollider = pieceObject.AddComponent<CapsuleCollider>();
                    horizontalCollider.center = Collider_Info.center;
                    if (direction == "R")
                    {
                        horizontalCollider.center = new Vector3(-horizontalCollider.center.x, horizontalCollider.center.y, horizontalCollider.center.z);
                    }
                    horizontalCollider.direction = 0; // X-Axis
                    horizontalCollider.radius = Collider_Info.extents.y;
                    horizontalCollider.height = Collider_Info.size.x;
                    horizontalCollider.material = Collider_Material;
                    CapsuleCollider verticalCollider = pieceObject.AddComponent<CapsuleCollider>();
                    verticalCollider.center = Collider_Info.center;
                    if (direction == "R")
                    {
                        verticalCollider.center = new Vector3(-verticalCollider.center.x, verticalCollider.center.y, verticalCollider.center.z);
                    }
                    verticalCollider.direction = 2; // Z-Axis
                    verticalCollider.radius = Collider_Info.extents.y;
                    verticalCollider.height = Collider_Info.size.z;
                    verticalCollider.material = Collider_Material;
                }
                // Stabilizer_CS
                Stabilizer_CS stabilizerScript = pieceObject.AddComponent<Stabilizer_CS>();
                stabilizerScript.This_Transform = pieceObject.transform;
                stabilizerScript.Is_Left = (direction == "L");
                stabilizerScript.Initial_Pos_Y = pieceObject.transform.localPosition.y;
                stabilizerScript.Initial_Angles = pieceObject.transform.localEulerAngles;
                // Damage_Control_02_Physics_Track_Piece_CS
                var damageScript = pieceObject.AddComponent<Damage_Control_03_Physics_Track_Piece_CS>();
                if (direction == "L")
                { // Left
                    damageScript.Track_Index = 0;
                }
                else
                { // Right
                    damageScript.Track_Index = 1;
                }
                // Static_Track_Setting_CS
                if (Static_Flag)
                {
                    Static_Track_Setting_CS settingScript = pieceObject.AddComponent<Static_Track_Setting_CS>();
                    settingScript.Use_2ndPiece = Use_2ndPiece;
                }
                // Set Layer
                pieceObject.layer = 0;
            }
            void Create_ShadowMesh(string direction, int Number_Straight)
            {
                //Create gameobject & Set transform
                Transform basePiece = transform.Find("TrackBelt_" + direction + "_" + Number_Straight);
                GameObject shadowObject = new GameObject("ShadowMesh_" + direction + "_" + Number_Straight);
                shadowObject.transform.position = basePiece.position;
                shadowObject.transform.rotation = basePiece.rotation;
                shadowObject.transform.parent = basePiece;
                // Mesh
                MeshFilter meshFilter = shadowObject.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = shadowObject.AddComponent<MeshRenderer>();
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                meshRenderer.receiveShadows = false;
                if (direction == "R")
                {
                    meshFilter.mesh = Track_R_Shadow_Mesh;
                }
                else
                {
                    meshFilter.mesh = Track_L_Shadow_Mesh;
                }
                Material[] materials = new Material[Track_Materials_Num];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = Track_Materials[i];
                }
                meshRenderer.materials = materials;
            }
            void Create_Reinforce(string direction, int Number_Straight)
            {
                //Create gameobject & Set transform
                Transform basePiece = transform.Find("TrackBelt_" + direction + "_" + Number_Straight);
                GameObject reinforceObject = new GameObject("Reinforce_" + direction + "_" + Number_Straight);
                reinforceObject.transform.position = basePiece.position;
                reinforceObject.transform.rotation = basePiece.rotation;
                reinforceObject.transform.parent = basePiece;
                // SphereCollider
                SphereCollider sphereCollider = reinforceObject.AddComponent<SphereCollider>();
                sphereCollider.radius = Reinforce_Radius;
                // Set Layer
                reinforceObject.layer = Layer_Settings_CS.Reinforce_Layer;
            }
            void Create_Joint(string direction, int Number_Straight)
            {
                //Create gameobject & Set transform
                Transform baseTransform = transform.Find("TrackBelt_" + direction + "_" + Number_Straight);
                Transform frontTransform = transform.Find("TrackBelt_" + direction + "_" + (Number_Straight + 1));
                if (frontTransform == null)
                {
                    frontTransform = transform.Find("TrackBelt_" + direction + "_1");
                }
                GameObject jointObject = new GameObject("Joint_" + direction + "_" + Number_Straight);
                jointObject.transform.parent = baseTransform;
                Vector3 basePos = baseTransform.position + (baseTransform.forward * Joint_Offset);
                Vector3 frontPos = frontTransform.position - (frontTransform.forward * Joint_Offset);
                jointObject.transform.position = Vector3.Lerp(basePos, frontPos, 0.5f);
                jointObject.transform.rotation = Quaternion.Lerp(baseTransform.rotation, frontTransform.rotation, 0.5f);
                // Mesh
                MeshFilter meshFilter = jointObject.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = jointObject.AddComponent<MeshRenderer>();
                if (Joint_Shadow == false)
                {
                    meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
                if (direction == "R")
                {
                    meshFilter.mesh = Joint_R_Mesh;
                }
                else
                {
                    meshFilter.mesh = Joint_L_Mesh;
                }
                Material[] materials = new Material[Joint_Materials_Num];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = Joint_Materials[i];
                }
                meshRenderer.materials = materials;
                // Track_Joint_CS
                Track_Joint_CS jointScript = jointObject.AddComponent<Track_Joint_CS>();
                jointScript.Base_Transform = baseTransform;
                jointScript.Front_Transform = frontTransform;
                jointScript.Joint_Offset = Joint_Offset;
                jointScript.Is_Left = (direction == "L");
            }
            void Finishing(string direction)
            {
                // Add RigidBody.
                for (int i = 1; i <= transform.childCount; i++)
                {
                    Transform basePiece = transform.Find("TrackBelt_" + direction + "_" + i);
                    if (basePiece)
                    {
                        // Add RigidBody.
                        Rigidbody rigidbody = basePiece.gameObject.AddComponent<Rigidbody>();
                        rigidbody.mass = Track_Mass;
                        rigidbody.angularDrag = Angular_Drag;
                        // for Static_Track creating
                        if (Static_Flag)
                        {
                            rigidbody.drag = 10.0f;
                        }
                    }
                }
                // Add HingeJoint.
                for (int i = 1; i <= transform.childCount; i++)
                {
                    Transform basePiece = transform.Find("TrackBelt_" + direction + "_" + i);
                    if (basePiece)
                    {
                        HingeJoint hingeJoint = basePiece.gameObject.AddComponent<HingeJoint>();
                        hingeJoint.anchor = new Vector3(0.0f, 0.0f, Spacing / 2.0f);
                        hingeJoint.axis = new Vector3(1.0f, 0.0f, 0.0f);
                        hingeJoint.breakForce = BreakForce;
                        Transform frontPiece = transform.Find("TrackBelt_" + direction + "_" + (i + 1));
                        if (frontPiece)
                        {
                            hingeJoint.connectedBody = frontPiece.GetComponent<Rigidbody>();
                        }
                        else
                        {
                            frontPiece = transform.Find("TrackBelt_" + direction + "_1");
                            if (frontPiece)
                            {
                                hingeJoint.connectedBody = frontPiece.GetComponent<Rigidbody>();
                            }
                        }
                    }
                }
            }
        }
    }
}
