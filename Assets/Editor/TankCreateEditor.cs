using UnityEditor;
using UnityEngine;
using ChobiAssets.PTM;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;

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
        var isTrackCreated = false;
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
                if (obj.transform.parent.gameObject.GetComponent<Turret_Horizontal_CS>())
                {
                    obj.gameObject.AddComponent<Turret_Horizontal_CS>();
                    obj.gameObject.AddComponent<AudioSource>().clip = _motorAudioClip;
                    obj.gameObject.AddComponent<Sound_Control_Motor_CS>();
                }
                break;

            case ObjectType.cannon:
                if (obj.transform.parent.gameObject.GetComponent<Cannon_Fire_CS>())
                {
                    obj.gameObject.AddComponent<Cannon_Vertical_CS>();
                    obj.gameObject.AddComponent<Cannon_Fire_CS>();
                    obj.gameObject.AddComponent<UI_Reloading_Circle_CS>();
                }
                break;

            case ObjectType.barrel:
                if (obj.transform.parent.gameObject.GetComponent<Recoil_Brake_CS>() != null)
                {
                    obj.gameObject.AddComponent<Recoil_Brake_CS>().Motion_Curve = _recoilBrakeMotion;
                }                   
                break;

            case ObjectType.track:
                if(obj.transform.parent.gameObject.GetComponent<Static_Track_Parent_CS>() != null)
                {
                    obj.transform.parent.gameObject.AddComponent<Static_Track_Parent_CS>();
                    var tracks = obj.transform.parent.gameObject.GetComponentsInChildren<ObjectToFind>();
                    var tracksL = new List<ObjectToFind>();
                    var tracksR = new List<ObjectToFind>();

                    foreach(var track in tracks)
                    {
                        if (track.isLeft)
                        {
                            tracksL.Add(track);
                        }

                        else
                        {
                            tracksR.Add(track);
                        }
                    }

                    ParceTracks(tracksL);
                    ParceTracks(tracksR);

                    void ParceTracks(List<ObjectToFind> list)
                    {
                        for (int i = 0; i>list.Count; i++)
                        {
                            var trackPiece = list.Where(t => t.trackNumber == i).FirstOrDefault();
                            var nextTrackPiece = list.Where(t => t.trackNumber == i++).FirstOrDefault();
                            if(nextTrackPiece != null)
                            {
                                var staticTrackPiece = trackPiece.AddComponent<Static_Track_Piece_CS>();
                                var staticNextTrackPiece = nextTrackPiece.AddComponent<Static_Track_Piece_CS>();
                                staticNextTrackPiece.Rear_Script = staticTrackPiece;
                                staticNextTrackPiece.Rear_Transform = staticTrackPiece.transform;
                                staticTrackPiece.Front_Script = staticNextTrackPiece;
                                staticTrackPiece.Front_Transform = staticNextTrackPiece.transform;
                            }
                        }
                    }
                }
                var first = obj.gameObject.AddComponent<CapsuleCollider>();
                first.height = _firstColiderHight;
                first.radius = _firstColiderRadius;
                first.center = _colidersCenter;
                first.enabled = false;
                var second = obj.gameObject.AddComponent<CapsuleCollider>();
                second.height = _secondColiderHight;
                second.radius = _secondColiderRadius;
                second.center = _colidersCenter;
                second.enabled = false;               
                break;

            case ObjectType.idlerWheel:
                if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() != null)
                {
                    obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                    obj.transform.parent.gameObject.AddComponent<Static_Wheel_Parent_CS>();
                }
                obj.gameObject.AddComponent<Static_Wheel_CS>();
                break;

            case ObjectType.spoketWheel:
                if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() != null)
                {
                    obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                    obj.transform.parent.gameObject.AddComponent<Static_Wheel_Parent_CS>();
                }
                obj.gameObject.AddComponent<Static_Wheel_CS>();
                break;
            
            case ObjectType.suspentionR:
                if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() != null)
                {
                    obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                }
                var rbR = obj.gameObject.AddComponent<Rigidbody>();
                obj.gameObject.AddComponent<HingeJoint>().connectedBody = _bodyRigedBody;
                obj.gameObject.AddComponent<SphereCollider>();
                var wheelRbR = obj.wheel.gameObject.AddComponent<Rigidbody>();
                obj.wheel.gameObject.AddComponent<HingeJoint>().connectedBody = rbR;
                obj.wheel.gameObject.AddComponent<SphereCollider>();
                var driveWheelR = obj.wheel.gameObject.AddComponent<Drive_Wheel_CS>();
                driveWheelR.This_Rigidbody = wheelRbR;
                if (driveWheelR.transform.parent.GetComponent<Drive_Wheel_Parent_CS>() == null)
                {
                    driveWheelR.transform.parent.AddComponent<Drive_Wheel_Parent_CS>();
                }
                driveWheelR.Parent_Script = driveWheelR.transform.parent.GetComponent<Drive_Wheel_Parent_CS>();
                driveWheelR.Is_Left = true;
                var stabilazerR = obj.wheel.gameObject.AddComponent<Stabilizer_CS>();
                stabilazerR.Is_Left = true;
                stabilazerR.Initial_Angles = Vector3.zero;
                stabilazerR.Initial_Pos_Y = stabilazerR.transform.position.y;
                stabilazerR.This_Transform = stabilazerR.transform;
                break;

            case ObjectType.suspentionL:
                if (obj.transform.parent.gameObject.GetComponent<Drive_Wheel_Parent_CS>() != null)
                {
                    obj.transform.parent.gameObject.AddComponent<Drive_Wheel_Parent_CS>();
                }
                var rbL = obj.gameObject.AddComponent<Rigidbody>();
                obj.gameObject.AddComponent<HingeJoint>().connectedBody = _bodyRigedBody;
                obj.gameObject.AddComponent<SphereCollider>();
                var wheelRbL = obj.wheel.gameObject.AddComponent<Rigidbody>();
                obj.wheel.gameObject.AddComponent<HingeJoint>().connectedBody = rbL;
                obj.wheel.gameObject.AddComponent<SphereCollider>();
                var driveWheelL = obj.wheel.gameObject.AddComponent<Drive_Wheel_CS>();
                driveWheelL.This_Rigidbody = wheelRbL;
                if (driveWheelL.transform.parent.GetComponent<Drive_Wheel_Parent_CS>() == null)
                {
                    driveWheelL.transform.parent.AddComponent<Drive_Wheel_Parent_CS>();
                }
                driveWheelL.Parent_Script = driveWheelL.transform.parent.GetComponent<Drive_Wheel_Parent_CS>();
                driveWheelL.Is_Left = true;
                var stabilazerL = obj.wheel.gameObject.AddComponent<Stabilizer_CS>();
                stabilazerL.Is_Left = true;
                stabilazerL.Initial_Angles = Vector3.zero;
                stabilazerL.Initial_Pos_Y = stabilazerL.transform.position.y;
                stabilazerL.This_Transform = stabilazerL.transform;
                break;

            case ObjectType.supportWheel:
                if (obj.transform.parent.gameObject.GetComponent<Static_Wheel_Parent_CS>() != null)
                {                  
                    obj.transform.parent.gameObject.AddComponent<Static_Wheel_Parent_CS>();
                }
                obj.gameObject.AddComponent<SphereCollider>();
                obj.gameObject.AddComponent<Static_Wheel_CS>();
                break;
            }
        }
    }
}