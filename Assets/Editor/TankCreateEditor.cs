using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using Unity.VisualScripting;
public class TankCreateEditor : EditorWindow
{
    private AudioClip _impactAudioClip;
    private AudioClip _motorAudioClip;
    private AnimationCurve _recoilBrakeMotion;
    private float _bodyMass;
    private float _firstColiderRadius;
    private float _firstColiderHight;
    private float _secondColiderRadius;
    private float _secondColiderHight;
    private Rigidbody _bodyRigedBody;
    private Vector3 _colidersCenter;
    [MenuItem("TankBeta/TankCreateEditor")]
    public static void ShowWindow() { GetWindow<TankCreateEditor>(); }

    private void OnGUI()
    {
        _impactAudioClip = (AudioClip)EditorGUILayout.ObjectField("impact Audio Clip", _impactAudioClip, typeof(AudioClip), false);
        _motorAudioClip = (AudioClip)EditorGUILayout.ObjectField("Motor Audio Clip", _motorAudioClip, typeof(AudioClip), false);
        _recoilBrakeMotion = EditorGUILayout.CurveField("Motion", _recoilBrakeMotion, Color.red, new Rect(Vector2.zero, new Vector2(1.0f, 1.0f)));
        _bodyMass = EditorGUILayout.DelayedFloatField("body mass", _bodyMass);
        EditorGUILayout.LabelField("Track colliders Setings ", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
        _colidersCenter = EditorGUILayout.Vector3Field("coliders Center", _colidersCenter);
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("first Track collider ");
        _firstColiderRadius = EditorGUILayout.DelayedFloatField("first Colider Radius", _firstColiderRadius);
        _firstColiderHight = EditorGUILayout.DelayedFloatField("first Colider Hight", _firstColiderRadius);
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("second Track collider ");
        _secondColiderRadius = EditorGUILayout.DelayedFloatField("second Colider Radius", _secondColiderRadius);
        _secondColiderHight = EditorGUILayout.DelayedFloatField("second Colider Hight", _secondColiderHight);

        if (GUILayout.Button("Create"))
        {
            Create();
        }
    }

    private void Create()
    {
      var objs = FindObjectsOfType<ObjectToFind>();
      foreach (var obj in objs)
      {
            
        switch (obj.GetType())
        {
            case ObjectType.body:
                obj.gameObject.AddComponent<Drive_Control_CS>();
                obj.gameObject.AddComponent<Aiming_Control_CS>();
                obj.gameObject.AddComponent<UI_Aim_Marker_Control_CS>();
                obj.gameObject.AddComponent<UI_Lead_Marker_Control_CS>();
                obj.gameObject.AddComponent<AudioSource>().clip = _impactAudioClip;
                obj.gameObject.AddComponent<Sound_Control_Impact_CS>();
                _bodyRigedBody = obj.gameObject.AddComponent<Rigidbody>();
                _bodyRigedBody.mass = _bodyMass;
                break;
            case ObjectType.turret:
                obj.gameObject.AddComponent<Turret_Horizontal_CS>();
                obj.gameObject.AddComponent<AudioSource>().clip = _motorAudioClip;
                obj.gameObject.AddComponent<Sound_Control_Motor_CS>();
                break;
            case ObjectType.cannon:
                obj.gameObject.AddComponent<Cannon_Vertical_CS>();
                obj.gameObject.AddComponent<Cannon_Fire_CS>();
                obj.gameObject.AddComponent<UI_Reloading_Circle_CS>();
                break;
            case ObjectType.barrel:
                obj.gameObject.AddComponent<Recoil_Brake_CS>().Motion_Curve = _recoilBrakeMotion;
                break;
            case ObjectType.track:
                var first = obj.gameObject.AddComponent<CapsuleCollider>();
                first.height = _firstColiderHight;
                first.radius = _firstColiderRadius;
                first.center = _colidersCenter;
                var second = obj.gameObject.AddComponent<CapsuleCollider>();
                second.height = _secondColiderHight;
                second.radius = _secondColiderRadius;
                second.center = _colidersCenter;
                obj.gameObject.AddComponent<Static_Track_CS>();
                break;
            case ObjectType.spoketWheel:
                obj.gameObject.AddComponent<Static_Wheel_CS>();
                break;
            case ObjectType.idlerWheel:
                obj.gameObject.AddComponent<Static_Wheel_CS>();
                break;
            case ObjectType.suspentionR:
                obj.gameObject.AddComponent<Rigidbody>();
                obj.gameObject.AddComponent<HingeJoint>().connectedBody = _bodyRigedBody;
                obj.gameObject.AddComponent<SphereCollider>();
                break;
            case ObjectType.suspentionL:
               var rb = obj.gameObject.AddComponent<Rigidbody>();
               obj.gameObject.AddComponent<HingeJoint>().connectedBody = _bodyRigedBody;
               obj.gameObject.AddComponent<SphereCollider>();
              var wheelRb = obj.wheel.gameObject.AddComponent<Rigidbody>();
               obj.wheel.gameObject.AddComponent<HingeJoint>().connectedBody = rb;
               obj.wheel.gameObject.AddComponent<SphereCollider>();
               var driveWheel = obj.wheel.gameObject.AddComponent<Drive_Wheel_CS>();
               driveWheel.This_Rigidbody = wheelRb;
                if (driveWheel.transform.parent.GetComponent<Drive_Wheel_Parent_CS>() == null)
                {
                    driveWheel.transform.parent.AddComponent<Drive_Wheel_Parent_CS>();
                }
                driveWheel.Parent_Script = driveWheel.transform.parent.GetComponent<Drive_Wheel_Parent_CS>();
                driveWheel.Is_Left = true;
                var stabilazer = obj.wheel.gameObject.AddComponent<Stabilizer_CS>();
                stabilazer.Is_Left = true;
                stabilazer.Initial_Angles = Vector3.zero;
                stabilazer.Initial_Pos_Y = stabilazer.transform.position.y;
                stabilazer.This_Transform = stabilazer.transform;
                break;
        }
      }
    }
}