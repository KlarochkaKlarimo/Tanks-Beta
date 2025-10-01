using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tanks;
using Unity.VisualScripting;

public class TankCreateEditor : EditorWindow
{
    private AudioClip _impactAudioClip;
    private PhysicMaterial _physicMaterial;
    private AudioClip _motorAudioClip;
    private AnimationCurve _recoilBrakeMotion;
    private float _bodyMass;
    private float _firstColiderRadius;
    private float _firstColiderHight;
    private float _secondColiderRadius;
    private float _secondColiderHight;
    private float _susMass=100f;
    private float _susSpring;
    private float _susDamper;
    private float _reinforceRadius;
    private float _anchorOffset;
    private float _susTarget;

    private bool _setIndividually;
    private GameObject _camera_pivot;
  //private GameObject _invisibleWheel;
    private Rigidbody _bodyRigedBody;
    private Vector3 _colidersCenter;
    public Vector3[] cameraPoints;
    private SerializedObject serializedObject;
    private PhysicMaterial Collider_Material;
    [MenuItem("TankBeta/TankCreateEditor")]
    public static void ShowWindow() { GetWindow<TankCreateEditor>(); }
    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
    }
    private void OnGUI()
    {
        _impactAudioClip = (AudioClip)EditorGUILayout.ObjectField("Impact Audio Clip", _impactAudioClip, typeof(AudioClip), false);
        _physicMaterial = (PhysicMaterial)EditorGUILayout.ObjectField("physic material", _physicMaterial, typeof(PhysicMaterial), false);
        _motorAudioClip = (AudioClip)EditorGUILayout.ObjectField("Motor Audio Clip", _motorAudioClip, typeof(AudioClip), false);
        _recoilBrakeMotion = EditorGUILayout.CurveField("Motion", _recoilBrakeMotion, Color.red, new Rect(Vector2.zero, new Vector2(1.0f, 1.0f)));
        _bodyMass = EditorGUILayout.DelayedFloatField("Body mass", _bodyMass);
        EditorGUILayout.LabelField("Track colliders Setings ", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
        _colidersCenter = EditorGUILayout.Vector3Field("Coliders Center", _colidersCenter);
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("First Track collider ");
        _firstColiderRadius = EditorGUILayout.DelayedFloatField("First Colider Radius", _firstColiderRadius);
        _firstColiderHight = EditorGUILayout.DelayedFloatField("First Colider Hight", _firstColiderHight);
        EditorGUILayout.Space(5);
        //EditorGUILayout.LabelField("Second Track collider ");
        //_secondColiderRadius = EditorGUILayout.DelayedFloatField("Second Colider Radius", _secondColiderRadius);
        //_secondColiderHight = EditorGUILayout.DelayedFloatField("Second Colider Hight", _secondColiderHight);
        _susMass = EditorGUILayout.Slider("Suspention mass", _susMass, 0.1f, 300.0f);
        _susSpring = EditorGUILayout.Slider("Sus Spring Force", _susSpring, 0.0f, 100000.0f);
        _susDamper = EditorGUILayout.Slider("Sus Damper", _susDamper, 0.0f, 10000.0f);
        _reinforceRadius = EditorGUILayout.Slider("Reinforce Collider Radius",_reinforceRadius, 0.1f, 1.0f);
        _anchorOffset = EditorGUILayout.Slider("Anchor Offset",_anchorOffset, -1.0f, 1.0f );
        _susTarget = EditorGUILayout.Slider("Sus Spring Target Angle", _susTarget, -90.0f, 90.0f);
        _setIndividually = EditorGUILayout.Toggle("Set Angles Individually", _setIndividually);
        serializedObject.GenerateEditorArray("cameraPoints");
        // _camera_pivot = (GameObject)EditorGUILayout.ObjectField("Camera pivot", _camera_pivot, typeof(GameObject), false);
      //_invisibleWheel = (GameObject)EditorGUILayout.ObjectField("Invisible Wheel", _invisibleWheel, typeof(GameObject), false);
        if (GUILayout.Button("Create"))
        {
            Create();
        }
    }

    private void Create()
    {
        var objs = FindObjectsOfType<ObjectToFind>();
        var body = objs.FirstOrDefault(t => t.GetType() == ObjectType.body);
        var root = objs.FirstOrDefault(t => t.GetType() == ObjectType.root);
        if (body == null)
        {
            Debug.LogError("No body");
            return;
        }
        if (root == null)
        {
            Debug.LogError("No root");
            return;
        }
        _bodyRigedBody = body.gameObject.AddComponent<Rigidbody>();
        body.gameObject.AddComponent<Drive_Control_CS>();
        //body.gameObject.AddComponent<BoxCollider>();
        body.gameObject.layer = 11;
       // var aim = body.gameObject.AddComponent<Aiming_Control_CS>();
       // body.gameObject.AddComponent<UI_Aim_Marker_Control_CS>();
       // body.gameObject.AddComponent<UI_Lead_Marker_Control_CS>();
       // body.gameObject.AddComponent<AudioSource>().clip = _impactAudioClip;
       // body.gameObject.AddComponent<Sound_Control_Impact_CS>();
       //// body.gameObject.AddComponent<BoxCollider>(); // remove this when fix track
       // body.gameObject.AddComponent<MainBody_Setting_CS>();

 
        root.gameObject.AddComponent<ID_Settings_CS>();
        //root.gameObject.AddComponent<Respawn_Controller_CS>();
        //root.gameObject.AddComponent<AI_Headquaters_Helper_CS>();
        //root.gameObject.AddComponent<Special_Settings_CS>();

        _bodyRigedBody.mass = _bodyMass;
        var turretHorizontalScripts = new List<Turret_Horizontal_CS>();
        var cannonVerticalScripts = new List<Cannon_Vertical_CS>();

        foreach (var obj in objs)
        {
            switch (obj.GetType())
            {
                //case ObjectType.turret:
                //    var trurret = obj.gameObject.AddComponent<Turret_Horizontal_CS>();
                //    obj.gameObject.AddComponent<AudioSource>().clip = _motorAudioClip;
                //    obj.gameObject.AddComponent<Sound_Control_Motor_CS>();
                //    turretHorizontalScripts.Add(trurret);
                //    break;
                //case ObjectType.cannon:
                //    var vertical = obj.gameObject.AddComponent<Cannon_Vertical_CS>();
                //    cannonVerticalScripts.Add(vertical);
                //    obj.gameObject.AddComponent<Cannon_Fire_CS>();
                //    obj.gameObject.AddComponent<UI_Reloading_Circle_CS>();
                //    break;
                //case ObjectType.barrel:
                //    obj.gameObject.AddComponent<Recoil_Brake_CS>().Motion_Curve = _recoilBrakeMotion;
                //    break;

                //case ObjectType.idlerWheel:
                //    if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() == null)
                //    {
                //        obj.transform.parent.gameObject.AddComponent<Static_Wheel_Parent_CS>().Wheel_Radius = 0.43f;
                //        obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                //    }
                //    obj.gameObject.AddComponent<Static_Wheel_CS>().Parent_Script = obj.transform.parent.gameObject.GetComponent<Static_Wheel_Parent_CS>();
                //     CreateInvisibleWheel(obj.isLeft?"L":"R");
                //    break;
                //case ObjectType.spoketWheel:
                //    if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() == null)
                //    {
                //        obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                //        obj.transform.parent.gameObject.AddComponent<Static_Wheel_Parent_CS>();
                //    }
                //    obj.gameObject.AddComponent<Static_Wheel_CS>().Parent_Script = obj.transform.parent.gameObject.GetComponent<Static_Wheel_Parent_CS>();
                //    //CreateInvisibleWheel(obj);
                //    break;
                case ObjectType.suspentionR:
                    CreateSus(obj,false);
                    break;
                case ObjectType.suspentionL:
                    CreateSus(obj, true);
                    break;
            }

            void CreateSus(ObjectToFind obj,bool isLeft)
            {
                if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() == null)
                {
                    obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                }
                //if (obj.wheel.gameObject.GetComponent<Drive_Wheel_CS>()!=null) return;
                var driveWheel = obj.wheel.gameObject.AddOrGetComponent<Drive_Wheel_CS>();

                var stabilazer = obj.wheel.gameObject.AddOrGetComponent<Stabilizer_CS>();
                stabilazer.Is_Left = isLeft;
                stabilazer.Initial_Angles = obj.wheel.transform.localEulerAngles;//Here i stop AAAAAAAAAAA
                stabilazer.Initial_Pos_Y = obj.wheel.transform.localPosition.y;
              //  stabilazer.This_Transform = obj.wheel.transform;
                var sus = obj.gameObject;
                sus.AddOrGetComponent<SphereCollider>();


                Rigidbody rigidbody = sus.AddOrGetComponent<Rigidbody>();
                rigidbody.mass = _susMass;
                // HingeJoint
                HingeJoint hingeJoint = sus.AddOrGetComponent<HingeJoint>();
                hingeJoint.connectedBody = _bodyRigedBody;// driveWheel.transform.parent.gameObject.GetComponent<Rigidbody>(); //MainBody's Rigidbody.
                hingeJoint.anchor = new Vector3(0.0f, 0.0f, _anchorOffset);
                hingeJoint.axis = new Vector3(1.0f, 0.0f, 0.0f);
                hingeJoint.useSpring = true;
                JointSpring jointSpring = hingeJoint.spring;
                jointSpring.spring = _susSpring;
                jointSpring.damper = _susDamper;

                hingeJoint.spring = jointSpring;
                hingeJoint.useLimits = true;
                JointLimits jointLimits = hingeJoint.limits;

                hingeJoint.limits = jointLimits;
                // Reinforce SphereCollider
                SphereCollider sphereCollider = sus.AddOrGetComponent<SphereCollider>();
                sphereCollider.radius = _reinforceRadius;
                // Set Layer
                sus.layer = Layer_Settings_CS.Reinforce_Layer;
                AddWheel(driveWheel);

            }
            void AddWheel(Drive_Wheel_CS driveWheel)
            {
                Debug.Log("driveWheel " + driveWheel.name + " sus" + obj.gameObject.name);
                driveWheel.gameObject.AddOrGetComponent<HingeJoint>().connectedBody = obj.gameObject.GetComponent<Rigidbody>();
                driveWheel.gameObject.AddOrGetComponent<SphereCollider>();
                driveWheel.This_Rigidbody = obj.wheel.gameObject.AddOrGetComponent<Rigidbody>();
                if (driveWheel.transform.parent.GetComponent<Drive_Wheel_Parent_CS>() == null)
                {
                    driveWheel.transform.parent.gameObject.AddOrGetComponent<Drive_Wheel_Parent_CS>();
                }
                driveWheel.Parent_Script = driveWheel.transform.parent.GetComponent<Drive_Wheel_Parent_CS>();
                driveWheel.Is_Left = obj.isLeft;
                //driveWheel.This_Rigidbody = driveWheel.gameObject.GetComponent<Rigidbody>();
            }
            void CreateInvisibleWheel(/*ObjectToFind obj*/string direction)
            {
                //var invisibleWheel = Instantiate(_invisibleWheel, obj.transform.parent);
                //invisibleWheel.GetComponent<SphereCollider>().radius = 0.43f;
                //invisibleWheel.GetComponent<HingeJoint>().connectedBody = _bodyRigedBody;
                //invisibleWheel.GetComponent<Drive_Wheel_CS>().This_Rigidbody = invisibleWheel.GetComponent<Rigidbody>();
                //invisibleWheel.GetComponent<Drive_Wheel_CS>().Is_Left = obj.isLeft;
                //invisibleWheel.GetComponent<Drive_Wheel_CS>().Parent_Script = obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>();
                //invisibleWheel.transform.position = obj.transform.position;
                GameObject gameObject = Create_GameObject("Invisible_IdlerWheel", direction);
                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>(); // Set only MeshFilter in order to get the mesh size.
                meshFilter.mesh = obj.transform.gameObject.GetComponent<MeshFilter>().sharedMesh;
                Add_SphereCollider(gameObject);
                Add_DrivingComponents(gameObject, direction);
            }
            GameObject Create_GameObject(string name, string direction)
            {
                GameObject gameObject = new GameObject(name + "_" + direction);
                gameObject.transform.parent = obj.transform.parent;
                gameObject.transform.localPosition = obj.transform.localPosition;
                //if (direction == "L")
                //{
                //    gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //}
                //else
                //{
                //    gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                //}
                gameObject.layer = Layer_Settings_CS.Wheels_Layer;
                return gameObject;
            }
            void Add_DrivingComponents(GameObject gameObject, string direction)
            {
                // Rigidbody
                Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.mass = 200f; //Wheel_MassProp.floatValue;
                // HingeJoint
                HingeJoint hingeJoint;
                hingeJoint = gameObject.AddComponent<HingeJoint>();
                hingeJoint.anchor = Vector3.zero;
                hingeJoint.axis = new Vector3(0.0f, 1.0f, 0.0f);
                hingeJoint.connectedBody = _bodyRigedBody;
                // Drive_Wheel_CS
                Drive_Wheel_CS driveScript = gameObject.AddComponent<Drive_Wheel_CS>();
                driveScript.This_Rigidbody = rigidbody;
                driveScript.Is_Left = (direction == "L");
                driveScript.Parent_Script = obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>();
            }
            void Add_SphereCollider(GameObject gameObject)
            {
                SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
                sphereCollider.radius = 0.43f;
                sphereCollider.center = Vector3.zero;
                sphereCollider.material = Collider_Material;
            }
        }
        // aim.cannonVerticalScripts = cannonVerticalScripts.ToArray();
        // aim.turretHorizontalScripts = turretHorizontalScripts.ToArray();
    }
}